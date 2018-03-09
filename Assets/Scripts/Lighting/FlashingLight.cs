using HordeEngine;
using UnityEngine;

[RequireComponent(typeof(MaterialColorSetter))]
public class FlashingLight : MonoBehaviour
{
    public float ScaleVariance = 0.1f;
    public Color Color0 = Color.red;
    public Color Color1 = Color.yellow;

    Transform trans_;
    MaterialColorSetter colorSetter_;
    Vector2 baseScale_;
    float h0_, s0_, v0_;
    float h1_, s1_, v1_;
    float nextFlash_;

    void Awake()
    {
        trans_ = transform;
        baseScale_ = trans_.localScale;

        colorSetter_ = GetComponent<MaterialColorSetter>();
        Color.RGBToHSV(Color0, out h0_, out s0_, out v0_);
        Color.RGBToHSV(Color1, out h1_, out s1_, out v1_);
    }

    void Update()
    {
        if (Global.TimeManager.SlowableTime > nextFlash_)
        {
            trans_.localScale = baseScale_ + Vector2.one * (Random.value - 0.5f) * ScaleVariance;

            float h = Random.value * (h1_ - h0_) + h0_;
            float s = Random.value * (s1_ - s0_) + s0_;
            float v = Random.value * (v1_ - v0_) + v0_;
            colorSetter_.Color = Color.HSVToRGB(h, s, v);

            nextFlash_ = Global.TimeManager.SlowableTime + Random.value * 0.1f + 0.1f;
        }
    }
}
