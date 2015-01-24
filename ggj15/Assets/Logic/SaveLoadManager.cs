using UnityEngine;
using System.Collections;

public static class SaveLoadManager  {

	static void LoadData(string filename){

	}

	static bool FileExists(string filename){
		return false;
	}

	public static void SaveData(string filename){
		Debug.Log(Application.persistentDataPath);
	}

	static void EraseData(string filename){

	}

	static void EraseAllData(){

	}
}
