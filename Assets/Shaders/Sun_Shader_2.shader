Shader "Unlit/Sun_Shader_2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		iChannel0("iChannel0", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			sampler2D iChannel0;
			float4 iChannel0_ST;


			// based on this https://www.shadertoy.com/view/MtXSzS port
			// iq's noise from here https://www.shadertoy.com/view/XslGRr
			// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0

			#define saturate(oo) clamp(oo, 0.0, 1.0)

						// Quality Settings
			#define MarchSteps 6
						// Scene Settings
			#define ExpPosition float3(0.0, 0.0, -500.0)
			#define Radius 350.8010f
			#define Background float4(0.1, 0.0, 0.0, 1.0)
						// Noise Settings
			#define NoiseSteps 4
			#define NoiseAmplitude 0.06
			#define NoiseFrequency 8.0
			#define Animation float3(0.0, -3.0, 0.5)
						// Colour Gradient
			#define Color1 float4(1.0, 1.0, 1.0, 1.0)
			#define Color2 float4(1.0, 0.8, 0.2, 1.0)
			#define Color3 float4(1.0, 0.03, 0.0, 1.0)
			#define Color4 float4(0.5, 0.2, 0.2, 1.0)


		float noise(in float3 x)
		{
		/*	float3 p = floor(x);
			float3 f = frac(x);
			f = f*f*(3.0 - 2.0*f);
			float2 uv = (p.xy + float2(37.0,17.0)*p.z) + f.xy;
			float2 rg = tex2D(iChannel0, (uv + 0.5) / 256.0).yx;
			return -1.0 + 1.7*lerp(rg.x, rg.y, f.z);
*/
			return frac(sin(dot(x.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
		}

	float Turbulence(float3 position, float minFreq, float maxFreq, float qWidth)
	{
		float value = 0.0;
		float cutoff = clamp(0.5 / qWidth, 0.0, maxFreq);
		float fade;
		float fOut = minFreq;
		int i = NoiseSteps;
		while (i >= 0)
		{
			if (fOut >= 0.5 * cutoff) {
				i = 0;
			}
			else {
				fOut *= 2.0;
				value += abs(noise(position * fOut)) / fOut;
			}
			i--;
		}
		fade = clamp(2.0 * (cutoff - fOut) / cutoff, 0.0, 1.0);
		value += fade * abs(noise(position * fOut)) / fOut;
		return 1.0 - value;
	}

	float SphereDist(float3 position)
	{
		return length(position - ExpPosition) - Radius;
	}

	float4 Shade(float distance)
	{
		float c1 = saturate(distance*5.0 + 0.5);
		float c2 = saturate(distance*5.0);
		float c3 = saturate(distance*3.4 - 0.5);
		float4 a = lerp(Color1,Color2, c1);
		float4 b = lerp(a,     Color3, c2);
		return   lerp(b,     Color4, c3);
	}

	// Draws the scene
	float RenderScene(float3 position, out float distance)
	{
		float noise = Turbulence(position * NoiseFrequency + Animation*_Time.y*0.24, 0.1, 1.5, 0.03) * NoiseAmplitude;
		noise = saturate(abs(noise));
		distance = SphereDist(position) - noise;
		return noise;
	}

	// Basic ray marching method.
	float4 March(float3 rayOrigin, float3 rayStep)
	{
		float3 position = rayOrigin;
		float distance;
		float displacement;
		int step = 0; 
		
		while (step < MarchSteps)
		{
			displacement = RenderScene(position, distance);

			if (distance < 0.5) {
				step = MarchSteps;
			}
			else {
				position += rayStep * distance;
			}
			step++;
		}


		return lerp(Shade(displacement), Background, float(distance >= 0.5));
	}

	bool IntersectSphere(float3 ro, float3 rd, float3 pos, float radius, out float3 intersectPoint)
	{
		float3 relDistance = (ro - pos);
		float b = dot(relDistance, rd);
		float c = dot(relDistance, relDistance) - radius*radius;
		float d = b*b - c;
		intersectPoint = ro + rd*(-b - sqrt(d));
		return d >= 0.0;
	}

	//void mainImage(out float4 fragColor, in float2 fragCoord)
	float4 mainImage(float2 fragCoord : SV_POSITION) : SV_Target
	{
		float4 fragColor;
		
		float2 p = 0.5; //(gl_FragCoord.xy / _ScreenParams.xy) * 2.0 - 1.0;
		p.x *= _ScreenParams.x / _ScreenParams.y;
		float rotx = 0;
		float roty = _Time.y*0.001;
		float zoom = 5.0;
		// camera
		float3 ro = zoom * normalize(float3(cos(roty), cos(rotx), sin(roty)));
		float3 ww = normalize(float3(0.0, 0.0, 0.0) - ro);
		float3 uu = normalize(cross(float3(0.0, 1.0, 0.0), ww));
		float3 vv = normalize(cross(ww, uu));
		float3 rd = normalize(p.x*uu + p.y*vv + 1.5*ww);
		float4 col = Background;
		float3 origin;

		if (IntersectSphere(ro, rd, ExpPosition, Radius + NoiseAmplitude*14.0, origin))
		{
			col = March(origin, rd);
		}

		fragColor = col;

		return fragColor;
	}



			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _i;
			float4 _MainTex_ST;



			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, iChannel0);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//// sample the texture
				//fixed4 col = tex2D(iChannel0, i.uv);
				//// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//
				//return col;
				//
				return mainImage(i.uv);
			}
			ENDCG
		}
	}
}
