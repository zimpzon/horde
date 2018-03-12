using UnityEngine;

namespace HordeEngine
{
    // NB! Every time one dies just move the last one to the now empty spot. No need for bitmap?
    struct Projectile
    {
        // Static
        public float StartTime;
        public Vector2 StartPos;
        public Vector2 OriginBaseOffset; // ex. rotating cross: time and offset is enough to rotate group around origin.
        public float MaxDist;
        public float MaxTime; // Needed for slow moving or completely still projectiles

        // Render only
        public int SpriteIdInTexture;
        public float RotationOffset;
        public bool RenderLight;
        public float LightSize;
        public Color Color;
        public Color LightColor;

        // Collision only
        public bool CollideWalls;
        public float Size;

        public int BouncesLeft;
        public Vector2 Velocity;

        public Vector2 Origin;
        public Vector2 ActualPos;

        public delegate void TickDelegate(ref Projectile projectile, int idx);
        public TickDelegate TickCallback;
    }
}
