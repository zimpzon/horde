using HordeEngine;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Make user answer questions so he learns something? Not blocking.
    // Can I do something with OpenStreetMap data?
    // really easy lights: vertex colors
    // Use props to place stufff? Lights (not just torches), traps, whatever?
    // KISS: Actors are game objects. Optimize LATER!
    // ActorScript (dumb?)
    // ActorController

    Transform trans_;

    void Awake()
    {
        trans_ = transform;
    }

    void Update()
    {
        Vector3 moveVec = Vector3.zero;
        moveVec.x = Input.GetAxis("Horizontal");
        moveVec.y = Input.GetAxis("Vertical");
        trans_.position += moveVec * Time.deltaTime * 4;
        Global.GameManager.ShowDebug("PlayerPos", trans_.position.ToString());

        if (CollisionUtil.CollisionMap != null)
            CollisionUtil.GetCollisionValue(trans_.position);
    }
}
