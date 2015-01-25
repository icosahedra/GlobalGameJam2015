using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

	/// Networking
	public string objectName;
	byte[] networkedBird = new byte[25];
	ByteField birdState;
	SingleField pX;
	SingleField pY;
	SingleField pZ;

	SingleField rYaw;
	SingleField rPitch;
	SingleField rRoll;

	public bool isLocal = false;

	public Transform gyro;
	public GameJamManager gjm;

	void Start(){
		birdState = new ByteField(objectName, "birdstate", 0);
		pX = new SingleField(objectName, "position x",0);
		pY = new SingleField(objectName, "position y",0);
		pZ = new SingleField(objectName, "position z",0);

		rYaw = new SingleField(objectName, "rotation yaw",0);
		rPitch = new SingleField(objectName, "rotation pitch",0);
		rRoll = new SingleField(objectName, "rotation roll",0);
	}

	void SerializeBird(){
		networkedBird[0] = birdState.Value;
		Icosahedra.IO.BinaryWriter.FloatToBytes converter = new Icosahedra.IO.BinaryWriter.FloatToBytes();

		converter.value = pX.Value;
		networkedBird[1] = converter.byte0;
		networkedBird[2] = converter.byte1;
		networkedBird[3] = converter.byte2;
		networkedBird[4] = converter.byte3;

		converter.value = pY.Value;
		networkedBird[5] = converter.byte0;
		networkedBird[6] = converter.byte1;
		networkedBird[7] = converter.byte2;
		networkedBird[8] = converter.byte3;

		converter.value = pZ.Value;
		networkedBird[9] = converter.byte0;
		networkedBird[10] = converter.byte1;
		networkedBird[11] = converter.byte2;
		networkedBird[12] = converter.byte3;

		converter.value = rYaw.Value;
		networkedBird[13] = converter.byte0;
		networkedBird[14] = converter.byte1;
		networkedBird[15] = converter.byte2;
		networkedBird[16] = converter.byte3;

		converter.value = rPitch.Value;
		networkedBird[17] = converter.byte0;
		networkedBird[18] = converter.byte1;
		networkedBird[19] = converter.byte2;
		networkedBird[20] = converter.byte3;

		converter.value = rRoll.Value;
		networkedBird[21] = converter.byte0;
		networkedBird[22] = converter.byte1;
		networkedBird[23] = converter.byte2;
		networkedBird[24] = converter.byte3;
	}

	void DeserializeBird(){
		birdState.Value = networkedBird[0];
		Icosahedra.IO.BinaryWriter.FloatToBytes converter = new Icosahedra.IO.BinaryWriter.FloatToBytes();


		converter.byte0 = networkedBird[1];
		converter.byte1 = networkedBird[2];
		converter.byte2 = networkedBird[3];
		converter.byte3 = networkedBird[4];
		pX.Value = converter.value;

		converter.byte0 = networkedBird[5];
		converter.byte1 = networkedBird[6];
		converter.byte2 = networkedBird[7];
		converter.byte3 = networkedBird[8];
		pY.Value = converter.value;

		converter.byte0 = networkedBird[9];
		converter.byte1 = networkedBird[10];
		converter.byte2 = networkedBird[11];
		converter.byte3 = networkedBird[12];
		pZ.Value = converter.value;

		converter.byte0 = networkedBird[13];
		converter.byte1 = networkedBird[14];
		converter.byte2 = networkedBird[15];
		converter.byte3 = networkedBird[16];
		rYaw.Value = converter.value;

		converter.byte0 = networkedBird[17];
		converter.byte1 = networkedBird[18];
		converter.byte2 = networkedBird[19];
		converter.byte3 = networkedBird[20];
		rPitch.Value = converter.value;

		converter.byte0 = networkedBird[21];
		converter.byte1 = networkedBird[22];
		converter.byte2 = networkedBird[23];
		converter.byte3 = networkedBird[24];
		rRoll.Value = converter.value;
	}

	void UpdateNetwork(){
		if(isLocal){
			Vector3 position = transform.position;
			pX.Value = position.x;
			pY.Value = position.y;
			pZ.Value = position.z;

			rYaw.Value = yaw;
			rPitch.Value = actualPitch;
			rRoll.Value = roll;

			SerializeBird();
			gjm.SendData(networkedBird);
		}

	}

	void ApplyNetData(){
		yaw = rYaw.Value;
		actualPitch = rPitch.Value;
		roll = rRoll.Value;


		transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(actualPitch, Vector3.right) * Quaternion.AngleAxis(roll, Vector3.forward);
		transform.position = new Vector3(pX.Value, pY.Value, pZ.Value);
	}

	float rollControl = 0;
	float pitchControl = 0;

	float roll;

	float velocity = 3f;
	float currentVelocity = 5f;
	//v = sqrt(r*g*tan(bank angle))
	//v^2 = r*g*tan(bank angle)
	//r = v^2 /g*tan(bank angle)
	float gravity = 9.8f;

	float targetYaw;
	float yaw = 0;
	float pitch;
	float rollSpeed = 0;
	float pitchSpeed = 0;
	float actualPitch;

	void Respawn(){
		transform.position = Vector3.zero;
	}

	void OnTriggerEnter(){
		Respawn();
	}


	void Update(){
		if(isLocal){
			LocalControl();
		}
		else{

			byte[] netData = gjm.GetNetData();
			
			if(netData != null){
				if(netData.Length >= networkedBird.Length){
					for(int i=0; i<networkedBird.Length; i++){
						networkedBird[i] = netData[i];
					}
				}
			}
			DeserializeBird();
			ApplyNetData();
		}
	}

	void LocalControl(){
		Quaternion targetRotation = gyro.rotation;
		Quaternion currentRotation = transform.rotation;
		currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Mathf.Clamp01(2f*Time.deltaTime));

		Vector3 targetEulerAngles = targetRotation.eulerAngles;
		//Vector3 eulerAngles = currentRotation.eulerAngles;
		rollControl = Mathf.Clamp( ( (targetEulerAngles.z+540f)%360f - 180f)/30f, -1f, 1f) ;
		pitchControl = Mathf.Clamp( ((targetEulerAngles.x+180f)%360f - 180f - 20f)/40f, -1f, 1f);
		
		targetYaw = targetEulerAngles.y;

		float height = transform.position.y;
		

		//roll = rollControl*rollControl*Mathf.Sign(rollControl)*30f;
		roll = Smoothing.SpringSmooth(roll, rollControl*40f, ref rollSpeed, 0.5f, Time.deltaTime);
		pitch = Smoothing.SpringSmooth(pitch, pitchControl*40f, ref pitchSpeed, 0.5f, Time.deltaTime);
		actualPitch  = pitch;
		//if(height < 10){
		//	if(pitch > 0){
		//		float adjustment = Mathf.Clamp01( (height-5)/ 5f);
		//		actualPitch =  adjustment*pitch;
		//	}
		//}
		//else 
		if(height > 200){
			if(pitch < 0){
				float adjustment = Mathf.Clamp01(1f- (height-200)/ 5f);
				actualPitch =  adjustment*pitch;
			}
		}

		//Debug.Log(pitch);
		//float deltaYaw = Mathf.DeltaAngle(yaw, targetYaw);
		//roll = Smoothing.SpringSmoothAngle(roll, -Mathf.Clamp(deltaYaw,-45,45), ref rollSpeed, 1, Time.deltaTime);
		//roll = Mathf.Clamp(roll-0.25f*Time.deltaTime*deltaYaw*deltaYaw*Mathf.Sign(deltaYaw) - Time.deltaTime*deltaYaw, -50,50);

		if(Mathf.Abs(roll) > 0.1f){
			float turningRadius = (currentVelocity*currentVelocity)/ (gravity* Mathf.Tan(Mathf.Deg2Rad*roll) *0.4f);
			float circum = Mathf.PI*2f*turningRadius;
			float frameDistance = Time.deltaTime*currentVelocity;
			float degreesOfTurn = 360f*(frameDistance / circum);
			yaw -= degreesOfTurn;
		}
		//Debug.Log(yaw + " : yaw " );
		transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(actualPitch, Vector3.right) * Quaternion.AngleAxis(roll, Vector3.forward);
		//transform.eulerAngles = new Vector3(0, yaw, roll);//actualPitch




		transform.position += Time.deltaTime* currentVelocity* transform.forward;


		UpdateNetwork();

	}



}
