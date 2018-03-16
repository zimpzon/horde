using UnityEngine;

namespace HordeEngine
{
    [CreateAssetMenu(fileName = "ProjectileDescription.asset", menuName = "HordeEngine/Projectile Description")]
    public class ProjectileDescription : ScriptableObject
    {
        public Sprite Sprite;
        public Material Material;
        public Vector2 Size = Vector2.one;
        public Color Color = Color.white;

        public ProjectileLight EmitLight;
        public Vector2 LightSize = Vector2.one;
        public Color LightColor = Color.white;
        public float LightOffsetY;

        public float CollisionSize = 1.0f;
        public bool BounceWalls;
        public int MaxWallBounces = 2;

        public float MaxDistance = 20;
        public float MaxTime = 10;
    }
}
