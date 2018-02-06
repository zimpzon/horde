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
            public static long CalcId(int x, int y) { return y << 32 + x; }
            public long Id;
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

        public void DrawMap(Material material)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            foreach (var chunk in chunks_.Values)
            {
                Graphics.DrawMesh(chunk.layerFloor.Mesh, matrix, material, 0);
                Graphics.DrawMesh(chunk.layerWalls.Mesh, matrix, material, 0);
                //chunk.layerFloor.Update(logicalMap_, logicalMap_.floor, 0, 0, Global.MapResources.TilemapMetaData);
                chunk.layerFloor.Update(logicalMap_, logicalMap_.walls, 0, 0, Global.MapResources.TilemapMetaData);
            }
        }

        public void SetMap(LogicalMap logicalMap)
        {
            logicalMap_ = logicalMap;
            ClearChunks();

            int chunksX = (logicalMap.Width / chunkW_) + 1;
            int chunksY = (logicalMap.Height / chunkH_) + 1;
            for (int cy = 0; cy < chunksY; ++cy)
            {
                for (int cx = 0; cx < chunksX; ++cx)
                {
                    var chunk = chunkCache_.GetObject();
                    chunk.Id = ChunkData.CalcId(cx, cy);
                    chunks_[chunk.Id] = chunk;
                    chunk.layerFloor.Update(logicalMap, logicalMap.floor, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData);
                    chunk.layerWalls.Update(logicalMap, logicalMap.walls, cx * chunkW_, cy * chunkH_, Global.MapResources.TilemapMetaData);
                }
            }
        }
    }
}
