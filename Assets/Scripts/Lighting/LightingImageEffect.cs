using System;
using UnityEngine;

namespace HordeEngine
{
    public class LightingImageEffect : MonoBehaviour
    {
        public Material EffectMaterial;

        [NonSerialized]public RenderTexture LightingTexture;

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            EffectMaterial.SetTexture("_LightingTex", LightingTexture);
            Graphics.Blit(source, destination, EffectMaterial);
        }
    }
}
