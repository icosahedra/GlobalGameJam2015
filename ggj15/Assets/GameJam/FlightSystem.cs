using UnityEngine;
using System.Collections;

public class FlightSystem : MonoBehaviour {

	
	//Data Values
	private float criticalAngleOfAttack = 15;
	
	private float maxVelocity = 100;  // meters/second, used to calculate parasitic drag
	private float mass = 2000; //in kilograms
	private float engineIdleSpeed = 0.2f;  //in percent of max power, the minimum engine power
	private float engineCruiseSpeed = 0.5f;  //in percent of max power, the engine cruising speed, where lift is balanced
	private float noseUpMax = 15;
	private float noseDownMax = -30;
	
	//Derived Values
	private float powerToWeight = 1.5f;
	private float maximumThrust;  //newtons , power to weight ratio of  0.5
	private float parasiticDrag;
	private float liftConstant;
	private float inducedDrag = 100;  //NOTE: I have no idea how to set this constant
	private float cruiseSpeed;
	//Constants
	const float GRAVITY = 9.8f;

	
	//Input and Control Surface Varibles
	private float aileron = 0;  //-1 to 1, representing full left to full right - left aileron up is bank left
	private float elevator = 0; //-1 to 1, representing full nose down to nose up - elevators up is nose up
	private float aileronResponseSpeed = 2f; //units per second
	private float elevatorResponseSpeed = 2f; //units per second
	
	private float rollSpeed = 180; //Degrees per second
	private float pitchSpeed = 10; //Degrees per second
	
	//Aircraft orientation variables
	private float rollAngle = 0;  //this is calculated after rotations are applied, used in force calcs
	private float noseAngle = 0;
	
	//Engine Values
	private float engineOutput;  //0-1, in percent of maximum output
	private float engineResponseSpeed = .5f;  //the speed at which the engine can deliver more power after input, at present, tied to keyboard response as well
	private float thrust;
	
	private float angleOfAttack = 0;
	
	private float lift;
	private Vector3 liftVector;

	private float totalDrag;
	private Vector3 dragVector;
	

	private float turningRadius;
	
	private Vector3 airspeed;  //the speed air passes by the plane
	private float sqrAirspeed;  //sqr magnitude
	private float airspeedPercent; //percent of max speed, used for turning
	
	
	private Vector3 accelerations;
	
	
	//Test Objects
	public Transform alignment;
	public GUIText throttle;
	public GUIText speed;	
	public GUIText alt;	
	
	public Transform leftA;
	public Transform rightA;
	public Transform elev;

	
	void Awake(){
		InitializeValues();
		SetInitialVelocity();
	}

	void InitializeValues(){
		//Set thrust based on our desired powerToWeight ratio
		maximumThrust = GRAVITY * mass * powerToWeight;
		
		//Set drag constant based on maxVelocity, solve for maximumThrust == parasiticDrag
		float maxVelocitySqr = maxVelocity*maxVelocity;
		parasiticDrag = maximumThrust / maxVelocitySqr;
		
		float cruiseThrust = engineCruiseSpeed * maximumThrust;
		float sqrCruiseSpeed = cruiseThrust / parasiticDrag;
		cruiseSpeed = Mathf.Sqrt(sqrCruiseSpeed);
			//Debug.Log( Mathf.Sqrt(sqrCruiseSpeed) );
		
		liftConstant = GRAVITY*mass / (LiftCoefficient(0) * sqrCruiseSpeed);
		
		
	}
	
	void SetInitialVelocity(){
		airspeed = new Vector3(0,0,cruiseSpeed );	
		engineOutput = engineCruiseSpeed;
	}

	void Update(){
		
		UpdateControlSurfaces();  //Take input from the player, and apply it to the control surfaces
		SimulateRoll();
		
		CalculateThrust();
		CalculateAngleOfAttack();
		CalculateLift();
		CalculateDrag();
		
		
		
		
		ApplyAcceleration();
		OrientHeading();
		
		throttle.text = "Throttle: " + engineOutput;
		speed.text = "Airspeed: " + Mathf.Sqrt(sqrAirspeed)*3.6f;
		alt.text = "Altitude: " + transform.position.y;
		
		leftA.localRotation = Quaternion.AngleAxis(0.5f*Mathf.Rad2Deg*Mathf.Asin(aileron),Vector3.right);
		rightA.localRotation = Quaternion.AngleAxis(-0.5f*Mathf.Rad2Deg*Mathf.Asin(aileron),Vector3.right);
		elev.localRotation = Quaternion.AngleAxis(0.5f*Mathf.Rad2Deg*Mathf.Asin(elevator),Vector3.right);
	}
	
	
	
	//This function moves the aircraft control surfaces
	private void UpdateControlSurfaces(){
		//this has modeled a delay in how quickly the surfaces fully respond
		
		if(Input.GetKey(KeyCode.W)){
			elevator = Mathf.Clamp(elevator + Time.deltaTime*elevatorResponseSpeed,-1,1);
		}
		else if(Input.GetKey(KeyCode.S)){
			elevator = Mathf.Clamp(elevator - Time.deltaTime*elevatorResponseSpeed,-1,1);
		}
		else{
			elevator = Mathf.Clamp(elevator -elevator*Mathf.Clamp01(Time.deltaTime*elevatorResponseSpeed),-1,1);
		}
		
		if(Input.GetKey(KeyCode.A)){
			aileron = Mathf.Clamp(aileron + Time.deltaTime*aileronResponseSpeed,-1,1);
		}
		else if(Input.GetKey(KeyCode.D)){
			aileron = Mathf.Clamp(aileron - Time.deltaTime*aileronResponseSpeed,-1,1);
		}
		else{
			aileron = Mathf.Clamp(aileron -aileron*Mathf.Clamp01(Time.deltaTime*aileronResponseSpeed),-1,1);	
		}
	}
	
	
	//This function actually rotates the aircraft.
	//Notes: Should rotation speed depend on airspeed?
	//We may need to calculate actual orientation values from this	
	private void SimulateRoll(){
		
		//Both these rotations are applied in local space
		transform.Rotate(Vector3.forward, airspeedPercent*rollSpeed*aileron*Time.deltaTime);
		//transform.Rotate(Vector3.right, pitchSpeed*elevator*Time.deltaTime);
		noseAngle = noseAngle + airspeedPercent*Mathf.Clamp01(5*Time.deltaTime)*(elevator* noseUpMax - noseAngle);
		
		rollAngle = (transform.eulerAngles.z);  //since z rotation is applied last, this is safely independent

	}
	
	void CalculateThrust(){
		
		if(Input.GetKey(KeyCode.R)){
			engineOutput = Mathf.Clamp(engineOutput + Time.deltaTime*engineResponseSpeed,engineIdleSpeed,1);
		}
		else if(Input.GetKey(KeyCode.F)){
			engineOutput = Mathf.Clamp(engineOutput - Time.deltaTime*engineResponseSpeed,engineIdleSpeed,1);
		}
		
		thrust = engineOutput * maximumThrust;
	
	}
	
	//This should be a function that returns values, -1 to 1
	//NOTES: Can lift be negative, if angle of attack is negative?  I think so, especially for inverted flight
	//Notes: we may want to account for incident angle of attack here as well
	float LiftCoefficient(float angleOfAttack){
		
		float normalizedValue;
		//this is dumb for now
		if(angleOfAttack > 2*criticalAngleOfAttack){
			normalizedValue = 0;
		}
		else{
			normalizedValue = Mathf.Clamp(0.25f + angleOfAttack / criticalAngleOfAttack,-1,1);
		}
		return normalizedValue;	
	}
	
	
	
	void CalculateAngleOfAttack(){
		//get the component along the airframe
		angleOfAttack = Vector3.Angle(airspeed, transform.forward);
		if(transform.forward.y < airspeed.normalized.y){
			angleOfAttack = - angleOfAttack;
		}
		angleOfAttack-= noseAngle;
	}
	
	void CalculateLift(){
		lift = LiftCoefficient(angleOfAttack) * sqrAirspeed * liftConstant;
		liftVector = transform.up * lift;
		
		Debug.Log(lift/(GRAVITY*mass));
	}
	

	void CalculateDrag(){
		float iDrag = 0;
		if(sqrAirspeed != 0){
			iDrag = lift*lift / (sqrAirspeed * inducedDrag);
		}
//		
		float pDrag = sqrAirspeed * parasiticDrag;
////		Debug.Log(lift + " : " + inducedDrag + " : " + parasiticDrag);
		
		totalDrag = pDrag + iDrag; //inducedDrag + 
		dragVector = -airspeed.normalized*totalDrag;
	}

	void ApplyAcceleration(){
		
		accelerations = ( (transform.forward * thrust) + liftVector + dragVector) / mass;
		accelerations += new Vector3(0,-GRAVITY,0); 
		
		float time = Time.deltaTime;
		float deltaX =  time * accelerations.x;
		float deltaY =  time * accelerations.y;
		float deltaZ =  time * accelerations.z;
		
		airspeed += new Vector3(deltaX, deltaY, deltaZ);
		
		sqrAirspeed = airspeed.sqrMagnitude;
		airspeedPercent = Mathf.Sqrt(sqrAirspeed) / maxVelocity;  //clamp?
		Vector3 deltaMove = airspeed * time; 
//		Debug.Log(airspeed.magnitude);
		transform.position += deltaMove;

		

//		if(theta != 0){
//			turningRadius = mass*sqrAirspeed/(lift * Mathf.Sin(Mathf.Deg2Rad*theta));
//			Vector3 headingVector = airspeed;
//			headingVector.y = 0;
//			
//			transform.forward = headingVector.normalized;  //this always points us in the direction we are heading, oooh dirty hack
//			
//			transform.rotation *= Quaternion.AngleAxis(theta, transform.forward);	
//		}
//		
	}
	
	
	//This function is intended to orient us against sheer force, not against pitch, we should fix that
	void OrientHeading(){
		Vector3 heading = airspeed;
		//heading = heading.normalized;
//		alignment.forward = heading;
//		Vector3 forward = transform.forward;
//		
//		Vector3 tmp = Vector3.Cross(transform.up, heading);
//		Vector3 result = Vector3.Cross(tmp, transform.up);
//		forward = result.normalized;
//		
//		transform.LookAt(transform.position + forward, transform.up);
		
		
		
		transform.LookAt(transform.position + heading, transform.up);
		//transform.rotation *= Quaternion.AngleAxis(noseAngle, Vector3.right);
		//transform.Rotate(transform.right, noseAngle);

		//we shoudld do that, then attempt to maintain last elevation setting...
		
		
//		transform.forward = forward;
//		Debug.Log(heading.magnitude);
		//Vector3 heading = airspeed.normalized;
		//heading.y += Mathf.Tan(Mathf.Deg2Rad*noseUpMax*elevator);
		
//		Debug.Log(transform.eulerAngles.y);
//		Vector3 rotations = transform.eulerAngles;
//		Vector3 headingVector = airspeed;
//		headingVector.y = 0;
//		transform.forward = headingVector.normalized;
//		rotations.y = transform.eulerAngles.y;
//		transform.eulerAngles = rotations;
		
//		float angle = (Vector3.Angle(headingVector, Vector3.forward));
//		headingVector = transform.eulerAngles;
//		headingVector.y = angle;
//		transform.eulerAngles =  headingVector;
//		transform.forward = airspeed;
		
	}
	
	
	
//	
//	//we could just plot a lift to drag ratio...
//	
//	
//	//public float maxCoefficientOfLift =2;
//	
//	// Induced Drag = Lift ^2 / (.5 * airspeed ^2 * someconstant)
//	// Parasitic Drag = .5 * airspeed^2 * someotherconstant
//	
//	//Lift = .5 * airspeed^2 * liftCoeffecientAtAngleOfAttack * anotherConstant  //see angle of attack
//	//lift happens perpendicular to relative wind.... fuck me.
//	
	
}


/* 
TODOS
+5 to -2.5 would be a good G range
v^2 for control surfaces , they are good for max speed right now


Common Aircraft thrust to weight ratios
	Condorde .373
	f22 .84
	Mig 29 1.1
	f15 1.04 - 1.6
	we'll use 1, it makes values easy
	// somewhere between 0.5 and 1
	// build in an angle of incidence (offset for wing values)
	
	//pitch caused by lift on the elevators
	//sideslip - rudder controls

L Sin theta = mv^2/r
radius = mv^2 /lsin theta

Lift = mg / cos theta //lift exceeds weight in banking flight


//stall, will auto pitch down
//pitch

//		//idle 60%, max is 100%
//		//idle prop rpm - 100
//		//full power rpm - 1050, 195 knots
//		// cruise - 500 - 150 knots
//		// 300 rpm 120 knots
//		// 800 rpm 170 knots
//		

*/
