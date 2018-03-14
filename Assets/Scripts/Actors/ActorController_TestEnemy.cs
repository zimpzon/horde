using System.Collections;
using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody))]
    public class ActorController_TestEnemy : MonoBehaviour, IComponentUpdate
    {
        public float MoveSpeed = 3.0f;
        public SpriteAnimationFrames_IdleRun Anim;
        public SpriteRenderer ActorSpriteRenderer;

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
            StartCoroutine(Think());
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

        void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            float td = Global.TimeManager.DeltaSlowableTime;

            if (Random.value < 0.005f)
                actorBody_.AddForce(Random.insideUnitCircle.normalized * (Random.value * 5.0f));

            bool isRunning = dir_.sqrMagnitude > 0.0f;
            if (isRunning)
            {
                flipX_ = dir_.x < 0.0f;

                var velocity = dir_.normalized * td * MoveSpeed;
                actorBody_.Move(velocity);
            }

            ActorSpriteRenderer.sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
            ActorSpriteRenderer.flipX = flipX_;
        }
    }
}
