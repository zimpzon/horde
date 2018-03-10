using HordeEngine;
using UnityEngine;

public class ActorPhysicsBody : MonoBehaviour
{
    public float Width = 1.0f;
    public float Drag = 1.0f;
    public float CollisionGranularity = 0.49f;
    public bool UseSlowableTime = true;

    Vector2 force_;
    Vector2 pendingMovement_;
    Transform trans_;

    private void Awake()
    {
        trans_ = transform;
    }

    /// <summary>
    /// A force length of 1 is 1 unit per second. Will be dampened according to Drag.
    /// </summary>
    /// <param name="force"></param>
    public void AddForce(Vector2 force)
    {
        force_ += force;
    }

    /// <summary>
    /// Direct movement. Not affected by frame rate or time scale.
    /// </summary>
    /// <param name="velocity"></param>
    public void Move(Vector2 velocity)
    {
        pendingMovement_ += velocity;
    }

    private void LateUpdate()
    {
        float dt = Global.TimeManager.GetDeltaTime(UseSlowableTime);

        var totalMove = force_ * dt + pendingMovement_;
        pendingMovement_ = Vector2.zero;

        bool isMoving = totalMove.sqrMagnitude > 0.0f;
        if (isMoving && CollisionUtil.CollisionMap != null)
        {
            int pointCount = Mathf.CeilToInt(Width / CollisionGranularity);
            float pointStep = Width / pointCount;

            // Start at the left edge of the body, then step forward
            Vector2 point = trans_.localPosition + Vector3.left * Width;
            CollisionUtil.AddPointsForLine(CollisionUtil.TempList, trans_.localPosition, Width, CollisionGranularity);

            Vector2 maxAllowedMove;
            CollisionUtil.TryMovePoints(CollisionUtil.TempList, totalMove, out maxAllowedMove);

            trans_.localPosition += (Vector3)maxAllowedMove;
        }

        // Dampen force
        if (force_.sqrMagnitude > 0.0f)
        {
            float forceLen = force_.magnitude;
            forceLen = Mathf.Clamp(forceLen - Drag * dt, 0.0f, float.MaxValue);
            force_ = force_.normalized * forceLen;
        }
    }
}
