﻿
Shader "Jucci/LineShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                return i.color;
            }
            ENDCG
        }
    }
}
