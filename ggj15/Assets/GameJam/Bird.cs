using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

	/// Networking
	public string objectName;
	byte[] networkedBird;
	ByteField birdState;
	SingleField pX;
	SingleField pY;
	SingleField pZ;

	//SingleField yaw;
	//SingleField pitch;
	//SingleField roll;

	bool isLocal = false;

	public Transform camera;


	void Start(){
		birdState = new ByteField(objectName, "birdstate", 0);
		pX = new SingleField(objectName, "position x",0);
		pY = new SingleField(objectName, "position y",0);
		pZ = new SingleField(objectName, "position z",0);
	}

	void UpdateNetwork(){
		if(isLocal){
			Vector3 position = transform.position;
			pX.Value = position.x;
			pY.Value = position.y;
			pZ.Value = position.z;
		}
	}

	float roll;

	float velocity = 3f;
	float currentVelocity = 3f;
	//v = sqrt(r*g*tan(bank angle))
	//v^2 = r*g*tan(bank angle)
	//r = v^2 /g*tan(bank angle)
	float gravity = 9.8f;

	float targetYaw;
	float yaw = 0;
	float pitch;
	float rollSpeed = 0;


	void Update(){
		Quaternion targetRotation = camera.rotation;
		Quaternion currentRotation = transform.rotation;
		currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Mathf.Clamp01(2f*Time.deltaTime));

		Vector3 targetEulerAngles = targetRotation.eulerAngles;
		Vector3 eulerAngles = currentRotation.eulerAngles;
		//roll = Mathf.Clamp((eulerAngles.z+180f)%360f - 180f, -50f, 50f);
		pitch = Mathf.Clamp((eulerAngles.x+180f)%360f - 180f, -20f, 20f);
		
		targetYaw = targetEulerAngles.y;

		float height = transform.position.y;
		float actualPitch  = pitch;

		if(height < 10){
			if(pitch > 0){
				float adjustment = Mathf.Clamp01( (height-5)/ 5f);
				actualPitch =  adjustment*pitch;
			}
		}
		else if(height > 30){
			if(pitch < 0){
				float adjustment = Mathf.Clamp01(1f- (height-30)/ 5f);
				actualPitch =  adjustment*pitch;
			}
		}

		float deltaYaw = Mathf.DeltaAngle(yaw, targetYaw);
		roll = Smoothing.SpringSmoothAngle(roll, -Mathf.Clamp(deltaYaw,-45,45), ref rollSpeed, 1, Time.deltaTime);
		//roll = Mathf.Clamp(roll-0.25f*Time.deltaTime*deltaYaw*deltaYaw*Mathf.Sign(deltaYaw) - Time.deltaTime*deltaYaw, -50,50);

		if(Mathf.Abs(roll) > 0.1f){
			float turningRadius = (currentVelocity*currentVelocity)/ (gravity* Mathf.Tan(Mathf.Deg2Rad*roll) *0.4f);
			float circum = Mathf.PI*2f*turningRadius;
			float frameDistance = Time.deltaTime*currentVelocity;
			float degreesOfTurn = 360f*(frameDistance / circum);
			yaw -= degreesOfTurn;
		}

		transform.eulerAngles = new Vector3(actualPitch, yaw, roll);




		transform.position += Time.deltaTime* currentVelocity* transform.forward;
	}



}
