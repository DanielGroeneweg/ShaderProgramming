Shader "Unlit/Sand"
{
Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _HeightMap("HeightMap", 2D) = "black" {}
        _Ambient("Ambient Color", Color) = (1,1,1,1)
        _Tint("Tint", Color) = (1,1,1,1)
        _FogColor("Fog Color", Color) = (1,1,1,1)
        _FogStrength("Fog Strength", float) = 1
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
                float4 posInCamera : POSITIONT;
            };

            sampler2D _MainTex;
            sampler2D _HeightMap;
            float4 _Ambient;
            float _Tint;
            float _FogStrength;
            float4 _FogColor;

            v2f vert(appdata v)
            {
                v2f o;

                float4 color = tex2Dlod(_HeightMap, float4(v.uv, 0, 0));
                float4 modVertex = (v.vertex + float4(0, -10 + length(color) * 10, 0, 0));
                o.vertex = UnityObjectToClipPos(modVertex);

                o.uv = v.uv;

                float4x4 mvp = UNITY_MATRIX_MV;
                o.posInCamera = mul(mvp, v.normal);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 albedo = tex2D(_MainTex, i.uv);
                albedo += _Tint;

                float fogValue = length(i.posInCamera) * _FogStrength;
                float4 fog = _FogColor * fogValue;


                albedo += _Ambient;

                albedo += fog;
                
                return albedo;
            }
            ENDCG
        }
    }
}
