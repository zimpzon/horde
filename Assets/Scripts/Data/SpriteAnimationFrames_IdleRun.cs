using UnityEngine;

[CreateAssetMenu(fileName = "SpriteAnimationFrames_IdleRun.asset", menuName = "HordeEngine/Sprite Frames, IdleRun")]
public class SpriteAnimationFrames_IdleRun : ScriptableObject
{
    public float DefaultAnimationFramesPerSecond;
    public Sprite[] Idle;
    public Sprite[] Run;
}
