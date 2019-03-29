using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public float Speed = 10;
    public SpriteAnimationFrames_IdleRun Anim;
    public float LookAtOffset = 10;
    public Tilemap Walls;

    Transform transform_;
    SpriteRenderer renderer_;
    Rigidbody2D body_;
    CameraPositioner camPositioner_;
    MapScript map_;
    bool flipX_;
    Vector3 lookAt_;

    void Awake()
    {
        transform_ = transform;
        renderer_ = GetComponent<SpriteRenderer>();
        body_ = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        map_ = SceneGlobals.Instance.MapScript;
        lookAt_ = transform_.position;
        camPositioner_ = SceneGlobals.Instance.CameraPositioner;
        camPositioner_.Target = lookAt_;
        camPositioner_.SetPosition(lookAt_);
    }

    void UpdatePlayer(float dt)
    {
        var horz = Input.GetAxisRaw("Horizontal");
        var vert = Input.GetAxisRaw("Vertical");
        Vector3 moveVec = new Vector3(horz, vert) * Speed * dt;

        bool isRunning = moveVec != Vector3.zero;
        if (isRunning)
        {
            body_.MovePosition(transform_.position + moveVec);
            flipX_ = moveVec.x < 0;
            lookAt_ = transform_.position + moveVec * LookAtOffset;
        }

        renderer_.sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
        renderer_.flipX = flipX_;

        camPositioner_.Target = lookAt_;
    }

    void FixedUpdate()
    {
        UpdatePlayer(Time.fixedUnscaledDeltaTime);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
            map_.ClearWallCircle(transform.position, 5);
    }
}
