Shader "Engine/LightingBlend"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "black" {}
        _LightingTex("Lighting Texture", 2D) = "black" {}
        _Multiplier("Multiplier", float) = 1.0
    }

        SubShader
    {

        Pass
    {
        CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

        struct appdata_t
    {
        float4 vertex   : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct v2f
    {
        float4 vertex   : SV_POSITION;
        half2 uv : TEXCOORD0;
    };

    v2f vert(appdata_t i)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(i.vertex);
        o.uv = i.uv;

        return o;
    }

    sampler2D _MainTex;
    sampler2D _LightingTex;
    float _Multiplier;

    half4 frag(v2f IN) : SV_Target
    {
        half4 base = tex2D(_MainTex, IN.uv);
        half4 lighting = tex2D(_LightingTex, IN.uv);
        half4 col = base * lighting * _Multiplier;

        // Could do greyscale here (0.2989, 0.5870, 0.1140.)
        //float g = col.r * 0.2989 + col.g * 0.5870 + col.b * 0.1140;
        //fixed4 grey = fixed4(g, g, g, 1.0);
        //return grey;
        return col;
    }

        ENDCG
    }
    }
}
