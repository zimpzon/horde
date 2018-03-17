using UnityEngine;

namespace HordeEngine
{
    public static class ProjectileUpdaters
    {
        public static bool BasicMove(ref Projectile projectile, int idx)
        {
            projectile.ActualPos += projectile.Velocity * Horde.Time.DeltaTime;
            return !CollisionUtil.IsCircleColliding(projectile.ActualPos, projectile.CollisionSize * 0.8f);
        }

        public static bool UpdateProjectile(ref Projectile projectile, int idx)
        {
            projectile.Origin += projectile.Velocity * Horde.Time.DeltaTime;

            projectile.ActualPos.x = projectile.Origin.x + Mathf.Sin(Time.time * 4 + idx);
            projectile.ActualPos.y = projectile.Origin.y + Mathf.Cos(Time.time * 4 + idx);

            return !CollisionUtil.IsCircleColliding(projectile.ActualPos, projectile.CollisionSize);
        }

        public static bool UpdateProjectile2(ref Projectile p, int idx)
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
