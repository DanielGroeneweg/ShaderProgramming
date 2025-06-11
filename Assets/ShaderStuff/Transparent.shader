Shader "Unlit/Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _FogColor("Fog Color", Color) = (1,1,1,1)
        _FogStrength("Fog Strength", float) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back 
        LOD 1

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 posInCamera : POSITIONT0;
                float4 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseColor;

            float4 _Ambient;
            float _Tint;
            float _FogStrength;
            float4 _FogColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4x4 mvp = UNITY_MATRIX_MV;
                o.posInCamera = mul(mvp, float4(v.vertex.xyz, 0));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 _Color = _BaseColor;

                _Color.xyz += _Tint;

                float fogValue = saturate(_FogStrength * length(i.posInCamera));
                float4 fog = _FogColor * fogValue;

                _Color.xyz += _Ambient.xyz;

                _Color.xyz += fog.xyz;

                return _Color;
            }
            ENDCG
        }
    }
}
