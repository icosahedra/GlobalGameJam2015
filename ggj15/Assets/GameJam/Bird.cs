using UnityEngine;
using System.Collections;
using Icosahedra;
public class Bird : MonoBehaviour {

	/// Networking
	public string objectName;
	byte[] networkedBird = new byte[50];
	ByteField birdState;
	SingleField pX;
	SingleField pY;
	SingleField pZ;

	SingleField vX;
	SingleField vY;
	SingleField vZ;

	SingleField time;

	SingleField rYaw;
	SingleField rPitch;
	SingleField rRoll;

	public bool isLocal = false;

	public Transform gyro;
	public GameJamManager gjm;

	public AnimationNode flap;
	public AnimationNode soar;
	public AnimationNode noise;
	public AnimationController animationController;

	bool flapping = false;

	public float VelocityMagnitude {
		get{
			return currentVelocity;
		}
	}
	public float NormalizedVelocityMagnitude {
		get{
			return Mathf.Clamp01(currentVelocity - ascentVelocity)/ (descentVelocity-ascentVelocity);
		}
	}
	void Start(){
		birdState = new ByteField(objectName, "birdstate", 0);
		pX = new SingleField(objectName, "position x",0);
		pY = new SingleField(objectName, "position y",0);
		pZ = new SingleField(objectName, "position z",0);

		vX = new SingleField(objectName, "velocity x",0);
		vY = new SingleField(objectName, "velocity y",0);
		vZ = new SingleField(objectName, "velocity z",0);

		rYaw = new SingleField(objectName, "rotation yaw",0);
		rPitch = new SingleField(objectName, "rotation pitch",0);
		rRoll = new SingleField(objectName, "rotation roll",0);

		time = new SingleField(objectName, "time",0);

		animationController.AddAnimation(flap);
		animationController.AddAnimation(soar);
		animationController.AddAnimation(noise);
		animationController.PlayAnimation(soar);
		animationController.PlayAnimation(noise);
	}

	void SerializeBird(){
		networkedBird[0] = 1;//birdState.Value;
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

		converter.value = vX.Value;
		networkedBird[25] = converter.byte0;
		networkedBird[26] = converter.byte1;
		networkedBird[27] = converter.byte2;
		networkedBird[28] = converter.byte3;

		converter.value = vY.Value;
		networkedBird[29] = converter.byte0;
		networkedBird[30] = converter.byte1;
		networkedBird[31] = converter.byte2;
		networkedBird[32] = converter.byte3;

		converter.value = vZ.Value;
		networkedBird[33] = converter.byte0;
		networkedBird[34] = converter.byte1;
		networkedBird[35] = converter.byte2;
		networkedBird[36] = converter.byte3;

		converter.value = time.Value;
		networkedBird[37] = converter.byte0;
		networkedBird[38] = converter.byte1;
		networkedBird[39] = converter.byte2;
		networkedBird[40] = converter.byte3;
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

		converter.byte0 = networkedBird[25];
		converter.byte1 = networkedBird[26];
		converter.byte2 = networkedBird[27];
		converter.byte3 = networkedBird[28];
		vX.Value = converter.value;

		converter.byte0 = networkedBird[29];
		converter.byte1 = networkedBird[30];
		converter.byte2 = networkedBird[31];
		converter.byte3 = networkedBird[32];
		vY.Value = converter.value;

		converter.byte0 = networkedBird[33];
		converter.byte1 = networkedBird[34];
		converter.byte2 = networkedBird[35];
		converter.byte3 = networkedBird[36];
		vZ.Value = converter.value;

		converter.byte0 = networkedBird[37];
		converter.byte1 = networkedBird[38];
		converter.byte2 = networkedBird[39];
		converter.byte3 = networkedBird[40];
		time.Value = converter.value;
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

			vX.Value = velocityVector.x;
			vY.Value = velocityVector.y;
			vZ.Value = velocityVector.z;
			time.Value = Time.time;
			SerializeBird();
			gjm.SendData(networkedBird);
		}

	}

	float lastNetworkUpdate = -1;

	void ApplyNetData(){


		///should be greater, but fuck it
		if(time.Value != lastNetworkUpdate){
			lastNetworkUpdate = time.Value;
			yaw = rYaw.Value;
			actualPitch = rPitch.Value;
			roll = rRoll.Value;


			transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(actualPitch, Vector3.right) * Quaternion.AngleAxis(roll, Vector3.forward);
			transform.position = new Vector3(pX.Value, pY.Value, pZ.Value);
		}
		else{
			transform.position += Time.deltaTime*new Vector3(vX.Value, vY.Value, vZ.Value);
		}
		
	}

	float rollControl = 0;
	float pitchControl = 0;

	float roll;

	float normalVelocity = 6f;
	float descentVelocity = 20f;
	float ascentVelocity = 3f;

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

	Vector3 velocityVector;

	void Respawn(){
		transform.position = new Vector3(0,30,0);
	}

	void OnTriggerEnter(){
		Respawn();
	}

	void LateUpdate(){
		if(actualPitch < -5){
			animationController.BlendAnimation(flap, 1, 0.5f);
		}
		else if(actualPitch > -1){
			animationController.BlendAnimation(flap, 0, 0.75f);
		}
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


		float maxVelocity = normalVelocity;
		float acceleration = 1;
		if(actualPitch > 2){
			float descent = Mathf.Clamp01(( actualPitch -2f) /8f); ///descent, 0 to 1
			maxVelocity = normalVelocity +  descent * (descentVelocity-normalVelocity);
			acceleration = 1 + descent * 10f;
			
		}
		else if(actualPitch < -10){
			float ascent = Mathf.Clamp01(( -actualPitch -10f) /10f); ///ascent, 0 to 1
			maxVelocity = ascent * ascentVelocity + (1-ascent)*normalVelocity;
			acceleration = 1;
		}
		else{
			maxVelocity = normalVelocity;
			acceleration = 1;
		}

		currentVelocity = Mathf.Clamp(currentVelocity + Mathf.Clamp01(acceleration*Time.deltaTime), ascentVelocity, descentVelocity);

		animationController.SetPlaybackSpeed(noise, 0.25f+Mathf.Clamp01(currentVelocity/normalVelocity-1));

		float maxVSqr = maxVelocity * maxVelocity;
		float dragCoef = acceleration / maxVSqr;
		currentVelocity = currentVelocity - dragCoef*currentVelocity*currentVelocity*Time.deltaTime;

		
		//Debug.Log(actualPitch + " : " + currentVelocity);
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



		velocityVector = currentVelocity* transform.forward;
		transform.position += Time.deltaTime* velocityVector;


		UpdateNetwork();

	}



}
