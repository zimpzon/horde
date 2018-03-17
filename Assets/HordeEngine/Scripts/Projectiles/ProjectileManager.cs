﻿using HordeEngine;
using UnityEngine;

public class ProjectileManager : MonoBehaviour, IComponentUpdate
{
    public int InitialCapacity = 1000;
    [SerializeField, Layer] public LayerMask ProjectileLayer;
    [SerializeField, Layer] public LayerMask LightLayer;
    public float OffsetY;

    // To allow updating structs in place we have to use array. A List<> will return a copy when accessing an element.
    Projectile[] projectiles_;
    int projectileLayer_;
    int lightLayer_;

    [Header("Debug")]
    public int ActiveProjectiles;

    void UpdateLayers()
    {
        projectileLayer_ = (int)ProjectileLayer;
        lightLayer_ = (int)LightLayer;
    }

    void Awake()
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

    public void FireProjectile(Vector3 pos)
    {
        Vector3 p = pos;

        int steps = 100;
        for (int i = 0; i < steps; ++i)
        {
            float angle = (Mathf.Deg2Rad * 360 / steps) * i;
            var spr = new Projectile()
            {
                Z = -2,
                Size = Vector2.one * 1f,
                LightSize = Vector2.one * 2.0f,
                StartPos = p,
                Origin = p,
                EmitLight = false,
                LightOffsetY = 0.25f,
                Velocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * 0.1f,
                CollisionSize = 1.0f,
                Color = Color.white,
                UpdateCallback = TickProjectile2
            };
            spr.ApplyDescription(Global.SceneAccess.ProjectileDescriptions.Yellow);

            spr.LightColor = spr.Color;
            spr.ActualPos = spr.StartPos;
            projectiles_[ActiveProjectiles++] = spr;

            // Not here
            if (ActiveProjectiles == projectiles_.Length)
            {
                System.Array.Resize(ref projectiles_, projectiles_.Length + (projectiles_.Length / 2));
                Debug.Log("Projectile array was expanded. Consider increasing initial capacity. New size: " + projectiles_.Length);
            }
        }
    }

    bool TickProjectile(ref Projectile projectile, int idx)
    {
        projectile.Origin += projectile.Velocity * Horde.Time.DeltaTime;

        projectile.ActualPos.x = projectile.Origin.x + Mathf.Sin(Time.time * 4 + idx);
        projectile.ActualPos.y = projectile.Origin.y + Mathf.Cos(Time.time * 4 + idx);

        return !CollisionUtil.IsCircleColliding(projectile.ActualPos, projectile.CollisionSize);
    }

    bool TickProjectile2(ref Projectile p, int idx)
    {
        p.ActualPos += p.Velocity * Horde.Time.DeltaTime;
//        p.Velocity += p.Velocity * 1.5f * Time.deltaTime;
        return !CollisionUtil.IsCircleColliding(p.ActualPos, p.CollisionSize);
    }

    void RenderProjectile(ref Projectile p)
    {
        Vector3 pos = p.ActualPos;
        pos.z = p.Z;
        pos.y += OffsetY;
        Horde.Sprites.AddQuad(pos, p.Size, 0.0f, p.Size.y, p.Color, p.Sprite, p.Material, projectileLayer_);

        if (p.EmitLight)
        {
            pos.y += p.LightOffsetY;
            Horde.Sprites.AddQuad(pos, p.LightSize, 0.0f, p.Size.y, p.LightColor, p.Sprite, p.Material, lightLayer_);
        }
    }

    void OnEnable()
    {
        UpdateLayers();
        Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late);
    }

    void OnDisable()
    {
        Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late);
    }

    public void ComponentUpdate(ComponentUpdatePass pass)
    {
        int i = 0;
        while (i < ActiveProjectiles)
        {
            bool success = projectiles_[i].UpdateCallback(ref projectiles_[i], i);
            if (!success)
            {
                RemoveProjectile(i);
                // Don't increase i. The last item in list is moved
                // to current spot so next iteration will process that.
                continue;
            }

            RenderProjectile(ref projectiles_[i]);

            i++;
        }
    }
}
