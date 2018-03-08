Shader "Engine/CustomSprite"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Brightness("Brightness", float) = 1.0
    }
        SubShader
    {
        Tags
    {
        "RenderType" = "Opaque"
        "Queue" = "Transparent+1"
    }

        Pass
    {
        ZWrite Off
        Blend One Zero

        CGPROGRAM
#pragma vertex vert
#pragma fragment frag

    sampler2D _MainTex;
    float _Brightness;

    struct Vertex
    {
        float4 vertex : POSITION;
        float2 uv_MainTex : TEXCOORD0;
        float2 uv2 : TEXCOORD1;
    };

    struct Fragment
    {
        float4 vertex : POSITION;
        float2 uv_MainTex : TEXCOORD0;
        float2 uv2 : TEXCOORD1;
    };

    Fragment vert(Vertex v)
    {
        Fragment o;

        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv_MainTex = v.uv_MainTex;
        o.uv2 = v.uv2;

        return o;
    }

    float4 frag(Fragment IN) : COLOR
    {
        half4 c = tex2D(_MainTex, IN.uv_MainTex);
		if (c.a < 0.5) discard; // Fixed cut-off at 0.5
		c.a = _Brightness;
		return c;
    }
        ENDCG
    }
    }
}