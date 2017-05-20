﻿Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		[NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
		[NoScaleOffset]_Bump("load in texture",2D) = "white"{}
		_range("health",Range(0, 1)) = 0
	}
	SubShader
	{ 
			Tags{ "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha

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
			};

			sampler2D _MainTex,_Bump;
			float4 _MainTex_ST;
			float _range;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float4 col = tex2D(_MainTex, i.uv);
				float val = tex2D(_Bump, i.uv).r;
				col.a *= step(_range, val);
				// apply fog
				return col;
			}
			ENDCG
		}
	}
}
