Shader "Custom/URP/SpritePaletteSwap_2D"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}

        _PaletteFrom ("Reference Palette (WxH)", 2D) = "white" {}
        _PaletteTo   ("Target Palette (WxH)", 2D) = "white" {}
        _SourcePalette   ("Source Palette (WxH)", 2D) = "white" {}

        _Color ("Tint", Color) = (1,1,1,1)

        _EnablePalette ("Enable Palette", Float) = 1
        _Tolerance ("Match Tolerance", Range(0,1)) = 0.015
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #define PAL_W 16
            #define PAL_H 3

            TEXTURE2D(_MainTex);     SAMPLER(sampler_MainTex);
            TEXTURE2D(_PaletteFrom); SAMPLER(sampler_PaletteFrom);
            TEXTURE2D(_PaletteTo);   SAMPLER(sampler_PaletteTo);

            float4 _Color;
            float _Tolerance;
            float _EnablePalette;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float4 Sample2DPalette(TEXTURE2D_PARAM(tex, samp), int x, int y)
            {
                float2 uv;
                uv.x = (x + 0.5) / PAL_W;
                uv.y = (y + 0.5) / PAL_H;

                return SAMPLE_TEXTURE2D(tex, samp, uv);
            }
             float _RequireExactMatch = 1;

            float4 frag(Varyings i) : SV_Target
            {
                float4 src = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                if (_EnablePalette < 0.5)
                    return src * _Color * i.color;
                if (src.a <= 0.001)
                    return 0;
                [unroll(PAL_H)]
                for (int y = 0; y < PAL_H; y++)
                {
                    [unroll(PAL_W)]
                    for (int x = 0; x < PAL_W; x++)
                    {
                        float4 refC = Sample2DPalette(TEXTURE2D_ARGS(_PaletteFrom, sampler_PaletteFrom), x, y);

                        if (refC.a <= 0.001)
                            continue;

                        float d = distance(src.rgb, refC.rgb);

                        if (d <= _Tolerance)
                        {
                            float4 dst = Sample2DPalette(TEXTURE2D_ARGS(_PaletteTo, sampler_PaletteTo), x, y);

                            if (dst.a <= 0.001)
                                return src * _Color * i.color;

                            return float4(dst.rgb, src.a) * _Color * i.color;
                        }
                    }
                }

                return src * _Color * i.color;
            }
            ENDHLSL
        }
    }
}