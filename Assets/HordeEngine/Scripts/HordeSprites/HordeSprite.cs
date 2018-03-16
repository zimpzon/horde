using UnityEngine;

namespace HordeEngine
{
    [ExecuteInEditMode]
    public class HordeSprite : MonoBehaviour, IComponentUpdate
    {
        [SerializeField, Layer] public LayerMask Layer;
        public Color Color = Color.white;
        public Vector2 Scale = Vector2.one;
        public Vector3 Offset = Vector3.zero;
        public ComponentUpdatePass UpdatePass = ComponentUpdatePass.Default;
        public DynamicQuadRenderer QuadRenderer;

        Transform trans_;

        void OnEnable()
        {
            trans_ = transform;
            Global.ComponentUpdater.RegisterForUpdate(this, UpdatePass);
        }

        void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, UpdatePass); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            if (QuadRenderer != null && QuadRenderer.QuadMesh != null)
                QuadRenderer.QuadMesh.AddQuad(trans_.position + Offset, Scale, 0.0f, Scale.y, Color); // , layer)
        }
    }
}
