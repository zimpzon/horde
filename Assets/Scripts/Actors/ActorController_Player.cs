using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody))]
    public class ActorController_Player : MonoBehaviour, IComponentUpdate
    {
        public float MoveSpeed = 5.0f;
        public SpriteAnimationFrames_IdleRun Anim;
        public SpriteRenderer PlayerSpriteRenderer;

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

        void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            float td = Global.TimeManager.DeltaTime;

            if (Input.GetKeyDown(KeyCode.R))
                Global.SceneAccess.CameraShake.AddTrauma(1.0f);

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

            PlayerSpriteRenderer.sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
            PlayerSpriteRenderer.flipX = flipX_;
        }
    }
}
