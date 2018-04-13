using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody))]
    [RequireComponent(typeof(ActorFeelings))]
    public class ActorAiMovement : MonoBehaviour, IComponentUpdate
    {
        public float MoveSpeed = 3.0f;
        public float Stamina = 0.5f;
        public SpriteAnimationFrames_IdleRun Anim;
        public HordeSprite ActorSpriteRenderer;

        public bool MoveTowardsTargetPosition;

        Vector2 target_;
        Transform trans_;
        ActorPhysicsBody actorBody_;
        ActorFeelings feelings_;

        bool flipX_;

        private void Awake()
        {
            trans_ = transform;
            actorBody_ = GetComponent<ActorPhysicsBody>();
            feelings_ = GetComponent<ActorFeelings>();
        }

        public void SetTargetPosition(Vector2 pos, bool doMove = true)
        {
            target_ = pos;
            UpdateDirection();
            MoveTowardsTargetPosition = doMove;
        }

        void UpdateDirection()
        {
            dir_ = (target_ - actorBody_.Position).normalized;
        }

        Vector2 dir_ = Vector2.zero;

        void OnValidate()
        {
            if (Anim != null && Anim.Idle != null && Anim.Idle.Length > 0 && ActorSpriteRenderer != null)
                ActorSpriteRenderer.Sprite = Anim.Idle[0];
        }

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            float dt = Horde.Time.DeltaSlowableTime;

            bool isRunning = dir_.sqrMagnitude > 0.0f && MoveTowardsTargetPosition;
            if (isRunning)
            {
                flipX_ = dir_.x < 0.0f;

                var velocity = dir_.normalized * dt * MoveSpeed;
                actorBody_.Move(velocity);

                const float StaminaScale = 0.1f;
                feelings_.TryUpdateFeeling(FeelingEnum.Tired, (1.0f - Stamina) * dt * MoveSpeed * StaminaScale);
            }

            ActorSpriteRenderer.Sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
            ActorSpriteRenderer.FlipX = flipX_;
        }
    }
}
