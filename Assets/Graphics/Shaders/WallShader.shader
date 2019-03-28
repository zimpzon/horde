// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Engine/WallShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _LightContribTex("Light Contribution Map", 2D) = "black" {}
    }

        SubShader
        {
            Tags
            {
                "RenderType" = "Opaque"
                "PreviewType" = "Plane"
            }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

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
//                    float3 worldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                sampler2D _LightContribTex;
                float4 _MainTex_ST;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.vertex = UnityPixelSnap(o.vertex);
//                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed lightContribution = tex2D(_LightContribTex, i.uv);
                    if (col.a < 0.5) discard;
                    col.a = lightContribution;
                    return col;
                }
                ENDCG
            }
        }
}
