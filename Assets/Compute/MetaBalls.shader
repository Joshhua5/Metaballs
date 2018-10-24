// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/MetaBalls"
{
	Properties
	{ 
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half4 vertex : SV_POSITION; 
				half2 uv : TEXCOORD0;
				half2 pixelWS : TEXCOORD1; 
			}; 

			uniform float4 _Position;
			uniform float _Aspect;
			uniform float _Height;
			uniform float _Cutoff;
			sampler2D _MainTex; 
			 
			uniform float4 _BallPosition[64]; // x, y, radius2
			uniform int _BallCount;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pixelWS = (mul(unity_ObjectToWorld, v.vertex).xy);
				//o.pixelWS = v.vertex.xy;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				// o.vertex = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			 
			fixed4 frag (v2f i) : SV_Target
			{   
				// Pixel Position  
				half2 wh = half2(_Aspect * _Height, _Height);
				half2 pixel = (i.pixelWS * wh * 2.0) + (_Position - wh);
				  
				// For each point
				half intensity = 0;
				for(int j = 0 ; j < _BallCount; ++j) {   
					// Add the distances together
					intensity += _BallPosition[j].z / distance(pixel, _BallPosition[j].xy);
				} 

				if(intensity < _Cutoff)
					return tex2D(_MainTex, i.uv); 
				return half4(intensity, intensity, intensity, 1); 
			}
			ENDCG
		}
	}
}
