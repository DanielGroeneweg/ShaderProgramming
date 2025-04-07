Shader "CustomRenderTexture/MusicShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
        _UseMainTex("Use InputTex", Range(0,1)) = 0
    }

     SubShader
     {
        Blend One Zero

        Pass
        {
            Name "MusicShader"

            CGPROGRAM

            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            sampler2D   _MainTex;
            int _BarCount;
            float _Bars[64];
            int _UseMainTex;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float4 color = tex2D(_MainTex, uv) * _Color;

                float barsDisplayed = 64/1.5;

                if (uv.y > _Bars[(int)(uv.x * (barsDisplayed - 1))]) {
                    color = 0;
                    }
                
                // Red at top, green at bottom
                else if (_UseMainTex < 1) color = float4(uv.y, 1 - uv.y, 0, 1);

                return color;
            }
            ENDCG
        }
    }
}
