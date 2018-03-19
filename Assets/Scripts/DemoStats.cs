using HordeEngine;
using TMPro;
using UnityEngine;

public class DemoStats : MonoBehaviour
{
    public Camera LightingCamera;
    public TextMeshProUGUI TextSpritesRendered;

    public void AmbientLightSliderChanged(float value)
    {
        LightingCamera.backgroundColor = Color.HSVToRGB(1.0f, 0.0f, value);
    }

    private void Start()
    {
        LightingCamera.backgroundColor = Color.HSVToRGB(1.0f, 0.0f, 0.8f);
    }

    int lastSprCount_;
    private void Update()
    {
        int newSprCount = Horde.Sprites.SpritesRendered;
        if (newSprCount != lastSprCount_)
        {
            TextSpritesRendered.SetText("{0}", newSprCount);
            lastSprCount_ = newSprCount;
        }
    }
}
