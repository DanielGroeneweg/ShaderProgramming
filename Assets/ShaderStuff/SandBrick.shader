// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/SandBrick"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Ambient("Ambient Color", Color) = (1,1,1,1)
        _Tint("Tint", Color) = (1,1,1,1)
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
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 normal : NORMAL;
                float4 posInCamera : POSITIONT;
            };

            sampler2D _MainTex;
            float4 _Ambient;
            float _Tint;
            float _FogStrength;
            float4 _FogColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = normalize(mul(UNITY_MATRIX_M, float4(v.normal.xyz, 0)));
                float4x4 mvp = UNITY_MATRIX_MV;
                o.posInCamera = mul(mvp, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 albedo = tex2D(_MainTex, i.uv);
                albedo += _Tint;

                float value = saturate(dot(i.normal, _WorldSpaceLightPos0));
                float4 light = _LightColor0 * value;

                float fogValue = length(i.posInCamera) * _FogStrength;
                float4 fog = _FogColor * fogValue;


                albedo *= (_Ambient + light);

                albedo += fog;
                
                return albedo;
            }
            ENDCG
        }
    }
}
