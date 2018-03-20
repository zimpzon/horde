﻿using UnityEngine;

namespace HordeEngine
{
    public static class ProjectileSpawners
    {
        static Projectile proto = new Projectile();

        public static void SpawnCircle(ProjectileBlueprint desc, bool collidePlayer, Vector2 origin, int count, float velocity, ProjectileManager manager, Projectile.TickDelegate updateFunc)
        {
            proto.Reset();
            proto.ApplyBlueprint(desc);
            for (int i = 0; i < count; ++i)
            {
                float angleDegrees = (360.0f / count) * i;
                float angle = angleDegrees * Mathf.Deg2Rad;
                var dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * 0.5f;
                var pos = origin + dir * 0.5f;

                proto.Idx = i;
                proto.StartPos = pos;
                proto.Origin = pos;
                proto.ActualPos = pos;
                proto.RotationDegrees = angleDegrees;
                proto.Velocity = dir * velocity;
                proto.UpdateCallback = updateFunc;
                proto.CollidePlayer = collidePlayer;

                manager.SpawnProjectile(ref proto);
            }
        }

        public static void SpawnSingle(ProjectileBlueprint desc, bool collidePlayer, Vector2 origin, Vector2 dir, float velocity, ProjectileManager manager, Projectile.TickDelegate updateFunc)
        {
            proto.Reset();
            proto.ApplyBlueprint(desc);

            proto.StartPos = origin;
            proto.Origin = origin;
            proto.ActualPos = origin;
            proto.RotationDegrees = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            proto.Velocity = dir * velocity;
            proto.UpdateCallback = updateFunc;
            proto.CollidePlayer = collidePlayer;

            manager.SpawnProjectile(ref proto);
        }

        public static void SpawnPattern(ProjectileBlueprint desc, Vector2 origin, Vector2 velocity, string[] pattern, ProjectileManager manager)
        {
            proto.Reset();
            proto.ApplyBlueprint(desc);
            foreach (var pos in ProjectilePatterns.PatternPositions(pattern, 0.5f))
            {
                proto.Origin = origin;
                proto.StartPos = origin + pos;
                proto.OriginOffset = origin + pos;
                proto.Velocity = velocity;
                proto.UpdateCallback = ProjectileUpdaters.BasicMove;
                proto.ActualPos = proto.StartPos;
            }
        }
    }
}
