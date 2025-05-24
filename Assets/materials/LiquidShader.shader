Shader "Unlit/LiquidShader"
{
    Properties
    {
        _Tint("Tint", Color) = (1,1,1,1)
        _FillLevel("Fill Level", Range(0,1)) = 0.3
        _MeshMinY("Mesh Min Y", float) = 0
        _MeshMaxY("Mesh Max Y", float) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull front 
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
                float4 pos : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION0;
                float4 normal : NORMAL0;
                float3 pos : NORMAL1;
            };

            float4 _Tint;
            float _FillLevel;
            float _MeshMinY;
            float _MeshMaxY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = normalize(mul(UNITY_MATRIX_M, float4(v.normal.xyz, 0)));
                o.pos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1));
                return o;
                //
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 _Color = _Tint;

                float normalizedY = (i.pos.y - _MeshMinY) / (_MeshMaxY - _MeshMinY);
                float alpha = _FillLevel - normalizedY;
                if (alpha <= 0) _Color.a = 0;

                return _Color;
            }
            ENDCG
        }
    }
}