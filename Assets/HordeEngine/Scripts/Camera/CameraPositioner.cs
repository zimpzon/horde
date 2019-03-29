using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    public float MoveSpeed = 8.0f;
    public Transform Target;

    Vector3 currentPos_;
    Transform trans_;

    private void Awake()
    {
        trans_ = transform;
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void SetPosition(Vector3 pos)
    {
        currentPos_ = pos;
    }

    public void Update()
    {
        var movement = (Target.position - currentPos_) * MoveSpeed;

        const float CloseEnough = 0.1f;
        if (movement.sqrMagnitude < CloseEnough * CloseEnough)
            return;

        // Keep a minimum speed so the camera don't spend seconds on the last few pixels.
        const float MinimumMovement = 1.0f;
        if (movement.sqrMagnitude < MinimumMovement * MinimumMovement)
            movement = movement.normalized * MinimumMovement;

        currentPos_ += movement * Time.unscaledDeltaTime;
        trans_.localPosition = currentPos_;
    }
}
