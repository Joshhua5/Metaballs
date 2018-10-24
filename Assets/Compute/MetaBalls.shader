// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Effect/MetaBalls"
{
	Properties
	{ 
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
				half4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half4 vertex : SV_POSITION; 
				half2 pixelWS : TEXCOORD0; 
			}; 

			uniform float4 _Position;
			uniform float _Aspect;
			uniform float _Height;
			 
			uniform float4 _BallPosition[64]; // x, y, radius2
			uniform int _BallCount;
			
			half2 compress(half2 value){
				return (value / 2.0) + half2(0.5, 0.5);	
			}

			half2 extract(half2 value){
				return (value - half2(0.5, 0.5)) * 2;
			}


			v2f vert (appdata v)
			{
				v2f o;
				o.pixelWS = (mul(unity_ObjectToWorld, v.vertex).xy);
				//o.pixelWS = v.vertex.xy;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// o.vertex = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			 

			fixed4 frag (v2f i) : SV_Target
			{  
				// half2 pixel = (i.pixelWS - half2(0.5, 0.5)) * 2;
				half2 pixel = (i.pixelWS); 
				  
				// Pixel Position 
              
				half2 wh = half2(_Aspect * _Height, _Height);
				pixel = (pixel * wh * 2.0) + (_Position - wh);
				 
				// Circle Position 

				// Calculate the distance squared
				 
				// For each collider
				half intensity = 0;
				for(int i = 0 ; i < _BallCount; ++i){ 
					// Get the distance between collider centre and ws pixel
					// var xComp = (i.pixelWS.x - cX) * (pX - cX);  
					// var yComp = (i.pixelWS.y - cY) * (pY - cY);
				  
					half2 distance = pow((pixel - _BallPosition[i].xy), 2); 
					
					// Blend colour accordingly
					intensity += _BallPosition[i].z / abs(distance.x + distance.y); 
					
				} 

				if(intensity >= 1)  
					return half4(intensity, intensity, intensity, 1);
				return half4( 0, 0, 0, 1);
			}
			ENDCG
		}
	}
}
