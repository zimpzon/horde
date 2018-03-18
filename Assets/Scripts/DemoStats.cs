using HordeEngine;
using TMPro;
using UnityEngine;

public class DemoStats : MonoBehaviour
{
    public Camera LightingCamera;
    public TextMeshProUGUI TextSpritesRendered;
    public TextMeshProUGUI TextFps;

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
            TextSpritesRendered.text = newSprCount.ToString();
            lastSprCount_ = newSprCount;
        }

        if (Time.frameCount % 5 == 0)
        {
            TextFps.text = string.Format("{0:0.0} fps", 1.0f / Time.deltaTime);
        }
    }
}
