using System;
using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// Renders quads sharing same material and texture. These can be rendered in one draw call.
    /// </summary>
    public class HordeBatchRenderer
    {
        [NonSerialized]public Texture Texture;
        [NonSerialized]public Material Material;
        [NonSerialized]public long Id;
        [NonSerialized]public int Layer;
        [NonSerialized]public int BatchMeshQuadCapacity = 256;
        [NonSerialized]public List<HordeBatchMesh> Meshes = new List<HordeBatchMesh>();
        [NonSerialized] public int QuadCount;

        int totalQuadCapacity_;
        float textureXToUV_;
        float textureYToUV_;

        public HordeBatchRenderer(long id, Texture texture, Material material, int layer)
        {
            Layer = layer;

            Texture = texture;
            textureXToUV_ = 1.0f / Texture.width;
            textureYToUV_ = 1.0f / Texture.height;

            Material = new Material(material)
            {
                mainTexture = texture
            };
        }

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

        public void AddQuad(Vector3 center, Vector2 size, float rotationDegrees, float zSkew, Color color, Sprite sprite)
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

            var textureRect = sprite.textureRect;
            Vector2 uvTopLeft = new Vector2(textureRect.x * textureXToUV_, (textureRect.y + textureRect.height) * textureYToUV_);
            Vector2 uvSize = new Vector2(textureRect.width * textureXToUV_, textureRect.height * textureYToUV_);
            currentMesh.AddQuad(center, size, rotationDegrees, zSkew, uvTopLeft, uvSize, color);
        }
    }
}
