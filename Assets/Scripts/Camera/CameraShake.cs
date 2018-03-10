using HordeEngine;
using UnityEngine;

// Object to shake will be forced at 0, 0, 0
public class CameraShake : MonoBehaviour
{
    const float TraumaDampening = 4.0f;
    const float Scale = 0.2f;
    float traumaValue_;
    Transform trans_;

    private void Awake()
    {
        trans_ = transform;
    }

    public void AddForce(Vector2 force)
    {
        // TODO: Add spring-like 2D motion.
    }

    public void AddTrauma(float amount)
    {
        traumaValue_ = Mathf.Clamp01(traumaValue_ + amount);
    }

	void Update()
    {
        float t = Global.TimeManager.SlowableTime * 10.0f;
        float dt = Global.TimeManager.DeltaSlowableTime;
        float power = traumaValue_ * traumaValue_ * traumaValue_;
        trans_.localPosition = new Vector3(
            Scale * power * Mathf.PerlinNoise(t + 1, t + 3.33f),
            Scale * power * Mathf.PerlinNoise(t + 2, t + 4.44f) * 0.25f,
            0.0f
        );

        const float MaxDegrees = 5;
        trans_.localRotation = Quaternion.Euler(0.0f, 0.0f, Scale * power * (Mathf.PerlinNoise(t + 5, t + 6.66f) - 0.5f) * MaxDegrees);

        traumaValue_ = Mathf.Clamp01(traumaValue_ - dt * TraumaDampening);
	}
}
