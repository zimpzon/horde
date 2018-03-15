using UnityEngine;

namespace HordeEngine
{
    public struct Projectile
    {
        // Static
        public float StartTime;
        public Vector2 StartPos;
        public Vector2 OriginBaseOffset;
        public float MaxDist;
        public float MaxTime;

        // Render only
        public int SpriteIdInTexture;
        public float RotationOffset;
        public bool RenderLight;
        public Vector2 Size;
        public Color Color;
        public Vector2 LightSize;
        public Color LightColor;

        // Collision only
        public bool CollideWalls;
        public float CollisionSize;

        public int BouncesLeft;
        public Vector2 Velocity;

        public Vector2 Origin;
        public Vector2 ActualPos;

        public delegate bool TickDelegate(ref Projectile projectile, int idx);
        public TickDelegate TickCallback;
    }
}
