using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody), typeof(ActorFeelings))]
    public class ActorAiController_Tired  : MonoBehaviour, IComponentUpdate
    {
        ActorPhysicsBody body_;
        ActorFeelings feelings_;

        void Awake()
        {
            body_ = GetComponent<ActorPhysicsBody>();
            feelings_ = GetComponent<ActorFeelings>();
        }

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            if (feelings_.CurrentFeeling == FeelingEnum.Tired)
            {
            }
        }
    }
}
