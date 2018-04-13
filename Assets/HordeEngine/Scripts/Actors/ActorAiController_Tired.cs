using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody), typeof(ActorFeelings), typeof(ActorAiMovement))]
    public class ActorAiController_Tired  : MonoBehaviour, IComponentUpdate
    {
        ActorPhysicsBody body_;
        ActorFeelings feelings_;
        ActorAiMovement movement_;

        void Awake()
        {
            body_ = GetComponent<ActorPhysicsBody>();
            feelings_ = GetComponent<ActorFeelings>();
            movement_ = GetComponent<ActorAiMovement>();
        }

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            if (feelings_.CurrentFeeling == FeelingEnum.Tired)
            {
                movement_.MoveTowardsTargetPosition = false;

                float dt = Horde.Time.DeltaSlowableTime;
                feelings_.TryUpdateFeeling(FeelingEnum.Tired, -(1.0f / 5) * dt);
            }
        }
    }
}
