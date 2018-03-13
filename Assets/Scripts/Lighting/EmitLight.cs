using UnityEngine;

namespace HordeEngine
{
    public class EmitLight : MonoBehaviour, IComponentUpdate
    {
        public Color Color = Color.yellow;
        public bool Enabled;
        public Vector2 Scale = Vector2.one;
        public Vector3 Offset = Vector3.zero;

        Transform trans_;
        DynamicQuadRenderer renderer_;

        private void Awake()
        {
            trans_ = transform;
            renderer_ = Global.SceneAccess.PointLightRenderer;
        }

        void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.RenderLights); }
        void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.RenderLights); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            renderer_.Mesh.AddQuad(trans_.position + Offset, Scale.x, Scale.y, 0.0f, 0.0f, Color);
        }
    }
}
