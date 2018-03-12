// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Engine/WallShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _HeightTex("Height Map", 2D) = "black" {}
        [MaterialToggle] PixelSnap("Pixel snap", float) = 1
    }

        SubShader
        {
            Tags
            {
                "RenderType" = "Opaque"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile DUMMY PIXELSNAP_ON

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 worldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                sampler2D _HeightTex;
                float4 _MainTex_ST;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    #ifdef PIXELSNAP_ON
                    o.vertex = UnityPixelSnap(o.vertex);
    #endif
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed height = tex2D(_HeightTex, i.uv);
                    if (col.a < 0.5) discard;
                    fixed a = clamp(-i.worldPos.z + height, 0, 1.0);
                    col.a = a * a * 0.3;
                    return col;
                }
                ENDCG
            }
        }
}
