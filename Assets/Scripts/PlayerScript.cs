using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float Speed = 10;
    Transform transform_;
    Rigidbody2D body_;

    void Awake()
    {
        transform_ = transform;
        body_ = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float dt = Time.fixedUnscaledDeltaTime;
        var horz = Input.GetAxisRaw("Horizontal");
        var vert = Input.GetAxisRaw("Vertical");
        Vector3 vec = new Vector3(horz, vert, 0) * Speed * dt;
        body_.MovePosition(transform_.position + vec);
    }
}
