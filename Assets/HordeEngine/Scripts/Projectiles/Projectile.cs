using UnityEngine;

namespace HordeEngine
{
    public enum ProjectileLight { None, Soft, Hard };

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
            EmitLight = ProjectileLight.Hard;
            LightOffsetY = 0.0f;
            Size = Vector2.one;
            Color = Color.white;
            LightSize = Vector2.one;
            LightColor = Color.white;
            BounceWalls = false;
            CollisionSize = 1.0f;
            BouncesLeft = 0;
            Velocity = Vector2.zero;
            ActualPos = Vector2.zero;
            UpdateCallback = null;
        }

        public void ApplyDescription(ProjectileDescription desc)
        {
            Size = desc.Size;
            Color = desc.Color;

            EmitLight = desc.EmitLight;
            LightSize = desc.LightSize;
            LightColor = desc.LightColor;
            LightOffsetY = desc.LightOffsetY;

            CollisionSize = desc.CollisionSize;
            BounceWalls = desc.BounceWalls;
            BouncesLeft = desc.MaxWallBounces;

            MaxDist = desc.MaxDistance;
            MaxTime = desc.MaxTime;
        }

        public float StartTime;
        public Vector2 StartPos;
        public Vector2 Origin;
        public Vector2 OriginOffset;
        public float MaxDist;
        public float MaxTime;

        public Vector2 Size;
        public Color Color;
        public ProjectileLight EmitLight;
        public Vector2 LightSize;
        public Color LightColor;
        public float LightOffsetY;

        public float CollisionSize;
        public bool BounceWalls;
        public int BouncesLeft;

        public Vector2 Velocity;
        public Vector2 ActualPos;

        public delegate bool TickDelegate(ref Projectile projectile, int idx);
        public TickDelegate UpdateCallback;
    }
}
