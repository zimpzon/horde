using System.Collections.Generic;

namespace HordeEngine
{
    /// <summary>
    /// VirtualMap is an infinite (within int.Min and int.Max) grid of chunks.
    /// </summary>
    public class VirtualMap
    {
#pragma warning disable CS0649
        class ChunkData
        {
            public LayerChunk layerFloor;
            public LayerChunk layerWalls;
            public LayerChunk layerProps;
            public LightmapChunk lightmapChunk;

            public void Initialize(int w, int h, float tileW, float tileH, int lightmapResolution)
            {
                layerFloor = new LayerChunk(w, h, tileW, tileH);
                layerWalls = new LayerChunk(w, h, tileW, tileH);
                layerProps = new LayerChunk(w, h, tileW, tileH);
                lightmapChunk = new LightmapChunk(w, h, tileW, tileH, lightmapResolution);

            }
        }
#pragma warning restore CS0649

        int chunkW_;
        int chunkH_;
        float tileW_;
        float tileH_;
        int lightmapResulution_;
        ReusableObject<ChunkData> chunkCache_;

        Dictionary<long, ChunkData> chunks_ = new Dictionary<long, ChunkData>();

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
            foreach (var chunk in chunks_.Values)
                chunkCache_.ReturnObject(chunk);

            chunks_.Clear();
        }

        public void SetMap(MapData mapData)
        {
            ClearChunks();

            int chunksX = (mapData.Width / chunkW_) + 1;
            int chunksY = (mapData.Height / chunkH_) + 1;
            for (int cy = 0; cy < chunksY; ++cy)
            {
                for (int cx = 0; cx < chunksX; ++cx)
                {
                    var chunkData = chunkCache_.GetObject();
                    chunkData.layerFloor.Update(mapData, mapData.floor, chunksX * chunkW_, chunksY * chunkH_, Global.MapResources.TilemapMetaData);
                }
            }
        }
    }
}
