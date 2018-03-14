﻿using System;
using UnityEngine;

namespace HordeEngine
{
    // REWRITE THIS so we have flat floor tiles and then Wall tiles that have a height. Extend vertices and UV to cover full height.
    // ADD BOTTOM tiles first to lessen overdraw!!
    // When chained to bottom this should align perfectly with all tilted sprites. ONE slope for full tile height.
    /// <summary>
    /// Vertices are initialized once and never changed.
    /// UV and Triangles are also fixed size but values are updated from a tile
    /// map, skipping empty tiles. The remaining triangles are left as degenerate,
    /// ie. they will be skipped by the graphics hardware.
    /// </summary>
    public class DisplayMapChunk
    {
        const float TileTopZSkew = -1.0f;
        [NonSerialized] public static Vector3[] SharedVertices;
        [NonSerialized] public static Vector3[] SharedVerticesSkewed;
        [NonSerialized] public int ChunkWidth;
        [NonSerialized] public int ChunkHeight;
        [NonSerialized] public float TileW;
        [NonSerialized] public float TileH;

        [NonSerialized] public Vector2[] UV = new Vector2[0];
        [NonSerialized] public int[] Indices = new int[0];
        [NonSerialized] public Mesh Mesh = new Mesh();

        [NonSerialized] public int ActiveTiles;
        [NonSerialized] public int ActiveWidth;
        [NonSerialized] public int ActiveHeight;
        [NonSerialized] public int TopLeftX;
        [NonSerialized] public int TopLeftY;

        public DisplayMapChunk(int w, int h, float tileW, float tileH)
        {
            ChunkWidth = w;
            ChunkHeight = h;
            TileW = tileW;
            TileH = tileH;

            Initialize();
        }

        public void Update(LogicalMap mapData, int[] tiles, int topLeftX, int topLeftY, TileMapMetadata tileMapMeta, bool skewTileTop, string debugName)
        {
            TopLeftX = topLeftX;
            TopLeftY = topLeftY;
            int w = topLeftX + ChunkWidth > mapData.Width ? mapData.Width - topLeftX : ChunkWidth;
            int h = topLeftY + ChunkHeight > mapData.Height ? mapData.Height - topLeftY : ChunkHeight;
            ActiveWidth = w;
            ActiveHeight = h;
            int stride = mapData.Stride;
            int tileCount = 0;
            int tileNonEmptyCount = 0;

            // Nudge UV a tiny bit inwards to avoid seams ("bleeding" from adjacent tiles) between rendered tiles caused by rounding.
            Vector2 uvNudgeX = new Vector2(0.0001f, 0.0f);
            Vector2 uvNudgeY = new Vector2(0.0f, 0.0001f);

            int endY = topLeftY + h;
            int endX = topLeftX + w;
            for (int y = topLeftY; y < endY; ++y)
            {
                for (int x = topLeftX; x < endX; ++x)
                {
                    int tileId = tiles[y * stride + x];
                    int indices0 = tileCount * 6;

                    if (tileId != TileMetadata.NoTile)
                    {
                        int vertex0 = tileCount * 4;
                        // 0---1
                        // | / | = [0, 1, 3] and [1, 2, 3]
                        // 3---2

                        // Left triangle
                        Indices[indices0 + 0] = vertex0 + 0;
                        Indices[indices0 + 1] = vertex0 + 1;
                        Indices[indices0 + 2] = vertex0 + 3;

                        // Right triangle
                        Indices[indices0 + 3] = vertex0 + 1;
                        Indices[indices0 + 4] = vertex0 + 2;
                        Indices[indices0 + 5] = vertex0 + 3;

                        // UV coordinate system is 0, 0 at bottom left but CalcUV will take care of that.
                        UV[vertex0 + 0] = tileMapMeta.CalcUV(tileId, 0, 0) + uvNudgeX - uvNudgeY;
                        UV[vertex0 + 1] = tileMapMeta.CalcUV(tileId, 1, 0) - uvNudgeX - uvNudgeY;
                        UV[vertex0 + 2] = tileMapMeta.CalcUV(tileId, 1, 1) - uvNudgeX + uvNudgeY;
                        UV[vertex0 + 3] = tileMapMeta.CalcUV(tileId, 0, 1) + uvNudgeX + uvNudgeY;

                        if (Global.DebugDrawing)
                        {
                            Debug.DrawLine(SharedVertices[vertex0 + 0], SharedVertices[vertex0 + 1], Color.green);
                            Debug.DrawLine(SharedVertices[vertex0 + 1], SharedVertices[vertex0 + 3], Color.green);
                            Debug.DrawLine(SharedVertices[vertex0 + 3], SharedVertices[vertex0 + 0], Color.green);

                            Debug.DrawLine(SharedVertices[vertex0 + 1], SharedVertices[vertex0 + 2], Color.green);
                            Debug.DrawLine(SharedVertices[vertex0 + 2], SharedVertices[vertex0 + 3], Color.green);
                        }

                        tileNonEmptyCount++;
                    }
                    else
                    {
                        // Create degenerate triangles where there is no tile to render.
                        // These will be skipped very quickly by the graphics hardware.
                        Indices[indices0 + 0] = 0;
                        Indices[indices0 + 1] = 0;
                        Indices[indices0 + 2] = 0;

                        Indices[indices0 + 3] = 0;
                        Indices[indices0 + 4] = 0;
                        Indices[indices0 + 5] = 0;
                    }

                    tileCount++;
                }
            }

            ActiveTiles = tileNonEmptyCount;

            Mesh.vertices = skewTileTop ? SharedVerticesSkewed : SharedVertices;
            Mesh.triangles = Indices;
            Mesh.uv = UV;
        }

        void InitializeSharedVertices(Vector3[] vertices, bool skewTileTop)
        {
            // Vertices starts at 0, 0 and moves down and to the right in the coordinate system (so y goes towards negative)
            int idx0 = 0;

            for (int y = 0; y < ChunkHeight; ++y)
            {
                for (int x = 0; x < ChunkWidth; ++x)
                {
                    // Clockwise
                    // 0----1
                    // |    |
                    // 3----2
                    if (skewTileTop)
                    {
                        // Standing (ex wall)
                        vertices[idx0 + 0] = new Vector3((x + 0) * TileW, (-y + 0) * TileH, TileTopZSkew);
                        vertices[idx0 + 1] = new Vector3((x + 1) * TileW, (-y + 0) * TileH, TileTopZSkew);
                        vertices[idx0 + 2] = new Vector3((x + 1) * TileW, (-y - 1) * TileH, 0.0f);
                        vertices[idx0 + 3] = new Vector3((x + 0) * TileW, (-y - 1) * TileH, 0.0f);
                    }
                    else
                    {
                        // Flat (ex floor)
                        vertices[idx0 + 0] = new Vector3((x + 0) * TileW, (-y + 0) * TileH, 0.0f);
                        vertices[idx0 + 1] = new Vector3((x + 1) * TileW, (-y + 0) * TileH, 0.0f);
                        vertices[idx0 + 2] = new Vector3((x + 1) * TileW, (-y - 1) * TileH, 0.0f);
                        vertices[idx0 + 3] = new Vector3((x + 0) * TileW, (-y - 1) * TileH, 0.0f);
                    }

                    idx0 += 4;
                }
            }
        }

        void Initialize()
        {
            int tileCount = ChunkWidth * ChunkHeight;
            int vertexCount = tileCount * 4;

            int maxTriangleCount = tileCount * 2;
            int maxIndexCount = maxTriangleCount * 3;

            UV = new Vector2[vertexCount];
            Indices = new int[maxTriangleCount * 3];

            if (SharedVertices == null)
            {
                SharedVertices = new Vector3[vertexCount];
                InitializeSharedVertices(SharedVertices, skewTileTop: false);
            }

            if (SharedVerticesSkewed == null)
            {
                SharedVerticesSkewed = new Vector3[vertexCount];
                InitializeSharedVertices(SharedVerticesSkewed, skewTileTop: true);
            }
        }
    }
}
