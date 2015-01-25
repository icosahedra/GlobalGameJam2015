using UnityEngine;
using System.Collections;
using System.Net;
using Icosahedra.Net;

public enum GameMode{Start, Middle, End, Finished}

public class GameJamManager : MonoBehaviour {

	
	SyncUdpClient network = new SyncUdpClient();
	public UnityEngine.UI.InputField inputField;
	public Canvas canvas;

	public Lighting lightingManager;

	public Bird localBird;
	public Bird networkedBird;

	GameMode currentMode = GameMode.Start;

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


	float birdDistance = 1000;
	public float BirdDistance{
		get{
			return birdDistance;
		}
	}


	void Update(){
		if(network.Initialized){
			int byteCount = network.ReceiveData();
			if(byteCount > 0){
				HideUI();
			//	Debug.Log(byteCount);
			}
		}

		birdDistance = (localBird.transform.position - networkedBird.transform.position).magnitude;


		if(localBird.playerId == 0){
			if(currentMode == GameMode.Start){
				if(birdDistance < 100){
					currentMode = GameMode.Middle;
				}
			}
			else if(currentMode == GameMode.Middle){
				if(birdDistance < 50){
					if(birdDistance < 10){
						lightingManager.UpdateTime( 15  );
					}
					else{
						lightingManager.UpdateTime( 5  );
					}
				}
				

				if(birdDistance > 50){
					if(lightingManager.dayPercent > 0.01f){
						lightingManager.ReverseTime(6);
					}
				}
				if(lightingManager.dayPercent > 0.75f){
					currentMode = GameMode.End;
				}
			}
			else if(currentMode == GameMode.End){
				if(birdDistance < 20){
					lightingManager.UpdateTime( 5  );
				}
				else if(lightingManager.dayPercent > 0.75f){
					lightingManager.ReverseTime(5);
				}
				if(lightingManager.dayPercent > 0.95f){
					currentMode = GameMode.Finished;
				}
			}
			else if(currentMode == GameMode.Finished){
				if(lightingManager.dayPercent < 1f){
					lightingManager.UpdateTime(2);
				}
			}
		}


	}

	void OnDisable(){
		network.Dispose();
	}
}
