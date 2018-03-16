using System;
using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// Renders quads sharing same material and texture. These can be rendered in one draw call.
    /// </summary>
    [Serializable]
    public class HordeBatchRenderer
    {
        public Texture Texture;
        public Material Material;
        public long Id;
        public int Layer;
        public int BatchMeshQuadCapacity = 16;
        public List<HordeBatchMesh> Meshes = new List<HordeBatchMesh>();
        public int QuadCount;

        int totalQuadCapacity_;

        public int GetActiveMeshCount()
        {
            return (QuadCount + BatchMeshQuadCapacity - 1) / BatchMeshQuadCapacity;
        }

        public void Clear()
        {
            QuadCount = 0;
            for (int i = 0; i < Meshes.Count; ++i)
                Meshes[i].Clear();
        }

        public void ApplyChanges()
        {
            for (int i = 0; i < Meshes.Count; ++i)
                Meshes[i].ApplyChanges();
        }

        public HordeBatchRenderer(long id, Texture texture, Material material, int layer)
        {
            Layer = layer;
            Texture = texture;
            Material = new Material(material)
            {
                mainTexture = texture
            };
        }

        public void AddQuad(Vector3 center, Vector2 size, float rotationDegrees, float zSkew, Color color)
        {
            QuadCount++;
            if (QuadCount > totalQuadCapacity_)
            {
                // Optimization TODO: Could get this from a cache and return them on clear.
                var newMesh = new HordeBatchMesh(BatchMeshQuadCapacity);
                Meshes.Add(newMesh);
                totalQuadCapacity_ += BatchMeshQuadCapacity;
            }

            int meshIdx = (QuadCount - 1) / BatchMeshQuadCapacity;
            var currentMesh = Meshes[meshIdx];
            currentMesh.AddQuad(center, size, rotationDegrees, zSkew, color);
        }
    }
}
