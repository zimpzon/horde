using System;

namespace HordeEngine
{
    public class LogicalMap
    {
        public int[] Walls;
        public int[] Floor;
        public int[] Props;
        public int Width;
        public int Height;
        public int Stride;
        public int Margin = 0;
        public byte[] CollisionMap;
        public int CollisionWidth;
        public int CollisionHeight;

        public void SetBounds(int w, int h, int stride)
        {
            Width = w;
            Height = h;
            Stride = stride;
            CollisionWidth = Width * 2;
            CollisionHeight = Height * 2;
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
                    int tileId = Walls[tileIdx] != TileMetadata.NoTile ? Walls[tileIdx] : Floor[tileIdx];
                    // Each rendered tile is split into 4 collision blocks for more fine-grained collision
                    // 01
                    // 23
                    int collisionIdx0 = y * Width * 4 + x * 2;
                    int collisionIdx1 = collisionIdx0 + Width * 0 + 1;
                    int collisionIdx2 = collisionIdx0 + Width * 2 + 0;
                    int collisionIdx3 = collisionIdx0 + Width * 2 + 1;

                    if (tileId == TileMetadata.NoTile)
                    {
                        CollisionMap[collisionIdx0] = MapConstants.CollOutsideMap;
                        CollisionMap[collisionIdx1] = MapConstants.CollOutsideMap;
                        CollisionMap[collisionIdx2] = MapConstants.CollOutsideMap;
                        CollisionMap[collisionIdx3] = MapConstants.CollOutsideMap;
                        continue;
                    }

                    CollisionMap[collisionIdx0] = MapConstants.CollWalkable;
                    CollisionMap[collisionIdx1] = MapConstants.CollWalkable;
                    CollisionMap[collisionIdx2] = MapConstants.CollWalkable;
                    CollisionMap[collisionIdx3] = MapConstants.CollWalkable;

                    TileMetadata tileProperties;
                    if (!Global.MapResources.TilemapMetaData.tileLookup.TryGetValue(tileId, out tileProperties))
                        continue;

                    // A collision string is 4 chars like "0101", "0000", "1111" etc.
                    var collisionStr = GetCollisionString(Walls, x, y);

                    // The collision map pieces will not fit together perfectly (but almost).

                    // Close hole at corner
                    // if 0011 and up = 1010 | 0011 set self 1011
                    // if 0011 and up = 0101 | 0011 set self 0111

                    // Remove artifact at corner
                    // if 1010 and left  = 0011 set self  0010
                    // if 0101 and right = 0011 set self  0001
                    //var up = GetCollisionString(Walls, x, y - 1);
                    //var left = GetCollisionString(Walls, x - 1, y);
                    //var right = GetCollisionString(Walls, x + 1, y);

                    //if (collisionStr == "0011" && (up == "1010" || up == "0011"))
                    //    collisionStr = "1111";
                    //else if (collisionStr == "0011" && (up == "0101" || up == "0011"))
                    //    collisionStr = "1111";
                    //else if (collisionStr == "1010" && left == "0011")
                    //    collisionStr = "0010";
                    //else if (collisionStr == "0101" && right == "0011")
                    //    collisionStr = "0001";

                    bool isValidCollisionStr = !string.IsNullOrEmpty(collisionStr) && collisionStr.Length == 4;
                    if (isValidCollisionStr)
                    {
                        CollisionMap[collisionIdx0] = (collisionStr[0] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                        CollisionMap[collisionIdx1] = (collisionStr[1] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                        CollisionMap[collisionIdx2] = (collisionStr[2] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                        CollisionMap[collisionIdx3] = (collisionStr[3] == '0') ? MapConstants.CollWalkable : MapConstants.CollBlocked;
                    }
                }
            }

            if (Global.WriteDebugPngFiles)
                Global.WriteDebugPng("collision", Array.ConvertAll(CollisionMap, item => (int)item), Width * 2, Height * 2, 0);
        }

        public void EnsureAllocatedSizeFromBounds()
        {
            int size = Width * Height;
            if (Walls == null || Walls.Length < size)
            {
                Walls = new int[size];
                Floor = new int[size];
                Props = new int[size];
                CollisionMap = new byte[size * 4];
            }
        }
    }
}
