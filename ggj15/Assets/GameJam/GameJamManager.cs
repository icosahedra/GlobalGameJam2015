using UnityEngine;
using System.Collections;
using System.Net;
using Icosahedra.Net;

public class GameJamManager : MonoBehaviour {

	
	SyncUdpClient network = new SyncUdpClient();

	public void Connect(UnityEngine.UI.InputField ipad){
		IPAddress ip;
		try{
			ip = IPAddress.Parse(ipad.text);
		}
		catch(System.Exception e){
			Debug.Log(e);
			ip = null;
		}
		if(ip != null){
			if(!network.Initialized){
			network.Initialize(ip);
			}
		}
	}	

	byte[] bytes = new byte[200];

	public void SendTestData(){
		if(network.Initialized){
			network.SendData(bytes, 200);
		}
	}
	

	void Update(){
		if(network.Initialized){
			int byteCount = network.ReceiveData();
			if(byteCount > 0){
				Debug.Log(byteCount);
			}
		}
	}

	void OnDisable(){
		network.Dispose();
	}
}
