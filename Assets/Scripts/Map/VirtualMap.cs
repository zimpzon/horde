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
        struct ChunkData
        {
            public LayerChunk layerFloor;
            public LayerChunk layerObstacles;
            public LayerChunk layerHigh; // Layer rendered above floor height, like top of walls, statues etc.
            public LightmapChunk lightmapChunk; // Calculated lightmap for chunk
        }
#pragma warning restore CS0649

        Vector2Int chunkSize_;
        Vector2Int origin_;
        Dictionary<long, ChunkData> chunks_ = new Dictionary<long, ChunkData>();

        public VirtualMap(Vector2Int chunkSize, Vector2Int origin)
        {
            chunkSize_ = chunkSize;
            origin_ = origin;
        }
    }
}
