using UnityEngine;

public class AnimTest : MonoBehaviour
{
    public SpriteAnimationFrames_IdleRun Anim;

    void Update()
    {
        var spr = GetComponent<SpriteRenderer>().sprite = SimpleSpriteAnimator.GetAnimationSprite(Anim.Run, Anim.DefaultAnimationFramesPerSecond);
    }
}
