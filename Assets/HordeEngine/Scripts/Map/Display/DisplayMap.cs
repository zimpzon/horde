﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
Very important rendering info
    Screen physical pixels must be a multiplier of rendered size. Right? Use black bars.

    Level geometry is offset in Y since we don't wan't to hide completely behind a wall
    and we don't need to be able to go 100% in front of one.

    Sprites are rotated around X go get the same skewing as the map tiles. This makes them
    shorter though, so the height has to be compensated.
    A 45 degrees rotation requires y scale of 1.4121 (sqr of 2, probably no coincidence?)
*/

namespace HordeEngine
{
    public class DisplayMap
    {
        class ChunkData
        {
            public static long CalcId(int x, int y) { return y * 10000 + x; }
            public long Id;
            public int Cx;
            public int Cy;
            public DisplayMapChunk layerFloor;
            public DisplayMapChunk layerWalls;
            public DisplayMapChunk layerProps;

            public void Initialize(int w, int h, float tileW, float tileH)
            {
                layerFloor = new DisplayMapChunk(w, h, tileW, tileH);
                layerWalls = new DisplayMapChunk(w, h, tileW, tileH);
                layerProps = new DisplayMapChunk(w, h, tileW, tileH);
            }
        }

        int chunkW_;
        int chunkH_;
        float tileW_;
        float tileH_;
        LogicalMap logicalMap_;
        ReusableObject<ChunkData> chunkCache_;

        List<ChunkData> chunks_ = new List<ChunkData>();

        public DisplayMap(int chunkW, int chunkH, float tileW, float tileH)
        {
            chunkW_ = chunkW;
            chunkH_ = chunkH;
            tileW_ = tileW;
            tileH_ = tileH;

            chunkCache_ = new ReusableObject<ChunkData>(initializeMethod: InitializeChunk);
        }

        void InitializeChunk(ChunkData chunk)
        {
            chunk.Initialize(chunkW_, chunkH_, tileW_, tileH_);
        }

        void ClearChunks()
        {
            for (int i = 0; i < chunks_.Count; ++i)
                chunkCache_.ReturnObject(chunks_[i]);

            chunks_.Clear();
        }

        void DrawDebugInfo()
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            for (int i = 0; i < chunks_.Count; ++i)
            {
                var chunk = chunks_[i];
                matrix.SetTRS(new Vector3(chunk.Cx * chunkW_, -chunk.Cy * chunkH_, 0.0f), Quaternion.identity, Vector3.one);
                DebugUtil.DrawRect(0.0f, 1.0f, 0.0f, 1.0f);

                float x0 = chunk.Cx * chunkW_;
                float x1 = x0 + chunkW_;
                float y0 = -chunk.Cy * chunkH_;
                float y1 = y0 - chunkH_;
                DebugUtil.DrawRect(x0, x1, y0, y1);
                DebugUtil.VectorText(new Vector3(x0 + 0.2f, y0 - 0.25f, 0.0f), string.Format("{0}-{1}", chunk.Cx, chunk.Cy), 0.1f);
            }
        }

        public void DrawMap(Material floorMat, Material wallMat, float floorZ, float offsetY)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            for (int i = 0; i < chunks_.Count; ++i)
            {
                var chunk = chunks_[i];
                matrix.SetTRS(new Vector3(0, 0, floorZ), Quaternion.identity, Vector3.one);
                Graphics.DrawMesh(chunk.layerWalls.Mesh, matrix, wallMat, 0);
                Graphics.DrawMesh(chunk.layerFloor.Mesh, matrix, floorMat, 0);
                //                Graphics.DrawMesh(chunk.layerProps.Mesh, matrix, wallMat, 0);
            }

            if (Global.DebugDrawing)
                DrawDebugInfo();
        }

        public void SetMap(LogicalMap logicalMap)
        {
            logicalMap_ = logicalMap;
            ClearChunks();

            if (Global.WriteDebugPngFiles)
            {
                Global.WriteDebugPng("logical_floor", logicalMap.Floor, logicalMap.Width, logicalMap.Height, TileMetadata.NoTile);
                Global.WriteDebugPng("logical_walls", logicalMap.Walls, logicalMap.Width, logicalMap.Height, TileMetadata.NoTile);
                Global.WriteDebugPng("logical_props", logicalMap.Props, logicalMap.Width, logicalMap.Height, TileMetadata.NoTile);
            }

            if (Global.DebugLogging)
                Debug.LogFormat("LogicalMap size: {0}, {1}", logicalMap.Width, logicalMap.Height);

            int chunksX = logicalMap.Width / chunkW_;
            int chunksY = logicalMap.Height / chunkH_;
            for (int cy = 0; cy < chunksY; ++cy)
            {
                for (int cx = 0; cx < chunksX; ++cx)
                {
                    var chunk = chunkCache_.GetObject();
                    chunk.Id = ChunkData.CalcId(cx, cy);
                    chunk.Cx = cx;
                    chunk.Cy = cy;

                    chunk.layerFloor.Update(logicalMap, logicalMap.Floor, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData, skewTileTop: false);
                    chunk.layerWalls.Update(logicalMap, logicalMap.Walls, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData, skewTileTop: true);
//                    chunk.layerProps.Update(logicalMap, logicalMap.Props, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData, skewTileTop: true, debugName: "props");

                    bool isEmpty = chunk.layerFloor.ActiveTiles + chunk.layerWalls.ActiveTiles + chunk.layerProps.ActiveTiles == 0;
                    if (!isEmpty)
                    {
                        chunks_.Add(chunk);

                        if (Global.DebugLogging)
                        {
                            Debug.LogFormat("Floor tile {0} ({1}, {2}): active = {3}", chunk.Id, cx, cy, chunk.layerFloor.ActiveTiles);
                            Debug.LogFormat("Walls tile {0} ({1}, {2}): active = {3}", chunk.Id, cx, cy, chunk.layerWalls.ActiveTiles);
                        }
                    }
                }
            }
        }
    }
}