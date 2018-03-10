using HordeEngine;
using UnityEngine;

[RequireComponent(typeof(ActorPhysicsBody))]
public class ActorAbility_Dodge : MonoBehaviour
{
    public bool UseSlowableTime = true;
    public float DodgeLength = 8.0f;
    public float DodgeTimeMs = 100.0f;

    bool isDodging_;
    ActorPhysicsBody actorBody_;
    Vector3 dodgeVelocity_;
    float currentDodgeLen_;
    Transform trans_;

    private void Awake()
    {
        trans_ = transform;
        actorBody_ = GetComponent<ActorPhysicsBody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isDodging_)
        {
            float velocity = DodgeLength / (DodgeTimeMs / 1000.0f); // Reach destination in DodgeTimeMs
            dodgeVelocity_ = Global.Crosshair.GetDirectionVector(trans_.localPosition, normalize: true) * velocity;
            isDodging_ = true;
        }

        if (dodgeVelocity_.sqrMagnitude >= 0.0f)
        {
            var dodgeVec = dodgeVelocity_ * Global.TimeManager.GetDeltaTime(UseSlowableTime);
            float frameDodgeLen = dodgeVec.magnitude;
            currentDodgeLen_ += frameDodgeLen;
            if (currentDodgeLen_ >= DodgeLength)
            {
                // Reached end of dodge. Clamp to exact length.
                frameDodgeLen = currentDodgeLen_ - DodgeLength;
                dodgeVec = dodgeVec.normalized * frameDodgeLen;
                currentDodgeLen_ = 0.0f;
                dodgeVelocity_ = Vector3.zero;
                isDodging_ = false;
            }

            actorBody_.Move(dodgeVec);
        }
    }
}
