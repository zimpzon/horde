using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// VirtualMap is an infinite (within int.Min and int.Max) grid of chunks.
    /// </summary>
    public class DisplayMap
    {
#pragma warning disable CS0649
        class ChunkData
        {
            public static long CalcId(int x, int y) { return y * 10000 + x; }
            public long Id;
            public int Cx;
            public int Cy;
            public DisplayMapChunk layerFloor;
            public DisplayMapChunk layerWalls;
            public DisplayMapChunk layerProps;
            public LightmapChunk lightmapChunk;

            public void Initialize(int w, int h, float tileW, float tileH, int lightmapResolution)
            {
                layerFloor = new DisplayMapChunk(w, h, tileW, tileH);
                layerWalls = new DisplayMapChunk(w, h, tileW, tileH);
                layerProps = new DisplayMapChunk(w, h, tileW, tileH);
                lightmapChunk = new LightmapChunk(w, h, tileW, tileH, lightmapResolution);
            }
        }
#pragma warning restore CS0649

        int chunkW_;
        int chunkH_;
        float tileW_;
        float tileH_;
        int lightmapResulution_;
        LogicalMap logicalMap_;
        ReusableObject<ChunkData> chunkCache_;

        Dictionary<long, ChunkData> chunks_ = new Dictionary<long, ChunkData>();

        public DisplayMap(int chunkW, int chunkH, float tileW, float tileH, int lightmapResolution)
        {
            chunkW_ = chunkW;
            chunkH_ = chunkH;
            tileW_ = tileW;
            tileH_ = tileH;
            lightmapResulution_ = lightmapResolution;

            chunkCache_ = new ReusableObject<ChunkData>(initializeMethod: InitializeChunk);
        }

        void InitializeChunk(ChunkData chunk)
        {
            chunk.Initialize(chunkW_, chunkH_, tileW_, tileH_, lightmapResulution_);
        }

        void ClearChunks()
        {
            foreach (var chunk in chunks_.Values)
                chunkCache_.ReturnObject(chunk);

            chunks_.Clear();
        }

        public void DrawMap(Material material, float floorZ)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            foreach (var chunk in chunks_.Values)
            {
                matrix.SetTRS(new Vector3(chunk.Cx * chunkW_, -chunk.Cy * chunkH_, floorZ), Quaternion.identity, Vector3.one);

                Graphics.DrawMesh(chunk.layerFloor.Mesh, matrix, material, 0);
                Graphics.DrawMesh(chunk.layerWalls.Mesh, matrix, material, 0);
                Graphics.DrawMesh(chunk.layerProps.Mesh, matrix, material, 0);
            }
        }

        public void SetMap(LogicalMap logicalMap)
        {
            logicalMap_ = logicalMap;
            ClearChunks();

            if (Global.WriteDebugPngFiles)
            {
                Global.WriteDebugPng("logical_floor", logicalMap.floor, logicalMap.Width, logicalMap.Height, TileMetadata.NoTile);
                Global.WriteDebugPng("logical_walls", logicalMap.walls, logicalMap.Width, logicalMap.Height, TileMetadata.NoTile);
                Global.WriteDebugPng("logical_props", logicalMap.props, logicalMap.Width, logicalMap.Height, TileMetadata.NoTile);
            }

            if (Global.DebugLogging)
                Debug.LogFormat("LogicalMap size: {0}, {1}", logicalMap.Width, logicalMap.Height);

            int chunksX = (logicalMap.Width + (chunkW_ - 1)) / chunkW_;
            int chunksY = (logicalMap.Height + (chunkH_ - 1)) / chunkH_;
            for (int cy = 0; cy < chunksY; ++cy)
            {
                for (int cx = 0; cx < chunksX; ++cx)
                {
                    var chunk = chunkCache_.GetObject();
                    chunk.Id = ChunkData.CalcId(cx, cy);
                    chunk.Cx = cx;
                    chunk.Cy = cy;

                    chunk.layerFloor.Update(logicalMap, logicalMap.floor, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData, skewTileTop: false, debugName: "floor");
                    chunk.layerWalls.Update(logicalMap, logicalMap.walls, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData, skewTileTop: true, debugName: "walls");
                    chunk.layerProps.Update(logicalMap, logicalMap.props, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData, skewTileTop: true, debugName: "props");

                    bool isEmpty = chunk.layerFloor.ActiveTiles + chunk.layerWalls.ActiveTiles + chunk.layerProps.ActiveTiles == 0;
                    if (!isEmpty)
                    {
                        chunks_[chunk.Id] = chunk;

                        if (Global.DebugLogging)
                        {
                            Debug.LogFormat("Floor tile {0} ({1}, {2}): active = {3} ({4}, {5})", chunk.Id, cx, cy, chunk.layerFloor.ActiveTiles, chunk.layerFloor.ActiveWidth, chunk.layerFloor.ActiveHeight);
                            Debug.LogFormat("Walls tile {0} ({1}, {2}): active = {3} ({4}, {5})", chunk.Id, cx, cy, chunk.layerWalls.ActiveTiles, chunk.layerWalls.ActiveWidth, chunk.layerWalls.ActiveHeight);
                            Debug.LogFormat("Props tile {0} ({1}, {2}): active = {3} ({4}, {5})", chunk.Id, cx, cy, chunk.layerProps.ActiveTiles, chunk.layerProps.ActiveWidth, chunk.layerProps.ActiveHeight);
                        }
                    }
                }
            }
        }
    }
}
