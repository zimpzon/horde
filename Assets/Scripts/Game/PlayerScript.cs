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

    float size_ = 1.0f;

    void Awake()
    {
        trans_ = transform;
    }

    void Update()
    {
        if (CollisionUtil.CollisionMap == null)
            return;

        if (Input.GetKeyDown(KeyCode.R))
            Global.SceneAccess.CameraShake.AddTrauma(1.0f);

        float halfSize = size_ * 0.33f;

        Vector3 moveVec = Vector3.zero;
        moveVec.x = Input.GetAxis("Horizontal");
        moveVec.y = Input.GetAxis("Vertical");
        var velocity = moveVec * Time.deltaTime * 6;
        if (velocity.sqrMagnitude > 0.0f)
        {
            var pos = trans_.position;
            CollisionUtil.PointsToMove.Clear();
            CollisionUtil.PointsToMove.Add(trans_.position + Vector3.left * halfSize);
            CollisionUtil.PointsToMove.Add(trans_.position + Vector3.right * halfSize);
            Vector3 shortestMove;
            bool couldMove = CollisionUtil.TryMovePoints(CollisionUtil.PointsToMove, velocity, out shortestMove);
            trans_.position += shortestMove;
            Global.GameManager.ShowDebug("CouldMove", couldMove.ToString());
            Global.GameManager.ShowDebug("PlayerPos", trans_.position.ToString());
        }

        Global.SceneAccess.CameraTarget.SetTarget(trans_.position);
    }
}
