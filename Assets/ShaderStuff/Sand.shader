// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 posInCamera : POSITIONT0;
                float3 normal : NORMAL;
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

            v2f vert(appdata v)
            {
                v2f o;

                float heightScale = 2.0;
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

                // Construct positions purely in UV-based space
                float3 pL = float3(uv.x - stepX, hL, uv.y);
                float3 pR = float3(uv.x + stepX, hR, uv.y);
                float3 pT = float3(uv.x, hT, uv.y + stepY);
                float3 pB = float3(uv.x, hB, uv.y - stepY);

                // Cross for normal
                float3 horizontal = pL - pR;
                float3 vertical = pB - pT;
                float3 normal = normalize(cross(horizontal, vertical));

                // If needed, transform to world space
                o.normal = mul((float3x3)unity_ObjectToWorld, normal);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 albedo = tex2D(_MainTex, i.uv);
                albedo += _Tint;

                float value = saturate(dot(normalize(i.normal), _WorldSpaceLightPos0.xyz));
                float4 light = _LightColor0 * value;
                albedo *= light;

                float fogValue = saturate(_FogStrength * length(i.posInCamera));
                float4 fog = _FogColor * fogValue;

                albedo += _Ambient;

                albedo += fog;
                
                return saturate(albedo);

                //return float4(i.normal * 0.5 + 0.5, 1);
            }
            ENDCG
        }
    }
}
