using UnityEngine;
using System.Collections.Generic;

namespace HordeEngine
{
    [ExecuteInEditMode]
    public class HordeSpriteManager : MonoBehaviour, IComponentUpdate
    {
        public Vector3 Offset = Vector3.zero;

        public List<HordeBatchRenderer> Batches = new List<HordeBatchRenderer>();
        List<long> keys_ = new List<long>();

        public void AddQuad(Vector3 center, Vector2 size, float rotationDegrees, float zSkew, Color color, Sprite sprite, Material material, int layer)
        {
            var batch = GetBatchRenderer(sprite, material, layer);
            batch.AddQuad(center, size, rotationDegrees, zSkew, color, sprite);
        }

        public HordeBatchRenderer GetBatchRenderer(Sprite sprite, Material material, int layer)
        {
            long key = sprite.texture.GetInstanceID() << 29 + material.GetInstanceID() << 6 + layer;
            int idx = keys_.IndexOf(key);
            if (idx < 0)
                idx = CreateBatchRenderer(key, sprite.texture, material, layer);

            return Batches[idx];
        }

        private int CreateBatchRenderer(long key, Texture sprite, Material material, int layer)
        {
            HordeBatchRenderer batch = new HordeBatchRenderer(key, sprite, material, layer);
            Batches.Add(batch);
            keys_.Add(key);
            return Batches.Count - 1;
        }
        
        void OnEnable()
        {
            Horde.Sprites = this;
            Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Internal_DrawMeshes);
        }

        void OnDisable()
        {
            Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Internal_DrawMeshes);
        }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(Offset, Quaternion.identity, Vector3.one);

            for (int i = 0; i < Batches.Count; ++i)
            {
                var batch = Batches[i];
                batch.ApplyChanges();
                GameManager.ShowDebug("batch " + i, batch.QuadCount);
                int activeMeshes = batch.GetActiveMeshCount();
                for (int j = 0; j < activeMeshes; ++j)
                {
                    Graphics.DrawMesh(batch.Meshes[j].Mesh, matrix, batch.Material, batch.Layer);
                }

                batch.Clear();
            }
        }
    }
}
