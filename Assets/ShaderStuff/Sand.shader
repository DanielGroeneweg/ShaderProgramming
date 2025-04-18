Shader "Unlit/Sand"
{
Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _HeightMap("HeightMap", 2D) = "black" {}
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
            };

            sampler2D _MainTex;
            sampler2D _HeightMap;

            v2f vert(appdata v)
            {
                v2f o;

                float4 color = tex2Dlod(_HeightMap, float4(v.uv, 0, 0));

                float4 modVertex = (v.vertex + float4(0, -20 + length(color) * 20, 0, 0));

                o.vertex = UnityObjectToClipPos(modVertex);
                // manual tiling for now:
                o.uv = v.uv * 4;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
