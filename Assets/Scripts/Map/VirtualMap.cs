using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// VirtualMap is an infinite (almost, within int.Min and int.Max) grid of chunks. Only chunks
    /// containing data are stored.
    /// </summary>
    public class VirtualMap
    {
#pragma warning disable CS0649
        class ChunkData
        {
            public LayerChunk layerFloor;
            private LayerChunk layerWalls;
            public LayerChunk layerProps;
            public LightmapChunk lightmapChunk;

            public LayerChunk LayerWalls
            {
                get
                {
                    return layerWalls;
                }

                set
                {
                    layerWalls = value;
                }
            }

            public void Initialize(int w, int h, float tileW, float tileH, int lightmapResolution)
            {
                layerFloor = new LayerChunk(w, h, tileW, tileH);
                layerWalls = new LayerChunk(w, h, tileW, tileH);
                layerProps = new LayerChunk(w, h, tileW, tileH);
                lightmapChunk = new LightmapChunk(w, h, tileW, tileH, lightmapResolution);
            }
        }
#pragma warning restore CS0649

        Vector2Int chunkSize_;
        Vector2Int origin_;
        int chunkW_;
        int chunkH_;
        float tileW_;
        float tileH_;
        int lightmapResulution_;
        ReusableObject<ChunkData> chunkCache_;

        Dictionary<long, ChunkData> chunkWalls_ = new Dictionary<long, ChunkData>();
        Dictionary<long, ChunkData> chunkFloor_ = new Dictionary<long, ChunkData>();
        Dictionary<long, ChunkData> chunkProps_ = new Dictionary<long, ChunkData>();

        public VirtualMap(int chunkW, int chunkH, float tileW, float tileH, int lightmapResolution)
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
            ClearChunkMap(chunkWalls_);
            ClearChunkMap(chunkFloor_);
            ClearChunkMap(chunkProps_);
        }

        void ClearChunkMap(Dictionary<long, ChunkData> map)
        {
            foreach (var chunk in map.Values)
                chunkCache_.ReturnObject(chunk);

            map.Clear();
        }

        public void SetMap(MapData mapData)
        {
            ClearChunks();

        }
    }
}
