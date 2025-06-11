// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Vertex_Color_Unlit"
{
	Properties
	{
	 	_Color ("Main Color", Color) = (1,1,1,1)
		 _FogColor("Fog Color", Color) = (1,1,1,1)
        _FogStrength("Fog Strength", float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            
            #include "UnityCG.cginc"

			struct appdata
			{
				 float4 vertex : POSITION;
            	 fixed4 color : COLOR;
			};

			struct v2f
			{
				  float4 vertex : SV_POSITION;
            	  fixed4 color : COLOR;
				  float4 posInCamera : POSITIONT;
			};
		
			fixed4 _Color;
			float4 _FogColor;
			float _FogStrength;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);				
	            o.color = v.color * _Color;
				o.posInCamera = mul(UNITY_MATRIX_MV, float4(v.vertex.xyz, 1));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 color = i.color;

				float fogValue = _FogStrength * length(i.posInCamera);
                float4 fog = _FogColor * fogValue;

				color += fog;

				color.r = min(color.r, _FogColor.r);
                color.g = min(color.g, _FogColor.g);
                color.b = min(color.b, _FogColor.b);

				return color;
			}
			
			ENDCG
		}
	}
}
