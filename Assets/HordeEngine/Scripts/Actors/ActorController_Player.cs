using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody))]
    public class ActorController_Player : MonoBehaviour, IComponentUpdate
    {
        public float MoveSpeed = 5.0f;
        public SpriteAnimationFrames_IdleRun Anim;
        public HordeSprite PlayerSpriteRenderer;

        Transform trans_;
        ActorPhysicsBody actorBody_;
        ActorAbility_Dodge dodge_;
        bool flipX_;

        private void Awake()
        {
            trans_ = transform;
            actorBody_ = GetComponent<ActorPhysicsBody>();
            dodge_ = GetComponent<ActorAbility_Dodge>();

            PlayerCollision.PlayerSize = actorBody_.CollisionCircleSize;
            PlayerCollision.OnPlayerCollision = Hit;
            PlayerCollision.PlayerBody = actorBody_;
        }

        void OnValidate()
        {
            if (Anim != null && Anim.Idle != null && Anim.Idle.Length > 0 && PlayerSpriteRenderer != null)
                PlayerSpriteRenderer.Sprite = Anim.Idle[0];
        }

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        Vector2 frameForce_;
        public void Hit(ref Projectile p)
        {
            if (dodge_.IsDodging)
                return;

            frameForce_ += p.Velocity.normalized;
        }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            actorBody_.AddForce(frameForce_.normalized * 2);
            frameForce_ = Vector2.zero;

            PlayerCollision.PlayerPos = trans_.localPosition + (Vector3)actorBody_.CollisionCircleOffset;

            float td = Horde.Time.DeltaTime;

            if (Input.GetKeyDown(KeyCode.R))
                Global.SceneAccess.CameraShake.AddTrauma(1.0f);

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
            {
                ProjectileSpawners.SpawnCircle(Global.SceneAccess.ProjectileBlueprints.Yellow, false, trans_.localPosition + Vector3.down * 0.75f, 250, 5.0f, Global.SceneAccess.ProjectileManager, ProjectileUpdaters.BasicMove);
                Global.SceneAccess.CameraShake.AddTrauma(1.0f);
            }

            Vector3 inputVec = Vector3.zero;
            inputVec.x = Input.GetAxis("Horizontal");
            inputVec.y = Input.GetAxis("Vertical");

            bool isRunning = inputVec.sqrMagnitude > 0.0f;
            if (isRunning)
            {
                var velocity = inputVec.normalized * td * MoveSpeed;
                actorBody_.Move(velocity);

                if (Input.GetMouseButtonDown(1) && !dodge_.IsDodging)
                    dodge_.DoDodge(inputVec);
            }

            flipX_ = Global.Crosshair.GetDirectionVector(trans_.localPosition).x < 0.0f;

            PlayerSpriteRenderer.Sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
            PlayerSpriteRenderer.FlipX = flipX_;
        }
    }
}
