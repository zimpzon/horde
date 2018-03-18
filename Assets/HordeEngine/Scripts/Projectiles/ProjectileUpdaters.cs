using UnityEngine;

namespace HordeEngine
{
    public static class ProjectileUpdaters
    {
        public static ActorController_Player Player;
        public static Vector2 PlayerPos;
        public static float PlayerSize;

        static void CollidePlayer(Vector2 pos, float size, Vector2 dir)
        {
            float s2 = PlayerSize + size;
            if (Mathf.Abs(pos.x - PlayerPos.x) < s2 && Mathf.Abs(pos.y - PlayerPos.y) < s2)
            {
                Player.Hit(dir);
            }
        }

        public static bool BasicMove(ref Projectile p)
        {
            p.ActualPos += p.Velocity * Horde.Time.DeltaTime;
            if (p.CollidePlayer)
                CollidePlayer(p.ActualPos, p.CollisionSize, p.Velocity);

            return !CollisionUtil.IsCircleColliding(p.ActualPos, p.CollisionSize);
        }

        public static bool CirclingMove(ref Projectile p)
        {
            p.Origin += p.Velocity * Horde.Time.DeltaTime;
            var oldPos = p.ActualPos;

            float deg = Time.time * 100 + p.Idx * 5;
            float sin = Mathf.Sin(-deg * Mathf.Deg2Rad);
            float cos = Mathf.Cos(-deg * Mathf.Deg2Rad);

            p.ActualPos.x = p.Origin.x + cos - sin;
            p.ActualPos.y = p.Origin.y + sin + cos; 

            var move = p.ActualPos - oldPos;
            p.RotationDegrees = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
            if (p.CollidePlayer)
                CollidePlayer(p.ActualPos, p.CollisionSize, p.Velocity);

            return !CollisionUtil.IsCircleColliding(p.ActualPos, p.CollisionSize);
        }

        public static bool UpdateProjectile2(ref Projectile p)
        {
            p.ActualPos += p.Velocity * Horde.Time.DeltaTime;
            float dist = (p.ActualPos - p.StartPos).sqrMagnitude;
            if (dist > p.MaxDist * p.MaxDist)
                return false;

            p.Velocity += p.Velocity * 1.5f * Time.deltaTime;
            return !CollisionUtil.IsCircleColliding(p.ActualPos, p.CollisionSize);
        }
    }
}
