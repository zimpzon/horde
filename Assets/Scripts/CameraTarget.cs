using HordeEngine;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    Vector3 target_;
    Vector3 currentPos_;
    Transform trans_;

    private void Awake()
    {
        trans_ = transform;
    }

    public void SetTarget(Vector3 target)
    {
        target_ = target;
    }

    public void SetPosition(Vector3 pos)
    {
        currentPos_ = pos;
    }

    void Update()
    {
        const float MoveFactor = 5.0f;
        var movement = (target_ - currentPos_) * MoveFactor;

        const float CloseEnough = 0.1f;
        if (movement.sqrMagnitude < CloseEnough * CloseEnough)
            return;

        // Keep a minimum speed so the camera don't spend seconds on the last few pixels.
        const float MinimumMovement = 1.0f;
        if (movement.sqrMagnitude < MinimumMovement * MinimumMovement)
            movement = movement.normalized * MinimumMovement;

        currentPos_ += movement * Global.TimeManager.DeltaSlowableTime;
        trans_.localPosition = currentPos_;
    }
}
