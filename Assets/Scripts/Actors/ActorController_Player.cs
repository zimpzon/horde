using HordeEngine;
using UnityEngine;

[RequireComponent(typeof(ActorPhysicsBody))]
public class ActorController_Player : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public SpriteAnimationFrames_IdleRun Anim;
    public SpriteRenderer PlayerSpriteRenderer;

    ActorPhysicsBody actorBody_;
    bool flipX_;

    private void Awake()
    {
        actorBody_ = GetComponent<ActorPhysicsBody>();
    }

    void Update()
    {
        float td = Global.TimeManager.DeltaTime;

        if (Input.GetKeyDown(KeyCode.R))
            Global.SceneAccess.CameraShake.AddTrauma(1.0f);

        Vector3 inputVec = Vector3.zero;
        inputVec.x = Input.GetAxis("Horizontal");
        inputVec.y = Input.GetAxis("Vertical");
        var velocity = inputVec.normalized * td * MoveSpeed;
        actorBody_.Move(velocity);

        bool isRunning = velocity.sqrMagnitude > 0.0f;
        if (isRunning)
            flipX_ = velocity.x < 0.0f;

        PlayerSpriteRenderer.sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
        PlayerSpriteRenderer.flipX = flipX_;
    }
}
