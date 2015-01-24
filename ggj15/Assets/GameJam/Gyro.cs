using UnityEngine;
using System.Collections;

public class Gyro : MonoBehaviour {

	Quaternion rotFix = new Quaternion (0, 0, 1, 0);

	//public GUIText gt;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		Quaternion target = Input.gyro.attitude*rotFix;
		transform.localRotation = target;
		//Input.location.Start();
		//Input.compass.enabled = true;

	}
	
	// Update is called once per frame
	void Update () {
		if(Application.isEditor){
			transform.localRotation = Quaternion.AngleAxis(270, Vector3.right);
		}
		else{
			Quaternion target = Input.gyro.attitude*rotFix;
			transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Mathf.Clamp01(10f*Time.deltaTime));
		}
		

		//if(Input.compass != null){
			//Debug.Log(gt.text);
			//gt.text = Input.compass.trueHeading + ":";
		//}

		//Vector3 forward = transform.forward;
		//float angle = Mathf.Atan2(forward.z,forward.x);
		//Shader.SetGlobalVector("CameraDirection", new Vector4(forward.x, forward.y, forward.z, angle));
		//Debug.Log(angle*Mathf.Rad2Deg);

	}
}
