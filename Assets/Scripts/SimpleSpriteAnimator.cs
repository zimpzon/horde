using HordeEngine;
using UnityEngine;

public static class SimpleSpriteAnimator
{
    public static Sprite GetAnimationSprite(Sprite[] sprites, float animationFramesPerSecond)
    {
        int id = (int)(Global.TimeManager.SlowableTime * animationFramesPerSecond) % sprites.Length;
        return sprites[id];
    }

    public static Rect GetAnimationUvRect(Sprite[] sprites, float animationFramesPerSecond)
    {
        Sprite spr = GetAnimationSprite(sprites, animationFramesPerSecond);
        return spr.textureRect;
    }
}
