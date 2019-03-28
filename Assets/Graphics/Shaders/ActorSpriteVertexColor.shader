Shader "Engine/ActorSpriteVertexColor"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        [MaterialToggle] PixelSnap("Pixel snap", float) = 1
        _Clarity("Clarity", float) = 1.0
    }
        SubShader
    {
        Tags
    {
        "RenderType" = "Opaque"
        "Queue" = "Transparent+1"
        "PreviewType" = "Plane"
        "CanUseSpriteAtlas" = "True"
    }

        Pass
    {
        Cull Off
        Blend One Zero

        CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON

#include "UnityCG.cginc"

        sampler2D _MainTex;
        float _Clarity;

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
        if (c.a < 0.5) discard; // Fixed cut-off at 0.5
        c.a = _Clarity;
		return c;
    }
        ENDCG
    }
    }
}