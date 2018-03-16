using System;
using System.Collections.Generic;
using UnityEngine;

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
            batch.AddQuad(center, size, rotationDegrees, zSkew, color);
        }

        public HordeBatchRenderer GetBatchRenderer(Sprite sprite, Material material, int layer)
        {
            long key = sprite.GetInstanceID() * material.GetInstanceID() << 6 + layer;
            int idx = keys_.IndexOf(key);
            if (idx < 0)
                idx = CreateBatch(key, sprite.texture, material, layer);

            return Batches[idx];
        }

        private int CreateBatch(long key, Texture sprite, Material material, int layer)
        {
            HordeBatchRenderer batch = new HordeBatchRenderer(key, sprite, material, layer);
            Batches.Add(batch);
            keys_.Add(key);
            return Batches.Count - 1;
        }

        void OnEnable()
        {
            Horde.Sprites = this;
            Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late);
        }

        void OnDisable()
        {
            Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late);
        }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            GameManager.ShowDebug("render", Time.time);
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(Offset, Quaternion.identity, Vector3.one);

            for (int i = 0; i < Batches.Count; ++i)
            {
                var batch = Batches[i];
                batch.ApplyChanges();

                int activeMeshes = batch.GetActiveMeshCount();
                GameManager.ShowDebug("batch activemeshes " + i, activeMeshes);
                GameManager.ShowDebug("batch quads " + i, batch.QuadCount);
                for (int j = 0; j < activeMeshes; ++j)
                {
                    GameManager.ShowDebug("batch mesh quads " + (i + 1) * j, batch.Meshes[j].ActiveQuadCount);
                    Graphics.DrawMesh(batch.Meshes[j].Mesh, matrix, batch.Material, batch.Layer);
                }

                batch.Clear();
            }
        }
    }
}
