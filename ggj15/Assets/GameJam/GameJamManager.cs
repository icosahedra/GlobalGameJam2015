using UnityEngine;
using System.Collections;
using System.Net;
using Icosahedra.Net;

public class GameJamManager : MonoBehaviour {

	
	SyncUdpClient network = new SyncUdpClient();
	public UnityEngine.UI.InputField inputField;
	public Canvas canvas;

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
				PlayerPrefs.SetString("lastIP",ipad.text);
				network.Initialize(ip);
			}
		}
	}	

	byte[] bytes = new byte[200];


	void Start(){
		Application.targetFrameRate = 60;
		if(PlayerPrefs.HasKey("lastIP")){
			inputField.text = PlayerPrefs.GetString("lastIP");
		}
	}


	public void SendData(byte[] data){
		if(network.Initialized){
			network.SendData(data, data.Length);
		}
	}

	public void SendTestData(){



		if(network.Initialized){
			network.SendData(bytes, 200);
		}

	}
	public byte[] GetNetData(){
		if(network.Initialized){
			return network.GetBuffer();
		}
		return null;
	}

	void HideUI(){
		canvas.enabled = false;
	}

	void Update(){
		if(network.Initialized){
			int byteCount = network.ReceiveData();
			if(byteCount > 0){
				HideUI();
			//	Debug.Log(byteCount);
			}
		}
	}

	void OnDisable(){
		network.Dispose();
	}
}
