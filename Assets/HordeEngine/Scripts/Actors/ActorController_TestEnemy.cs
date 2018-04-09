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
            StartCoroutine(Think());
        }

        Vector2 dir_ = Vector2.zero;

        IEnumerator Think()
        {
            while (true)
            {
                // Walk
                dir_ = Random.insideUnitCircle.normalized;
                yield return Yielders.WaitForSeconds(Random.value * 3 + 3);
                dir_ = Vector2.zero;

                //var startPos = trans_.localPosition + Vector3.down * 0.5f;
                //ProjectileSpawners.SpawnCircle(Global.SceneAccess.ProjectileBlueprints.Bullet2, true, startPos, 3, 1.0f, Global.SceneAccess.ProjectileManager, ProjectileUpdaters.ChasePlayer);
                //yield return Yielders.WaitForSeconds(1.5f);

                //ProjectileSpawners.SpawnCircle(Global.SceneAccess.ProjectileBlueprints.Bullet1, true, trans_.localPosition + Vector3.down * 0.5f, 50, 10.0f, Global.SceneAccess.ProjectileManager, ProjectileUpdaters.CirclingMove);
                //yield return Yielders.WaitForSeconds(2);

                //ProjectileSpawners.SpawnCircle(Global.SceneAccess.ProjectileBlueprints.Bullet0, true, trans_.localPosition + Vector3.down * 0.5f, 100, 8.0f, Global.SceneAccess.ProjectileManager, ProjectileUpdaters.BasicMove);
                //yield return Yielders.WaitForSeconds(2);
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
