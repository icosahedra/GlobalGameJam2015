uniform float4 _LinearFog;

uniform sampler2D _TextureFog;
uniform sampler2D _TextureFogStart;
uniform sampler2D _TextureFogEnd;
uniform float _TextureFogBlend
;
uniform float4 _FogDensity;
uniform float4 _VerticalFog;

float4 Fog(float3 worldPosition, float3 cameraPosition){


	float3 distanceVector = worldPosition - _WorldSpaceCameraPos;
	float distanceToCamera = length(distanceVector);

	

	float yValue = smoothstep(_LinearFog.z, _LinearFog.w, worldPosition.y);
	float xValue = smoothstep(_LinearFog.x, _LinearFog.y, distanceToCamera);

	//float4 textureFog = tex2D(_TextureFog, float2(xValue, yValue));
	float4 textureFogStart = tex2D(_TextureFogStart, float2(xValue, yValue));
	float4 textureFogEnd = tex2D(_TextureFogEnd, float2(xValue, yValue));

	float4 textureFog = (1-_TextureFogBlend)*textureFogStart + _TextureFogBlend * textureFogEnd;

	float distanceFogDensity =  (smoothstep(_FogDensity.x,_FogDensity.y,distanceToCamera) );
	distanceFogDensity = _FogDensity.z * sqrt(distanceFogDensity);


	float verticalFogDensity = (1.0-smoothstep(_VerticalFog.x,_VerticalFog.y,worldPosition.y));// + (smoothstep(_VerticalFog.z,_VerticalFog.w,worldPosition.y)) ;
	verticalFogDensity = _FogDensity.w * sqrt(verticalFogDensity);

	float totalDensity = saturate(verticalFogDensity + distanceFogDensity);


    return float4(textureFog.rgb,totalDensity);//totalDensity );
}

