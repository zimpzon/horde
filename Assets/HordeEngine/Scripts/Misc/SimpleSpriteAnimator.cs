﻿using HordeEngine;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleSpriteAnimator : MonoBehaviour
{
    public Sprite[] AnimationSprites;
    public float AnimationFramesPerSecond = 5.0f;

    SpriteRenderer renderer_;

    private void Awake()
    {
        renderer_ = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        renderer_.sprite = GetAnimationSprite(AnimationSprites, AnimationFramesPerSecond);
    }

    public static Sprite GetAnimationSprite(Sprite[] sprites, float animationFramesPerSecond)
    {
        int id = (int)(Time.fixedUnscaledTime * animationFramesPerSecond) % sprites.Length;
        return sprites[id];
    }

    public static Rect GetAnimationUvRect(Sprite[] sprites, float animationFramesPerSecond)
    {
        Sprite spr = GetAnimationSprite(sprites, animationFramesPerSecond);
        return spr.textureRect;
    }
}
