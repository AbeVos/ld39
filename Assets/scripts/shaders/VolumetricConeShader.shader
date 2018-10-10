// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/VolumetricConeShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Center ("Center", Vector) = (0,0,0) 
		_Radius ("Radius", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#define STEPS 256
			#define STEP_SIZE 0.01

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldPosition : TEXTCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float3 _Center;
			float _Radius;

			bool sphereHit(float3 position)
			{
				return distance(position, _Center) < _Radius;
			}

			bool raymarch(float3 position, float3 direction)
			{
				for (int i = 0; i < STEPS; i++)
				{
					if (sphereHit(position))
					{
						return fixed4(1,0,0,1);
					}

					position += direction * STEP_SIZE;
				}

				return fixed4(0,0,0,0);
			}
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 worldPosition = i.worldPosition;
				float3 viewDirection = normalize(i.worldPosition - _WorldSpaceCameraPos);
				return raymarch(i.worldPosition, viewDirection);
			}
			ENDCG
		}
	}
}
