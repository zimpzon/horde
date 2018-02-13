Shader "Engine/AdditiveTint"
{
    Properties
    {
        [PerRendererData]_Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = ""
    }

    SubShader
    {
        Tags { Queue = Transparent }
        Blend SrcAlpha One
        ZWrite Off
        Pass
        {
            SetTexture[_MainTex]
            {
                constantColor[_Color]
                Combine texture * constant
            }
        }
    }
}
