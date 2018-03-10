using HordeEngine;
using UnityEngine;

// Dodge, Physics (pushable etc), wield weapon, explode (something)
// Shared data...

public class PlayerScript : MonoBehaviour
{
    //    Dungeon of the Endless
    //https://www.youtube.com/watch?v=zPQOHX9hiL0

    //Dudes
    //https://0x72.itch.io/pixeldudesmaker

    //Music
    //http://soundimage.org/fantasy-2/

    // Draw walls with different alpha to get different color? Or just different color?
    // Make user answer questions so he learns something? Not blocking.
    // Can I do something with OpenStreetMap data?
    // really easy lights: vertex colors
    // Use props to place stufff? Lights (not just torches), traps, whatever?

    float DodgeLength = 8.0f;

    public SpriteAnimationFrames_IdleRun Anim;
    public SpriteRenderer Renderer;
    public Transform PlayerLightTrans;
    public Transform Transform;

    MaterialColorSetter playerLightColorSetter_;
    bool flipX_;
    Vector3 dodgeForce_;
    float currentDodgeLen_;

    float size_ = 1.0f;

    void Awake()
    {
        Transform = transform;
        playerLightColorSetter_ = PlayerLightTrans.GetComponent<MaterialColorSetter>();
    }

    void Update()
    {
        if (CollisionUtil.CollisionMap == null)
            return;

        float td = Global.TimeManager.DeltaTime;

        if (Input.GetKeyDown(KeyCode.R))
            Global.SceneAccess.CameraShake.AddTrauma(1.0f);

        if (Input.GetMouseButtonDown(1) && currentDodgeLen_ == 0.0f)
        {
            dodgeForce_ = Global.Crosshair.GetDirectionVector(Transform.localPosition, normalize: true) * 50;
        }

        float halfSize = size_ * 0.33f;

        Vector3 moveVec = Vector3.zero;

        Vector3 inputVec = Vector3.zero;
        inputVec.x = Input.GetAxis("Horizontal");
        inputVec.y = Input.GetAxis("Vertical");

        var velocity = inputVec.normalized * td * 5;
        bool isRunning = velocity.sqrMagnitude > 0.0f;
        if (isRunning)
        {
            flipX_ = velocity.x < 0.0f;
            CollisionUtil.TempList.Clear();
            CollisionUtil.TempList.Add(Transform.localPosition + Vector3.left * halfSize);
            CollisionUtil.TempList.Add(Transform.localPosition + Vector3.right * halfSize);
            Vector2 shortestMove;
            bool couldMove = CollisionUtil.TryMovePoints(CollisionUtil.TempList, velocity, out shortestMove);
            moveVec = shortestMove;
        }

        if (dodgeForce_.sqrMagnitude >= 0.0f)
        {
            var dodgeVec = dodgeForce_ * td;
            float frameDodgeLen = dodgeVec.magnitude;
            currentDodgeLen_ += frameDodgeLen;
            if (currentDodgeLen_ >= DodgeLength)
            {
                // Reached end of dodge. Clamp to exact length.
                frameDodgeLen = currentDodgeLen_ - DodgeLength;
                dodgeVec = dodgeVec.normalized * frameDodgeLen;
                currentDodgeLen_ = 0.0f;
                dodgeForce_ = Vector3.zero;
            }
            moveVec += dodgeVec;
        }

        Transform.position += moveVec;

        Renderer.sprite = SimpleSpriteAnimator.GetAnimationSprite(isRunning ? Anim.Run : Anim.Idle, Anim.DefaultAnimationFramesPerSecond);
        Renderer.flipX = flipX_;

        float t = Global.TimeManager.SlowableTime * 4;
        PlayerLightTrans.localScale = new Vector2(8 + Mathf.Sin(t) * 0.25f, 4 + Mathf.Cos(t) * 0.25f);
    }
}
