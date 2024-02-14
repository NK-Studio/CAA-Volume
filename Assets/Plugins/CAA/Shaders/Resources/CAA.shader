Shader "Hidden/Universal Render Pipeline/CAA"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    float _Intensity;
    float2 _RedOffset;
    float2 _GreenOffset;
    float2 _BlueOffset;

    half4 Frag(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        half3 color;

        float2 red_uv = input.texcoord +  _RedOffset * _Intensity;
        float2 green_uv = input.texcoord + _GreenOffset * _Intensity;
        float2 blue_uv = input.texcoord + _BlueOffset * _Intensity;

        color.r = _BlitTexture.SampleLevel(sampler_PointClamp, red_uv, 0).r;
        color.g = _BlitTexture.SampleLevel(sampler_PointClamp, green_uv, 0).g;
        color.b = _BlitTexture.SampleLevel(sampler_PointClamp, blue_uv, 0).b;

        return half4(color, 1);
    }
    ENDHLSL

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent"
        }

        Pass
        {
            Name "CAA Pass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
}