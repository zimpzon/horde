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
        const float MovePct = 5.0f;
        currentPos_ += (target_ - currentPos_) * MovePct * Global.TimeManager.DeltaSlowableTime;
        trans_.localPosition = currentPos_;
    }
}
