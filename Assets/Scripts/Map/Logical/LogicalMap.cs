using System;

namespace HordeEngine
{
    public class LogicalMap
    {
        public int[] walls;
        public int[] floor;
        public int[] props;
        public byte[] collision;
        public int Stride;
        public int Width;
        public int Height;
        public int Margin = 0;

        public void SetBounds(int w, int h, int stride)
        {
            Width = w;
            Height = h;
            Stride = stride;
        }

        string GetCollisionString(int[] tiles, int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return string.Empty;

            int tileIdx = y * Width + x;
            int tileId = tiles[tileIdx];

            TileMetadata tileProperties;
            if (!Global.MapResources.TilemapMetaData.tileLookup.TryGetValue(tileId, out tileProperties))
                return string.Empty;

            return tileProperties.CollisionStr;
        }

        public void UpdateCollisionMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int tileIdx = y * Width + x;
                    int tileId = walls[tileIdx] != TileMetadata.NoTile ? walls[tileIdx] : floor[tileIdx];
                    // Each rendered tile is split into 4 collision blocks for more fine-grained collision
                    // 01
                    // 23
                    int collisionIdx0 = y * Width * 4 + x * 2;
                    int collisionIdx1 = collisionIdx0 + Width * 0 + 1;
                    int collisionIdx2 = collisionIdx0 + Width * 2 + 0;
                    int collisionIdx3 = collisionIdx0 + Width * 2 + 1;

                    if (tileId == TileMetadata.NoTile)
                    {
                        collision[collisionIdx0] = MapConstants.CollOutsideMap;
                        collision[collisionIdx1] = MapConstants.CollOutsideMap;
                        collision[collisionIdx2] = MapConstants.CollOutsideMap;
                        collision[collisionIdx3] = MapConstants.CollOutsideMap;
                        continue;
                    }

                    collision[collisionIdx0] = MapConstants.CollWalkable;
                    collision[collisionIdx1] = MapConstants.CollWalkable;
                    collision[collisionIdx2] = MapConstants.CollWalkable;
                    collision[collisionIdx3] = MapConstants.CollWalkable;

                    TileMetadata tileProperties;
                    if (!Global.MapResources.TilemapMetaData.tileLookup.TryGetValue(tileId, out tileProperties))
                        continue;

                    // A collision string is 4 chars like "0101", "0000", "1111" etc.
                    var collisionStr = GetCollisionString(walls, x, y);

                    // The collision map pieces will not fit together perfectly (but almost).

                    // close hole at corner
                    // if 0011 and up = 1010 | 0011 set self 1011
                    // if 0011 and up = 0101 | 0011 set self 0111

                    // remove artifact at corner
                    // if 1010 and left  = 0011 set self  0010
                    // if 0101 and right = 0011 set self  0001
                    var up = GetCollisionString(walls, x, y - 1);
                    var left = GetCollisionString(walls, x - 1, y);
                    var right = GetCollisionString(walls, x + 1, y);

                    if (collisionStr == "0011" && (up == "1010" || up == "0011"))
                        collisionStr = "1111";
                    else if (collisionStr == "0011" && (up == "0101" || up == "0011"))
                        collisionStr = "1111";
                    else if (collisionStr == "1010" && left == "0011")
                        collisionStr = "0010";
                    else if (collisionStr == "0101" && right == "0011")
                        collisionStr = "0001";

                    bool isValidCollisionStr = !string.IsNullOrEmpty(collisionStr) && collisionStr.Length == 4;
                    if (isValidCollisionStr)
                    {
                        collision[collisionIdx0] = (collisionStr[0] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                        collision[collisionIdx1] = (collisionStr[1] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                        collision[collisionIdx2] = (collisionStr[2] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                        collision[collisionIdx3] = (collisionStr[3] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                    }
                }
            }

            if (Global.WriteDebugPngFiles)
                Global.WriteDebugPng("collision", Array.ConvertAll(collision, item => (int)item), Width * 2, Height * 2, 0);
        }

        public void EnsureAllocatedSizeFromBounds()
        {
            int size = Width * Height;
            if (walls == null || walls.Length < size)
            {
                walls = new int[size];
                floor = new int[size];
                props = new int[size];
                collision = new byte[size * 4];
            }
        }
    }
}
