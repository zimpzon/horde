using UnityEngine;

namespace HordeEngine
{
    public class ActorPhysicsBody : MonoBehaviour, IComponentUpdate
    {
        public float Width = 1.0f;
        public float Drag = 1.0f;
        public float Bounciness = 0.5f;
        public float CollisionGranularity = 0.49f;
        public bool UseSlowableTime = true;

        Vector2 force_;
        Vector2 pendingMovement_;
        Transform trans_;

        private void Awake()
        {
            trans_ = transform;
        }

        public void AddForce(Vector2 force)
        {
            force_ += force;
        }

        public void SetForce(Vector2 force)
        {
            force_ = force;
        }

        /// <summary>
        /// Direct movement in current frame only.
        /// </summary>
        /// <param name="velocity"></param>
        public void Move(Vector2 velocity)
        {
            pendingMovement_ += velocity;
        }

        void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late); }
        void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late); }

        public void ComponentUpdate(ComponentUpdatePass pass)
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
                Vector2 collisionNormal;
                bool collided = !CollisionUtil.TryMovePoints(CollisionUtil.TempList, totalMove, out maxAllowedMove, out collisionNormal);
                trans_.localPosition += (Vector3)maxAllowedMove;
                if (collided)
                {
                    // There was a collision. Reflect the current force against the collider.
                    Vector2 reflectedForce = Vector2.Reflect(totalMove, collisionNormal).normalized * force_.magnitude * Bounciness;
                    SetForce(reflectedForce);
                    // After a collision the body position is clamped just in front of the collider. Add the remaining velocity to be added in next update.
                    float remainingVelocity = (totalMove.magnitude - maxAllowedMove.magnitude);
                    Move(force_.normalized * remainingVelocity);

                    Debug.LogFormat("Reflect {0} @ {1} = {2}", totalMove.ToString("0.00000000"), collisionNormal.ToString("0.00000000"), reflectedForce.ToString("0.00000000"));
                }
            }

            // Dampen force
            if (force_.sqrMagnitude > 0.0f)
            {
                float forceLen = force_.magnitude;
                forceLen = Mathf.Clamp(forceLen - Drag * dt, 0.0f, float.MaxValue);
//                force_ = force_.normalized * forceLen;
            }
        }
    }
}
