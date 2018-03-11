using HordeEngine;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActorPhysicsBody))]
public class ActorController_TestEnemy : MonoBehaviour
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

    void Update()
    {
        float td = Global.TimeManager.DeltaSlowableTime;

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
