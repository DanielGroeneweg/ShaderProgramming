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
        _ShadowStrength("Shadow Brightness", Range(0,1)) = 0.1
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
            float4 _Tint;
            float _FogStrength;
            float4 _FogColor;

            // Lighting
            float _ShadowStrength;

            v2f vert(appdata v)
            {
                v2f o;

                float heightScale = 3.0;
                float heightBase = 0.5 * heightScale;

                float4 color = tex2Dlod(_HeightMap, float4(v.uv, 0, 0));
                float4 modVertex = v.vertex;
                modVertex.y = color.r * heightScale - heightBase;
                o.vertex = UnityObjectToClipPos(modVertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4 worldPos = mul(unity_ObjectToWorld, modVertex);
                o.posInCamera = mul(UNITY_MATRIX_V, worldPos);

                // calculate normals
                float2 uv = v.uv;
                float stepX = (_UseStepSize == 1) ? _StepSize : _HeightMap_TexelSize.x;
                float stepY = (_UseStepSize == 1) ? _StepSize : _HeightMap_TexelSize.y;

                // Heights
                float hL = tex2Dlod(_HeightMap, float4(uv - float2(stepX, 0), 0, 0)).r * heightScale - heightBase;
                float hR = tex2Dlod(_HeightMap, float4(uv + float2(stepX, 0), 0, 0)).r * heightScale - heightBase;
                float hT = tex2Dlod(_HeightMap, float4(uv + float2(0, stepY), 0, 0)).r * heightScale - heightBase;
                float hB = tex2Dlod(_HeightMap, float4(uv - float2(0, stepY), 0, 0)).r * heightScale - heightBase;

                // Vertices
                float3 vL = float3(-stepX, hL, 0);
                float3 vR = float3(stepX, hR, 0);
                float3 vT = float3(0, hT, stepY);
                float3 vB = float3(0, hB, -stepY);

                // Cross for normal
                float3 horizontal = vL - vR;
                float3 vertical = vT - vB;
                float3 normal = cross(horizontal, vertical);

                // Transform to world space
                o.normal = mul(UNITY_MATRIX_M, float4(normalize(normal), 0));

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                col += _Tint;
                col *= _LightColor0;
                
                float diffuse = saturate(dot(i.normal, _WorldSpaceLightPos0));
                col = lerp (_Ambient + col * _ShadowStrength, col, diffuse);

                return col;
                //return float4(i.normal.xyz, 1);
            }
            ENDCG
        }
    }
}
