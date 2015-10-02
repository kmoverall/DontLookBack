// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/IllusionReveal" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_NoiseTex ("Noise Texture", 2D) = "white" {}
	_Radius ("Radius", Float) = 0.5
	_Feather ("Feather Radius", Float) = 0.25
	_Fade ("Noise Fade Amount", Float) = 1.0
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				float2 screenPos : TEXCOORD1;
				half2 noisecoord : TEXCOORD2;
				//UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			float _Radius;
			float _Feather;
			float _NoiseIntensity;
			float _Fade;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.noisecoord = TRANSFORM_TEX(v.texcoord, _NoiseTex);
				o.screenPos = o.vertex.xy / o.vertex.w;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				fixed4 noise = tex2D(_NoiseTex, i.noisecoord * 1.5);

				//Radii described using 1/2 of the screen height for units, creating a circle with radius relative the screen height
				float rad = pow(_Radius * _ScreenParams.y, 2);
				float featherrad = pow((_Feather + _Radius) * _ScreenParams.y, 2);

				//Coordinates of the pixel
				float transx = i.screenPos.x * _ScreenParams.x;
				float transy = i.screenPos.y * _ScreenParams.y;

				float dist = pow(transx, 2) + pow(transy, 2);

				//Set transparency and feather around edges
				if (dist < rad) {
					col.a = 1;
				}
				else if (dist < featherrad) {
					col.a = 1 - ((dist - rad) / (featherrad - rad));
					col.a = col.a * col.a * col.a * (col.a * (col.a * 6 - 15) + 10);
				}
				else {
					col.a = 0.0f;
				}
				
				float noiseMod = noise.r +_Fade;
				noiseMod = clamp(noiseMod, -1, 1);
				col.a = col.a * noiseMod; 

				return col;
			}
		ENDCG
	}
}

}
