Shader "Custom RP/Lit"
{
    Properties {
        _BaseColor ("Base Color", Color) = (0.5,0.5,0.5,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        [Toggle(_CLIPPING)] _CLipping("Alpha Clipping", Float) = 0
        _BaseMap("Texture" , 2D) = "white" {}
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
        [Enum(Off,0,On,1)] _ZWrite ("Z Write", Float) = 1
        }
    SubShader
    {
        Pass
        {
            
            Tags { "LightMode" = "CustomLit" }
            
            Blend [_SrcBlend] [_DstBlend]
            Zwrite [_ZWrite]
            
            HLSLPROGRAM
            #pragma target 3.5
            #pragma shader_feature _CLIPPING
            #pragma multi_compile_instancing
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment
            #include "LitPass.hlsl"
            ENDHLSL
        }
    }
}