using UnityEngine;
using System.Collections;

public class LogicTest : MonoBehaviour {

	int x = 1;
	
	Int32Field time = new Int32Field("testobject", "testfield", 1);
	UInt64Field t = new UInt64Field("testobject", "testfield4", 1);
	SByteField time2 = new SByteField("testobject", "testfield2", 1);

	SingleField other = new SingleField("testobject", "testobject", 1.1f);
	StringField st = new StringField("testobject2", "floater", "");
	//Vector3Field v3 = new Vector3Field("testobject", "position", Vector3.zero);

	//QuaternionField q = new QuaternionField("Other", "rotation", Quaternion.identity);



	// Use this for initialization
	void Start () {
		//SaveLoadManager.SaveData("blah");
		Debug.Log(time.Value);
		//SaveData.Save();

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			//time.Value = (int)(100*Time.time);
			time2.Value = (sbyte)(100*Time.time);
			other.Value = (100*Time.time);
			st.Value = Time.time.ToString();

		}
		if(Input.GetMouseButtonDown(1)){
			PersistentData.Save();
		}
		if(Input.GetMouseButtonDown(2)){
			PersistentData.Load();
		}
		//v3.Value = transform.position;
	//	q.Value = transform.rotation;
		//PerformanceTest();
	}

	void OnDisable(){
		
	}



	void PerformanceTest(){
		Int32Data d = new Int32Data();
		FieldData da = d as FieldData;
		for(int i=0; i<100000; i++){
			FieldType t = da.Type;
			switch(t){
				case FieldType.Int32:
					Int32Data id = da as Int32Data;
				break;
			}
			
		}
	}
}


