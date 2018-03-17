using UnityEngine;

namespace HordeEngine
{
    public static class ProjectileSpawners
    {
        public static void SpawnCircle(ProjectileDescription desc, Vector2 origin, int count, float velocity, ProjectileManager manager, Projectile.TickDelegate updateFunc)
        {
            for (int i = 0; i < count; ++i)
            {
                float angle = (Mathf.Deg2Rad * 360 / count) * i;
                var dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * 0.5f;
                var pos = origin + dir * 0.5f;
                var p = new Projectile()
                {
                    StartPos = pos,
                    Origin = pos,
                    ActualPos = pos,
                    Velocity = dir * velocity,
                    UpdateCallback = updateFunc
                };
                p.ApplyDescription(Global.SceneAccess.ProjectileDescriptions.Yellow);
                manager.SpawnProjectile(ref p);
            }
        }

        public static void SpawnPattern(ProjectileDescription desc, Vector2 origin, Vector2 velocity, string[] pattern, ProjectileManager manager)
        {
            foreach(var pos in ProjectilePatterns.PatternPositions(pattern, 0.5f))
            {
                var p = new Projectile()
                {
                    Origin = origin,
                    StartPos = origin + pos,
                    OriginOffset = origin + pos,
                    Velocity = velocity,
                    UpdateCallback = ProjectileUpdaters.UpdateProjectile,
                };
                p.ActualPos = p.StartPos;
                p.ApplyDescription(desc);

            }
        }
    }
}
