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

        public void SetBounds(int w, int h)
        {
            // Round up to a whole number of chunks
            int chunkCountX = ((w - 1) / MapConstants.ChunkW) + 1;
            int chunkCountY = ((h - 1) / MapConstants.ChunkH) + 1;
            Width = chunkCountX * MapConstants.ChunkW;
            Height = chunkCountY * MapConstants.ChunkH;
            Stride = Width;
            CollisionWidth = Width;
            CollisionHeight = Height;
        }

        public void UpdateCollisionMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int tileIdx = y * Width + x;
                    int tileId = Walls[tileIdx];
                    int collisionIdx0 = y * Width + x;

                    if (tileId == TileMetadata.NoTile)
                    {
                        // When there is no wall use floor to determine if tile is inside or outside of map
                        bool outsideMap = Floor[tileIdx] == TileMetadata.NoTile;
                        CollisionMap[collisionIdx0] = outsideMap ? MapConstants.CollOutsideMap: MapConstants.CollWalkable;
                        continue;
                    }

                    TileMetadata tileProperties;
                    if (!Global.MapResources.TilemapMetaData.tileLookup.TryGetValue(tileId, out tileProperties))
                    {
                        CollisionMap[collisionIdx0] = MapConstants.CollWalkable;
                        continue;
                    }

                    CollisionMap[collisionIdx0] = tileProperties.Blocking ? MapConstants.CollBlocked : MapConstants.CollWalkable;
                }
            }

            if (Global.WriteDebugPngFiles)
                Global.WriteDebugPng("collision", Array.ConvertAll(CollisionMap, item => (int)item), Width, Height, 0);
        }

        public void Clear()
        {
            for (int i = 0; i < Walls.Length; ++i)
            {
                Walls[i] = TileMetadata.NoTile;
                Floor[i] = TileMetadata.NoTile;
                Props[i] = TileMetadata.NoTile;
            }
        }

        public void EnsureAllocatedSizeFromBounds()
        {
            int size = Width * Height;
            if (Walls == null || Walls.Length < size)
            {
                Walls = new int[size];
                Floor = new int[size];
                Props = new int[size];
                CollisionMap = new byte[size];
            }
        }
    }
}
