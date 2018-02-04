using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// Vertices are initialized once and never changed.
    /// UV and Triangles are also fixed size but values are updated from a tile
    /// map, skipping empty tiles.
    /// </summary>
    public class LayerChunk
    {
        public int Width;
        public int Height;
        public float TileW;
        public float TileH;
        public Vector3[] Vertices = new Vector3[0];
        public Vector2[] UV = new Vector2[0];
        public int[] Indices = new int[0];

        public LayerChunk(int w, int h, float tileW, float tileH)
        {
            Width = w;
            Height = h;
            TileW = tileW;
            TileH = tileH;

            Initialize();
        }

        public void Update(MapData mapData, int[] tiles, int tileX, int tileY, TileMapMetadata tileMeta)
        {
            //
            int w = Width;
            int h = Height;
            int stride = mapData.Stride;
            int tileNonEmptyCount = 0;

            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    int tileId = tiles[(y + tileY) * stride + (x + tileX)];
                    if (tileId != 0)
                    {
                        int indices0 = tileNonEmptyCount * 6;
                        int vertex0 = (y * w + x) * 4;
                        // 0---1
                        // | / |
                        // 3---2

                        // Left triangle
                        Indices[indices0 + 0] = vertex0 + 0;
                        Indices[indices0 + 1] = vertex0 + 1;
                        Indices[indices0 + 2] = vertex0 + 3;

                        // Right triangle
                        Indices[indices0 + 3] = vertex0 + 1;
                        Indices[indices0 + 4] = vertex0 + 2;
                        Indices[indices0 + 5] = vertex0 + 3;

                        Debug.DrawLine(Vertices[vertex0 + 0], Vertices[vertex0 + 1], Color.green);
                        Debug.DrawLine(Vertices[vertex0 + 1], Vertices[vertex0 + 3], Color.green);
                        Debug.DrawLine(Vertices[vertex0 + 3], Vertices[vertex0 + 0], Color.green);

                        Debug.DrawLine(Vertices[vertex0 + 1], Vertices[vertex0 + 2], Color.green);
                        Debug.DrawLine(Vertices[vertex0 + 2], Vertices[vertex0 + 3], Color.green);

                        tileNonEmptyCount++;
                    }
                }
            }
        }

        void Initialize()
        {
            int tileCount = Width * Height;
            int vertexCount = tileCount * 4;

            int maxTriangleCount = tileCount * 2;
            int maxIndexCount = maxTriangleCount * 3;

            Vertices = new Vector3[vertexCount];
            UV = new Vector2[vertexCount];
            Indices = new int[maxTriangleCount];

            int idx0 = 0;
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // Clockwise
                    // 0----1
                    // |    |
                    // 3----2
                    Vertices[idx0 + 0] = new Vector3((x + 0) * TileW, (y + 1) * TileH, 0.0f);
                    Vertices[idx0 + 1] = new Vector3((x + 1) * TileW, (y + 1) * TileH, 0.0f);
                    Vertices[idx0 + 2] = new Vector3((x + 1) * TileW, (y + 0) * TileH, 0.0f);
                    Vertices[idx0 + 3] = new Vector3((x + 0) * TileW, (y + 0) * TileH, 0.0f);

                    idx0 += 4;
                }
            }
        }
    }
}
