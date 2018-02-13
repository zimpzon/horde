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

        void UpdateCollisionMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int tileIdx = y * Width + x;
                    int tileId = walls[tileIdx];
                    // Each rendered tile is split into 4 collision blocks for more fine grained collision
                    // 01
                    // 23
                    int collisionIdx0 = tileIdx * 4;
                    int collisionIdx1 = collisionIdx0 + Width * 0 + 4;
                    int collisionIdx2 = collisionIdx0 + Width * 4 + 0;
                    int collisionIdx3 = collisionIdx0 + Width * 4 + 4;

                    var tileProperties = Global.MapResources.TilemapMetaData.tileLookup[tileId];
                    var collisionStr = tileProperties.CollisionStr;

                    // A collision string is 4 chars like "0101", "0000", "1111" etc.
                    bool isValidCollisionStr = !string.IsNullOrEmpty(collisionStr) && collisionStr.Length == 4;
                    if (tileId == 0 || !isValidCollisionStr)
                    {
                        collision[collisionIdx0] = 0;
                        collision[collisionIdx1] = 0;
                        collision[collisionIdx2] = 0;
                        collision[collisionIdx3] = 0;
                    }
                    else
                    {
                        collision[collisionIdx0] = (byte)((collisionStr[0] == '0') ? 0 : 255);
                        collision[collisionIdx1] = (byte)((collisionStr[1] == '0') ? 0 : 255);
                        collision[collisionIdx2] = (byte)((collisionStr[2] == '0') ? 0 : 255);
                        collision[collisionIdx3] = (byte)((collisionStr[3] == '0') ? 0 : 255);
                    }
                }
            }
            MapUtil.TilesToPng(@"c:\private\collision.png", )
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
