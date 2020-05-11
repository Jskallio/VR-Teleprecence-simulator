// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Jucci/DottedLine"
{
	Properties
	{
		_SpriteTex("Base (RGB)", 2D) = "white" {}
		_Size("Size", Range(0, 3)) = 0.5
	}

		SubShader
		{

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			GrabPass
			{
				"_BackgroundTexture"
			}

			Pass
			{
				Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
				LOD 200

				CGPROGRAM
					#pragma target 5.0
					#pragma vertex VS_Main
					#pragma fragment FS_Main
					#pragma geometry GS_Main
					#include "UnityCG.cginc" 

			struct GSI
			{
				float4	pos		: SV_POSITION;
				float3	normal	: NORMAL;
				float2  tex0	: TEXCOORD0;
			};

			struct FSI
			{
				float4	pos		: SV_POSITION;
				float2  tex0	: TEXCOORD0;
				float4  grabPos : TEXCOORD1;
			};



			float _Size;
			float4x4 _VP;
			Texture2D _SpriteTex;
			SamplerState sampler_SpriteTex;


			// Vertex Shader
			GSI VS_Main(appdata_base v)
			{
				GSI output = (GSI)0;

				output.pos = v.vertex;
				output.normal = v.normal;
				output.tex0 = float2(0, 0);

				return output;
			}

			float4 RotateAroundYInDegrees(float4 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float4(mul(m, vertex.xz), vertex.yw).xzyw;
			}


			// Geometry Shader
			[maxvertexcount(4)]
			void GS_Main(point GSI p[1], inout TriangleStream<FSI> triStream)
			{
				float3 up = p[0].normal;
				float3 right = RotateAroundYInDegrees(float4(up,1),90).xyz;

				float halfS = 0.5f * _Size;

				float4 v[4];
				v[0] = UnityObjectToClipPos(float4(p[0].pos + halfS * right - halfS * up, 1.0f));
				v[1] = UnityObjectToClipPos(float4(p[0].pos + halfS * right + halfS * up, 1.0f));
				v[2] = UnityObjectToClipPos(float4(p[0].pos - halfS * right - halfS * up, 1.0f));
				v[3] = UnityObjectToClipPos(float4(p[0].pos - halfS * right + halfS * up, 1.0f));

				FSI pIn;
				pIn.pos = v[0];
				pIn.tex0 = float2(1.0f, 0.0f);
				pIn.grabPos = ComputeGrabScreenPos(v[0]);
				triStream.Append(pIn);

				pIn.pos = (v[1]);
				pIn.tex0 = float2(1.0f, 1.0f);
				pIn.grabPos = ComputeGrabScreenPos(v[1]);
				triStream.Append(pIn);

				pIn.pos = (v[2]);
				pIn.tex0 = float2(0.0f, 0.0f);
				pIn.grabPos = ComputeGrabScreenPos(v[2]);
				triStream.Append(pIn);

				pIn.pos = (v[3]);
				pIn.tex0 = float2(0.0f, 1.0f);
				pIn.grabPos = ComputeGrabScreenPos(v[3]);
				triStream.Append(pIn);
			}

			sampler2D _BackgroundTexture;

			// Fragment Shader
			float4 FS_Main(FSI input) : COLOR
			{
				half4 bgcolor = tex2Dproj(_BackgroundTexture, input.grabPos);
				bgcolor = 1 - bgcolor;
				bgcolor.a = _SpriteTex.Sample(sampler_SpriteTex, input.tex0).a;
				return bgcolor;
			}

		ENDCG
	}
		}
}
