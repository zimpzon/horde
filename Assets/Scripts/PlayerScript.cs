using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float Speed = 10;
    public SpriteAnimationFrames_IdleRun Anim;

    Transform transform_;
    SpriteRenderer renderer_;
    Rigidbody2D body_;
    bool flipX_;

    void Awake()
    {
        transform_ = transform;
        renderer_ = GetComponent<SpriteRenderer>();
        body_ = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float dt = Time.fixedUnscaledDeltaTime;
        var horz = Input.GetAxisRaw("Horizontal");
        var vert = Input.GetAxisRaw("Vertical");
        Vector3 vec = new Vector3(horz, vert, 0) * Speed * dt;

        bool isRunning = vec != Vector3.zero;
        if (isRunning)
        {
            body_.MovePosition(transform_.position + vec);
            flipX_ = vec.x < 0;
        }

        renderer_.sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
        renderer_.flipX = flipX_;
    }
}
