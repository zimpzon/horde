using UnityEngine;

namespace HordeEngine
{
    [ExecuteInEditMode]
    public class ActorSprite : MonoBehaviour, IComponentUpdate
    {
        public Color Color = Color.white;
        public bool Enabled;
        public Vector2 Scale = Vector2.one;
        public Vector3 Offset = Vector3.zero;
        public ComponentUpdatePass UpdatePass = ComponentUpdatePass.Default;
        public DynamicQuadRenderer QuadRenderer;

        Transform trans_;

        void OnEnable()
        {
            Global.ComponentUpdater.RegisterForUpdate(this, UpdatePass);
        }

        void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, UpdatePass); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            if (trans_ == null)
                trans_ = transform;

            QuadRenderer.QuadMesh.AddQuad(trans_.position + Offset, Scale, 0.0f, Scale.y, Color);
        }
    }
}
