using System;
using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    // REWRITE THIS so we have flat floor tiles and then Wall tiles that have a height. Extend vertices and UV to cover full height.
    // ADD BOTTOM tiles first to lessen overdraw!!
    // When chained to bottom this should align perfectly with all tilted sprites. ONE slope for full tile height.
    public class DisplayMapChunk
    {
        const float TileTopZSkew = -1.0f;
        [NonSerialized] public int ChunkWidth;
        [NonSerialized] public int ChunkHeight;
        [NonSerialized] public float TileW;
        [NonSerialized] public float TileH;

        [NonSerialized] public List<Vector3> Vertices;
        [NonSerialized] public List<Vector2> UV;
        [NonSerialized] public List<int> Indices;
        [NonSerialized] public Mesh Mesh = new Mesh();

        [NonSerialized] public int ActiveTiles;
        [NonSerialized] public int TopLeftX;
        [NonSerialized] public int TopLeftY;

        public DisplayMapChunk(int w, int h, float tileW, float tileH)
        {
            ChunkWidth = w;
            ChunkHeight = h;
            TileW = tileW;
            TileH = tileH;

            int tileCount = ChunkWidth * ChunkHeight;
            Vertices = new List<Vector3>(tileCount * 4);
            UV = new List<Vector2>(tileCount * 4);
            Indices = new List<int>(tileCount * 6);
        }

        public void Update(LogicalMap mapData, int[] tiles, int topLeftX, int topLeftY, TileMapMetadata tileMapMeta, bool skewTileTop)
        {
            Vertices.Clear();
            UV.Clear();
            Indices.Clear();

            TopLeftX = topLeftX;
            TopLeftY = topLeftY;
            int w = ChunkWidth;
            int h = ChunkHeight;
            int stride = mapData.Stride;
            int tileCount = 0;
            int tileNonEmptyCount = 0;

            // Nudge UV a tiny bit inwards to avoid seams ("bleeding" from adjacent tiles) between rendered tiles caused by rounding.
            Vector2 uvNudgeX = new Vector2(0.0001f, 0.0f);
            Vector2 uvNudgeY = new Vector2(0.0f, 0.0001f);

            int endY = topLeftY + h;
            int endX = topLeftX + w;

            // Adding triangles bottom-up will reduce overdraw since a tile above another is rendered behind it
            for (int y = endY - 1; y >= topLeftY; --y)
            {
                for (int x = topLeftX; x < endX; ++x)
                {
                    int tileId = tiles[y * stride + x];

                    if (tileId != TileMetadata.NoTile)
                    {
                        TileMetadata tileMeta;
                        if (!tileMapMeta.tileLookup.TryGetValue(tileId, out tileMeta))
                            tileMeta = TileMetadata.Default;

                        // 0---1
                        // | / | = [0, 1, 3] and [1, 2, 3]
                        // 3---2
                        float tileBaseY = (-y - 1) * TileH;
                        float tileTopY = tileBaseY + tileMeta.Height;
                        // Tiles are rotated -45 degrees around x-axis (z = -height)
                        float tileTopZ = skewTileTop ? -tileMeta.Height : 0.0f;
                        Vertices.Add(new Vector3((x + 0) * TileW, tileTopY, tileTopZ));
                        Vertices.Add(new Vector3((x + 1) * TileW, tileTopY, tileTopZ));
                        Vertices.Add(new Vector3((x + 1) * TileW, tileBaseY, 0.0f));
                        Vertices.Add(new Vector3((x + 0) * TileW, tileBaseY, 0.0f));

                        int vertex0 = Vertices.Count - 4;

                        // Left triangle
                        Indices.Add(vertex0 + 0);
                        Indices.Add(vertex0 + 1);
                        Indices.Add(vertex0 + 3);

                        // Right triangle
                        Indices.Add(vertex0 + 1);
                        Indices.Add(vertex0 + 2);
                        Indices.Add(vertex0 + 3);

                        // UV coordinate system is 0, 0 at bottom left
                        UV.Add(tileMapMeta.CalcUV(tileId, 0, 1, tileMeta.Height) + uvNudgeX - uvNudgeY);
                        UV.Add(tileMapMeta.CalcUV(tileId, 1, 1, tileMeta.Height) - uvNudgeX - uvNudgeY);
                        UV.Add(tileMapMeta.CalcUV(tileId, 1, 0, tileMeta.Height) - uvNudgeX + uvNudgeY);
                        UV.Add(tileMapMeta.CalcUV(tileId, 0, 0, tileMeta.Height) + uvNudgeX + uvNudgeY);

                        if (Global.DebugDrawing)
                        {
                            float duration = 10000;
                            Debug.DrawLine(Vertices[vertex0 + 0], Vertices[vertex0 + 1], Color.green, duration);
                            Debug.DrawLine(Vertices[vertex0 + 1], Vertices[vertex0 + 3], Color.green, duration);
                            Debug.DrawLine(Vertices[vertex0 + 3], Vertices[vertex0 + 0], Color.green, duration);
                            Debug.DrawLine(Vertices[vertex0 + 1], Vertices[vertex0 + 2], Color.green, duration);
                            Debug.DrawLine(Vertices[vertex0 + 2], Vertices[vertex0 + 3], Color.green, duration);
                        }

                        tileNonEmptyCount++;
                    }

                    tileCount++;
                }
            }

            ActiveTiles = tileNonEmptyCount;

            Mesh.Clear();
            Mesh.SetVertices(Vertices);
            Mesh.SetTriangles(Indices, 0);
            Mesh.SetUVs(0, UV);
        }
    }
}
