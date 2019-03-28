Shader "Engine/AdditiveVertexColor"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags
    {
        "Queue" = "Transparent"
        "PreviewType" = "Plane"
        "CanUseSpriteAtlas" = "True"
    }

        Pass
    {
        ZWrite Off
        Cull Off
        Blend SrcAlpha One

        CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON

#include "UnityCG.cginc"

        sampler2D _MainTex;

    struct Vertex
    {
        float4 vertex : POSITION;
        float2 uv_MainTex : TEXCOORD0;
        float4 color : COLOR;
    };

    struct Fragment
    {
        float4 vertex : POSITION;
        float2 uv_MainTex : TEXCOORD0;
        float4 color : COLOR;
    };

    Fragment vert(Vertex v)
    {
        Fragment o;

        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv_MainTex = v.uv_MainTex;
        o.color = v.color;
#ifdef PIXELSNAP_ON
        o.vertex = UnityPixelSnap(o.vertex);
#endif
        return o;
    }

    float4 frag(Fragment IN) : COLOR
    {
        half4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
		return c;
    }
        ENDCG
    }
    }
}