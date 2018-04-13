using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody))]
    [RequireComponent(typeof(ActorFeelings))]
    [RequireComponent(typeof(ActorAiMovement))]
    [RequireComponent(typeof(ActorLineOfSight))]
    public class ActorAiController_Fight : MonoBehaviour, IComponentUpdate
    {
        ActorPhysicsBody body_;
        ActorFeelings feelings_;
        ActorAiMovement movement_;
        ActorLineOfSight lineOfSight_;

        void Awake()
        {
            body_ = GetComponent<ActorPhysicsBody>();
            feelings_ = GetComponent<ActorFeelings>();
            movement_ = GetComponent<ActorAiMovement>();
            lineOfSight_ = GetComponent<ActorLineOfSight>();
        }

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            if (feelings_.CurrentFeeling == FeelingEnum.Fight)
            {
                float dt = Horde.Time.DeltaSlowableTime;

                Vector2 targetPos;
                if (lineOfSight_.TryGetRecentLineOfSightPosition(out targetPos, maxAge: 10.0f))
                {
                    movement_.SetTargetPosition(targetPos);
                }
                else
                {
                    // Nothing to chase
                    movement_.MoveTowardsTargetPosition = false;
                }
            }
        }
    }
}
