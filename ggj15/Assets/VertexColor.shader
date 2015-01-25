Shader "GGJ/Vertex Color" {

Properties {
    _CubeMap ("Cube Map (RGB)", Cube) = "white" {}

}

SubShader {
	Tags {"Queue"="Geometry" "RenderType"="Opaque"}

    Pass {
    	Cull Back // default
    	ZWrite On //default
    	ZTest LEqual //default
    	Blend Off
    	AlphaTest Off
    	Fog {Mode Off}

CGPROGRAM
#pragma vertex VertexProgram
#pragma fragment FragmentProgram

#include "UnityCG.cginc"

#include "Fog.cginc"
#pragma glsl
#pragma target 3.0 


struct VertexInput {
    float4  position : POSITION;
    float4 color : COLOR0;
    float4 normal : NORMAL;
};

struct VertexToFragment {

    float4  position : POSITION;
    float3 color : COLOR1;

};

samplerCUBE _CubeMap;
float4 _CubeMap_ST;

float4 _DLight;
float4 _DLightColor;


VertexToFragment VertexProgram (VertexInput vertex)
{
    VertexToFragment output;

    float4 worldPosition = mul(_Object2World, vertex.position);
    output.position = mul (UNITY_MATRIX_VP, worldPosition);
    float4 fog = Fog(worldPosition.xyz, _WorldSpaceCameraPos);

    float3 lightColor = half3(1,1,1);
    float3 worldNormal = mul(_Object2World, float4(vertex.normal.xyz, 0));

    float3 cubeTex = texCUBE(_CubeMap, worldNormal);

    float NDotL = saturate(dot(worldNormal,_DLight));

    output.color = NDotL*_DLightColor *  cubeTex;//vertex.color.rgb * lightColor*(1-fog.a) + fog.rgb * fog.a;
    return output;
}

half4 FragmentProgram (VertexToFragment fragment) : COLOR
{   

    return half4(fragment.color,1);
}

ENDCG



    }
}


} 
