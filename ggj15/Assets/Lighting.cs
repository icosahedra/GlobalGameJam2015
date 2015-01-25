using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Lighting : MonoBehaviour {

	public Color dLightColor;
	public Gradient dLightByTime;

	public Color moonLightColor;
	public Gradient moonLightByTime;
	public Transform moonTransform;

	const int daySeconds = 60*60*24;
	 int timeOfDay = 0;
	float speedMultiplier = 360; //240 second day
	float startTime = 0;
	public float dayPercent;


	[SerializeField] float fogColorCameraZMin;
	[SerializeField] float fogColorCameraZMax;

	[SerializeField] float fogColorWorldYMin;
	[SerializeField] float fogColorWorldYMax;

	[SerializeField] Texture2D textureFog;

	[SerializeField] float fogDensityCameraZMin;
	[SerializeField] float fogDensityCameraZMax;

	[SerializeField] Vector4 fogDensityWorldY;

	[SerializeField] float distanceFogScale;
	[SerializeField] float verticalFogScale;

	void Start(){
		startTime = Time.realtimeSinceStartup;
	}


	void Update () {

		if(Application.isPlaying){
			UpdateTime();
		}

		dLightColor = dLightByTime.Evaluate(dayPercent);
		transform.rotation = Quaternion.AngleAxis(360f*dayPercent -90, Vector3.right);

		moonLightColor = moonLightByTime.Evaluate(dayPercent);
		moonTransform.rotation = Quaternion.AngleAxis(360f*dayPercent -270 +20, Vector3.right)*Quaternion.AngleAxis(30, Vector3.up);

		Shader.SetGlobalVector("_DLight", -transform.forward);
		Shader.SetGlobalColor("_DLightColor", dLightColor);

		Shader.SetGlobalVector("_MoonLight", -moonTransform.forward);
		Shader.SetGlobalColor("_MoonLightColor", moonLightColor);

		UpdateFog();
	}
	
	void UpdateFog(){
		Shader.SetGlobalVector("_LinearFog", new Vector4(fogColorCameraZMin, fogColorCameraZMax, fogColorWorldYMin,fogColorWorldYMax));
		Shader.SetGlobalVector("_FogDensity", new Vector4(fogDensityCameraZMin, fogDensityCameraZMax, distanceFogScale,verticalFogScale));
		Shader.SetGlobalVector("_VerticalFog", fogDensityWorldY);
		//Shader.SetGlobalColor("_Ambient", ambientLight);

		Shader.SetGlobalTexture("_TextureFog", textureFog);
	}

	void UpdateTime(){
		float deltaTime = Time.realtimeSinceStartup - startTime;
		timeOfDay = ((int)(deltaTime*speedMultiplier))%daySeconds;
		dayPercent = (float)timeOfDay / (float)daySeconds;
	}
	
}
