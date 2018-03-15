using UnityEngine;

namespace HordeEngine
{
    public struct Projectile
    {
        public void Reset()
        {
            StartTime = 0.0f;
            StartPos = Vector2.zero;
            Origin = Vector2.zero;
            OriginOffset = Vector2.zero;
            MaxDist = float.MaxValue;
            MaxTime = float.MaxValue;
            RenderLight = false;
            LightOffsetY = 0.0f;
            Size = Vector2.one;
            Color = Color.white;
            LightSize = Vector2.one;
            LightColor = Color.white;
            CollideWalls = false;
            CollisionSize = 1.0f;
            BouncesLeft = 0;
            Velocity = Vector2.zero;
            ActualPos = Vector2.zero;
            TickCallback = null;
        }

        public float StartTime;
        public Vector2 StartPos;
        public Vector2 Origin;
        public Vector2 OriginOffset;
        public float MaxDist;
        public float MaxTime;

        public bool RenderLight;
        public float LightOffsetY;
        public Vector2 Size;
        public Color Color;
        public Vector2 LightSize;
        public Color LightColor;

        public bool CollideWalls;
        public float CollisionSize;
        public int BouncesLeft;

        public Vector2 Velocity;
        public Vector2 ActualPos;

        public delegate bool TickDelegate(ref Projectile projectile, int idx);
        public TickDelegate TickCallback;
    }
}
