using System.Collections;
using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody))]
    public class ActorController_TestEnemy : MonoBehaviour, IComponentUpdate
    {
        public float MoveSpeed = 3.0f;
        public SpriteAnimationFrames_IdleRun Anim;
        public HordeSprite ActorSpriteRenderer;

        Transform trans_;
        ActorPhysicsBody actorBody_;
        ActorAbility_Dodge dodge_;
        bool flipX_;

        private void Awake()
        {
            trans_ = transform;
            actorBody_ = GetComponent<ActorPhysicsBody>();
            dodge_ = GetComponent<ActorAbility_Dodge>();
        }

        private void Start()
        {
//            StartCoroutine(Think());
        }

        Vector2 dir_ = Vector2.zero;

        IEnumerator Think()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.value * 1 + 0.5f);
                dir_ = Random.insideUnitCircle.normalized;
                yield return new WaitForSeconds(Random.value * 1 + 0.5f);
                dir_ = Vector2.zero;
            }
        }

        void OnValidate()
        {
            if (Anim != null && Anim.Idle != null && Anim.Idle.Length > 0 && ActorSpriteRenderer != null)
                ActorSpriteRenderer.Sprite = Anim.Idle[0];
        }

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            float td = Horde.Time.DeltaSlowableTime;

            if (Random.value < 0.01f)
                actorBody_.AddForce(Random.insideUnitCircle.normalized * (Random.value * 50.0f + 1.0f));

            bool isRunning = dir_.sqrMagnitude > 0.0f;
            if (isRunning)
            {
                flipX_ = dir_.x < 0.0f;

                var velocity = dir_.normalized * td * MoveSpeed;
                actorBody_.Move(velocity);
            }

            ActorSpriteRenderer.Sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
            ActorSpriteRenderer.FlipX = flipX_;
        }
    }
}
