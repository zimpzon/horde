﻿using HordeEngine;
using UnityEngine;

public class ProjectileManager : MonoBehaviour, IComponentUpdate
{
    public int InitialCapacity = 1000;
    public DynamicQuadRenderer ProjectileRenderer;
    public DynamicQuadRenderer LightRenderer;

    // To allow updating structs in place we have to use array. A List<> will return a copy when accessing an element.
    Projectile[] projectiles_;

    [Header("Debug")]
    public int ActiveProjectiles;

    private void Awake()
    {
        projectiles_ = new Projectile[InitialCapacity];
    }

    public void Clear()
    {
        ActiveProjectiles = 0;
    }

    public void RemoveProjectile(int idx)
    {
        projectiles_[idx] = projectiles_[ActiveProjectiles - 1];
        ActiveProjectiles--;
    }

    public void FireProjectile(Vector2 p)
    {
        if (ActiveProjectiles == projectiles_.Length)
        {
            // TODO: After 3375 something throws
            System.Array.Resize(ref projectiles_, projectiles_.Length + (projectiles_.Length / 2));
            Debug.Log("Projectile array was expanded. Consider increasing initial capacity. New size: " + projectiles_.Length);
        }

        int steps = 10;
        for (int i = 0; i < steps; ++i)
        {
            float angle = (360.0f / steps) * i;
            var spr = new Projectile()
            {
                Size = Vector2.one * 0.5f,
                LightSize = Vector2.one * 3,
                StartPos = p,
                Origin = p,
                RenderLight = true,
                Velocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * 2,
                CollisionSize = 0.5f,
                Color = new Color(Random.value, Random.value, Random.value),
                TickCallback = TickProjectile2
            };
            spr.LightColor = spr.Color;
            spr.ActualPos = spr.StartPos;
            projectiles_[ActiveProjectiles++] = spr;
        }
    }

    bool TickProjectile(ref Projectile projectile, int idx)
    {
        projectile.Origin += projectile.Velocity * Global.TimeManager.DeltaTime;

        projectile.ActualPos.x = projectile.Origin.x + Mathf.Sin(Time.time + idx);
        projectile.ActualPos.y = projectile.Origin.y + Mathf.Cos(Time.time + idx);

        return !CollisionUtil.IsCircleColliding(projectile.ActualPos, projectile.CollisionSize);
    }

    bool TickProjectile2(ref Projectile p, int idx)
    {
        p.ActualPos += p.Velocity * Global.TimeManager.DeltaTime;
        p.Velocity += p.Velocity * 0.01f * Time.deltaTime;
        return !CollisionUtil.IsCircleColliding(p.ActualPos, p.CollisionSize);
    }

    void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
    void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

    public void ComponentUpdate(ComponentUpdatePass pass)
    {
        int i = 0;
        while (i < ActiveProjectiles)
        {
            bool success = projectiles_[i].TickCallback(ref projectiles_[i], i);
            if (!success)
            {
                RemoveProjectile(i);
                // Don't increase i. The last item in list is moved
                // to current spot so next iteration will process that.
                continue;
            }

            Vector2 size = projectiles_[i].Size;
            Vector2 lightSize = projectiles_[i].LightSize;

            // Might split these into different passes. Seems nicer. How about perf?
            ProjectileRenderer.QuadMesh.AddQuad(projectiles_[i].ActualPos, size, 0.0f, size.y, projectiles_[i].Color);
            if (projectiles_[i].RenderLight)
                LightRenderer.QuadMesh.AddQuad(projectiles_[i].ActualPos, lightSize, 0.0f, lightSize.y, projectiles_[i].LightColor);

            i++;
        }
    }
}
