using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// LayerChunk
    /// </summary>
    public class LayerChunk
    {
        Vector2Int chunkSize_;
        // vertices (maybe 0 based), uv in atlas,  Vertex color?

        public LayerChunk(Vector2Int chunkSize)
        {
            chunkSize_ = chunkSize;
        }
    }
}
