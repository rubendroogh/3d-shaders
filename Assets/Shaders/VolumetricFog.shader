// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skybox/SkyboxMart"
{
	// Properites hold variables
	// $Variablename$('$name in editor$', $Variable type$) = $Standard value$
	Properties
	{
		_SkyColorTop("Top Color", Color) = (0.37, 0.52, 0.73, 0)
		_SkyColorTopModifier("Top Modifier", Float) = 8.5
		_SkyColorBottom("Bottom Color", Color) = (0.37, 0.52, 0.73, 0)
		_SkyColorBottomModifier("Bottom Modifier", Float) = 3.0
		_SkyColorHorizon("Horizon Color", Color) = (0.37, 0.52, 0.73, 0)
		_SunColor("sun color", color) = (248, 243, 158, 0)
		_SunIntensity("Sun intensity", float) = 30
		_SunBlend("Sun Glow", float) = 1
		_SunRadius("Sun Radius", float) = 30
		_SunRiseAngle("Point of Sun rise", float) = 0
		_SunAngle("Current Sun Angle", float) = 0
		_SunVector("SunVector (DO NOT MANIPULATE)", vector) = (0,0,0)
		_SunGlowColor("sun glow color", color) = (248, 243, 158, 0)
		_SunGlowIntensity("Sun glow intensity", float) = 10
		_AltitudeGlowModifier("Sun Altitude Glow supressor", float) = 2
	}
	

	SubShader
	{
		// Is the stuff that gets run
		Pass
		{
			// starts the code that is run
			CGPROGRAM

			// #pragma is like a function 
			// there are two types vertex and fragement
			// vertex finds out what the object is to manipulate
			// fragment Gives the specific pixels color.
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc
			
			// includes a standard library from nvidia
			#include "UnityCG.cginc"
			
			struct appdata {
				
				// position of the vertex we are working on
				float4 vertex : POSITION;

				// the coordinate where to wrap the texture
				float3 uv : TEXCOORD0; 
			};

			// vertex to fragment
			struct v2f {
				float4 position : SV_POSITION;
		        float3 uv		: TEXCOORD0;
			};

			// variable declaring
			
			// function that takes in appdata and puts out v2f
			v2f vertexFunc(appdata IN)
			{
				v2f OUT;
				//takes object position from unity and gives it to the v2f
				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				return OUT;
			}

			// SV_Target gives it the target to render on
			float4 fragmentFunc(v2f IN) : SV_TARGET
			{

				return float4(1, 1, 1, 0.5f);
			}

			// ends the code that is run
			ENDCG
		}
	}
}