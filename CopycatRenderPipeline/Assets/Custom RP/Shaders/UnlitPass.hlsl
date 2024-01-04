#ifndef CUSTOM_UNLIT_PASS_INCLUDED
#define CUSTOM_UNLIT_PASS_INCLUDED

#include "../ShaderLibrary/Common.hlsl"

struct appdata
{
};

struct v2f
{
};

CBUFFER_START(UnityPerMaterial)
float4 _BaseColor;
CBUFFER_END

float4 UnlitPassVertex(float3 positionOS : POSITION) : SV_Position
{
    float3 positionWS = TransformObjectToWorld(positionOS.xyz);
    return TransformWorldToHClip(positionWS);
}

float4 UnlitPassFragment(v2f i) : SV_Target
{
    return _BaseColor;
}

#endif
