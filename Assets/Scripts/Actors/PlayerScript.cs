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
}
