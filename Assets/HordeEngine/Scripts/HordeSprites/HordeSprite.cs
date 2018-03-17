using UnityEngine;

namespace HordeEngine
{
    [ExecuteInEditMode]
    public class HordeSprite : MonoBehaviour, IComponentUpdate
    {
        public Sprite Sprite;
        public Material Material;
        public bool FlipX;
        [SerializeField, Layer] public LayerMask Layer;
        public Color Color = Color.white;
        public Vector2 Scale = Vector2.one;
        public Vector3 Offset = Vector3.zero;

        int layer_;

        Transform trans_;

        void UpdateLayer()
        {
            layer_ = Layer;
        }

        void OnValidate()
        {
            UpdateLayer();
        }
        
        void OnEnable()
        {
            trans_ = transform;
            UpdateLayer();
            Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default);
        }

        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            var scale = Scale;
            scale.x *= FlipX ? -1 : 1;
            Horde.Sprites.AddQuad(trans_.position + Offset, scale, 0.0f, Scale.y, Color, Sprite, Material, layer_);
        }
    }
}
