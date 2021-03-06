﻿using UnityEngine;
using HordeEngine;

// Object to shake will be forced at 0, 0, 0
public class CameraShake : MonoBehaviour, IComponentUpdate
{
    public bool ShakeRotation = true;
    public bool ShakePosition = true;
    public float Dampening = 4.0f;
    public float Scale = 0.2f;
    public float CurrentAmount;
    Transform trans_;

    private void Awake()
    {
        trans_ = transform;
    }

    public void AddShake(float amount)
    {
        CurrentAmount = Mathf.Clamp01(CurrentAmount + amount);
    }

    void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late); }
    void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late); }

    public void ComponentUpdate(ComponentUpdatePass pass)
    {
        float t = Horde.Time.Time * 10.0f;
        float dt = Horde.Time.Time;
        float power = CurrentAmount * CurrentAmount * CurrentAmount;
        if (ShakePosition)
        {
            trans_.localPosition = new Vector3(
                Scale * power * Mathf.PerlinNoise(t + 1, t + 3.33f),
                Scale * power * Mathf.PerlinNoise(t + 2, t + 4.44f) * 0.25f,
                0.0f
            );
        }
        else
        {
            trans_.localPosition = Vector3.zero;
        }

        const float MaxDegrees = 5;
        if (ShakeRotation)
            trans_.localRotation = Quaternion.Euler(0, 0, Scale * power * (Mathf.PerlinNoise(t + 5, t + 6.66f) - 0.5f) * MaxDegrees);
        else
            trans_.localRotation = Quaternion.Euler(0, 0, 0);

        CurrentAmount = Mathf.Clamp01(CurrentAmount - dt * Dampening);
	}
}
