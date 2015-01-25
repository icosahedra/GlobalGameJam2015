using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Lighting : MonoBehaviour {

	Color dLightColor;
	public Gradient dLightByTime;

	Color moonLightColor;
	public Gradient moonLightByTime;

	Color ambientLight;
	public Gradient ambientByTime;

	public Transform moonTransform;

	const int daySeconds = 60*60*24;
	 int timeOfDay = 0;
	float speedMultiplier = 360; //240 second day
	float startTime = 0;
	public float dayPercent;

	public TimeFog[] fogBlend;
	Texture2D fogStart;
	Texture2D fogEnd;
	float fogBlendValue;


	[System.Serializable]
	public class TimeFog{
		public Texture2D texture;
		//public float time;
	}


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

		//if(Application.isPlaying){
		//	UpdateTime();
		//}

		dLightColor = dLightByTime.Evaluate(dayPercent);
		transform.rotation = Quaternion.AngleAxis(360f*dayPercent -90, Vector3.right);

		moonLightColor = moonLightByTime.Evaluate(dayPercent);
		moonTransform.rotation = Quaternion.AngleAxis(360f*dayPercent -270 +20, Vector3.right)*Quaternion.AngleAxis(30, Vector3.up);

		ambientLight = ambientByTime.Evaluate(dayPercent);

		Shader.SetGlobalVector("_DLight", -transform.forward);
		Shader.SetGlobalColor("_DLightColor", dLightColor);

		Shader.SetGlobalVector("_MoonLight", -moonTransform.forward);
		Shader.SetGlobalColor("_MoonLightColor", moonLightColor);

		Shader.SetGlobalColor("_Ambient", ambientLight);

		UpdateFog();
	}
	
	int start;
	int end;
	float val;
	void UpdateFog(){

		int length = fogBlend.Length;
		 val = Mathf.Clamp01(dayPercent) * (length-1);
		 start = Mathf.Clamp( (int)(val),0 ,length-1);
		 end = Mathf.Clamp( (start + 1) ,0, length-1);

		Shader.SetGlobalTexture("_TextureFogStart", fogBlend[start].texture);
		Shader.SetGlobalTexture("_TextureFogEnd", fogBlend[end].texture);
		fogBlendValue = Mathf.Clamp01(val - start);
		Shader.SetGlobalFloat("_TextureFogBlend", fogBlendValue);

		Shader.SetGlobalVector("_LinearFog", new Vector4(fogColorCameraZMin, fogColorCameraZMax, fogColorWorldYMin,fogColorWorldYMax));
		Shader.SetGlobalVector("_FogDensity", new Vector4(fogDensityCameraZMin, fogDensityCameraZMax, distanceFogScale,verticalFogScale));
		Shader.SetGlobalVector("_VerticalFog", fogDensityWorldY);
		//Shader.SetGlobalColor("_Ambient", ambientLight);

		Shader.SetGlobalTexture("_TextureFog", textureFog);
	}

	float timeAccumulator = 0;
	public void UpdateTime(float speed){
		timeAccumulator = Mathf.Clamp(timeAccumulator + speed*Time.deltaTime, 0, 240);
		timeOfDay = ((int)(timeAccumulator*speedMultiplier))%daySeconds;
		dayPercent = (float)timeOfDay / (float)daySeconds;
	}
	public void ReverseTime(float speed){
		timeAccumulator = Mathf.Clamp(timeAccumulator - speed*Time.deltaTime, 0, 240);
		timeOfDay = ((int)(timeAccumulator*speedMultiplier))%daySeconds;
		dayPercent = (float)timeOfDay / (float)daySeconds;
	}
	
}
