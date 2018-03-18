using UnityEngine;

namespace HordeEngine
{
    public struct Projectile
    {
        public void Reset()
        {
            Idx = 0;
            StartPos = Vector2.zero;
            StartTime = 0.0f;
            Origin = Vector2.zero;
            OriginOffset = Vector2.zero;
            MaxDist = float.MaxValue;
            MaxTime = float.MaxValue;
            EmitLight = false;
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
            Sprite = desc.Sprite;
            Material = desc.Material;
            Size = desc.Size;
            Color = desc.Color;

            EmitLight = desc.EmitLight;
            LightSprite = desc.LightSprite;
            LightMaterial = desc.LightMaterial;
            LightSize = desc.LightSize;
            LightColor = desc.LightColor;
            LightOffsetY = desc.LightOffsetY;

            CollisionSize = desc.CollisionSize;
            BounceWalls = desc.BounceWalls;
            BouncesLeft = desc.MaxWallBounces;

            MaxDist = desc.MaxDistance;
            MaxTime = desc.MaxTime;
        }

        public int Idx;
        public float StartTime;
        public Vector2 StartPos;
        public Vector2 Origin;
        public Vector2 OriginOffset;
        public float MaxDist;
        public float MaxTime;

        public Sprite Sprite;
        public Material Material;
        public Vector2 Size;
        public Color Color;
        public bool EmitLight;
        public Sprite LightSprite;
        public Material LightMaterial;
        public Vector2 LightSize;
        public Color LightColor;
        public float LightOffsetY;

        public bool CollidePlayer;
        public float CollisionSize;
        public bool BounceWalls;
        public int BouncesLeft;

        public Vector2 Velocity;
        public Vector2 ActualPos;
        public float RotationDegrees;
        public float Z;

        public delegate bool TickDelegate(ref Projectile projectile);
        public TickDelegate UpdateCallback;
    }
}
