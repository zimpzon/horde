﻿Shader "Engine/MapShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        [MaterialToggle] PixelSnap("Pixel snap", float) = 1
    }

	SubShader
	{
		Tags
        {
            "RenderType"="Opaque"
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
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
            // Can uv2 be used to fake ambient occlusion?
            // Might have to do a lookup in a 1D shade texture and multiply?
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
#ifdef PIXELSNAP_ON
                o.vertex = UnityPixelSnap(o.vertex);
#endif
                return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
                if (col.a < 0.5) discard; // Fixed cut-off at 0.5
				col.a = 0.0f;
                return col;
			}
			ENDCG
		}
	}
}
