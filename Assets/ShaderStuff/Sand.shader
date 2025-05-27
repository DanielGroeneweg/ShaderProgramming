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
        _StepSize("Step Size", float) = 0.01
        _UseStepSize("Use Step Size", Range(0,1)) = 0
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
                float4 posInCamera : POSITIONT0;
                float4 normal : NORMAL;
            };

            // HeightMap
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _HeightMap;
            float4 _HeightMap_TexelSize;
            float _StepSize;
            float _UseStepSize;

            // Fog
            float4 _Ambient;
            float _Tint;
            float _FogStrength;
            float4 _FogColor;

            v2f vert(appdata v)
            {
                v2f o;

                float4 color = tex2Dlod(_HeightMap, float4(v.uv, 0, 0));
                float4 modVertex = v.vertex;
                modVertex.y += length(color) * 2 - 2.5;
                o.vertex = UnityObjectToClipPos(modVertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4x4 mvp = UNITY_MATRIX_MV;
                o.posInCamera = mul(mvp, o.vertex);

                o.normal = v.normal;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                /*
                float4 albedo = tex2D(_MainTex, i.uv);
                albedo += _Tint;

                float value = saturate(dot(i.normal, _WorldSpaceLightPos0));
                float4 light = _LightColor0 * value;

                float fogValue = clamp(length(i.posInCamera) * _FogStrength, 0, 100);
                float4 fog = _FogColor * fogValue;

                albedo += _Ambient;

                albedo += fog;
                
                return saturate(albedo);
                */

                // Horizontal
                

                // calculating normals
                // Horizontal
                float stepSize = _HeightMap_TexelSize.x;
                if (_UseStepSize == 1) stepSize = _StepSize;

                float4 _LeftColor = tex2D(_HeightMap, float2(i.uv.x - stepSize, i.uv.y));
                float4 _RightColor = tex2D(_HeightMap, float2(i.uv.x + stepSize, i.uv.y));

                float4 _HorizontalColor = (_RightColor - _LeftColor) * 25;

                // Vertical
                stepSize = _HeightMap_TexelSize.y;
                if (_UseStepSize == 1) stepSize = _StepSize;

                float4 _TopColor = tex2D(_HeightMap, float2(i.uv.x, i.uv.y + stepSize));
                float4 _BottomColor = tex2D(_HeightMap, float2(i.uv.x, i.uv.y - stepSize));

                float4 _VerticalColor = (_BottomColor - _TopColor) * 25;

                // Total color
                //float3 _Cross = cross(float3(_HorizontalColor.xyz), float3(_VerticalColor.xyz));

                float4 _Color = float4(0.15, _VerticalColor.y, 0, 1) + float4(0.15, _HorizontalColor.y, 0, 1);

                return _Color;
            }
            ENDCG
        }
    }
}
