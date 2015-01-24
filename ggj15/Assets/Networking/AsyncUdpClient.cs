using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Icosahedra.Net{



///This class contains all the theaded nastiness
[System.Serializable]
public class AsyncUdpClient : System.IDisposable{

	public enum NetworkStatus{None, Active}

    private static readonly IPAddress multicastIP = new IPAddress(new byte[4]{239,255,255,255});
    int listenPort = 8051;
    IPEndPoint localEndPoint;
	IPEndPoint multicastEndPoint;



	////These methods are sync
	public int ReceiveData(byte[] buffer){
		int returnValue = 0;
		lock(receiveLock){
			for(int i=0; i<receiveBufferSize; i++){
				buffer[i] = receiveBuffer[i];
			}
			returnValue = receiveBufferSize;
		}
		return returnValue;
	}

	public void SendData(byte[] buffer, int size){
		lock(sendLock){
			sendBufferSize = size;
			for(int i=0; i<sendBufferSize; i++){
				sendBuffer[i] = buffer[i];
			}
		}
	}
	
	public void Initialize(){
		status = NetworkStatus.Active;
		localEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
		multicastEndPoint = new IPEndPoint(multicastIP, listenPort);
		StartNetworkingThread();
	}

	void StartNetworkingThread(){
		Thread listenerThread = new Thread(new ThreadStart(InitializeNetwork));
		listenerThread.Start();


	}


	public void Dispose(){
		status = NetworkStatus.None;
	}

	private NetworkStatus status = NetworkStatus.None;

	private Object receiveLock = new Object();
	private byte[] receiveBuffer = new byte[10000];
	private int receiveBufferSize = 10000;

	private Object sendLock = new Object();
    private byte[] sendBuffer = new byte[10000];
    private int sendBufferSize = 10000;

    ////Networking Thread
    /* **************   */
    BufferedUdpClient udpClient;
    
    const int NETWORK_SLEEP = 100; //100 ms



    private void InitializeNetwork(){
    	udpClient = new BufferedUdpClient(localEndPoint);//localEndPoint
    	udpClient.MulticastLoopback = true;
		udpClient.JoinMulticastGroup(multicastIP);
    	NetworkLoop();
    }

    private void NetworkLoop(){
    	while(status == NetworkStatus.Active){
    		//Debug.Log(status);
    		udpClient.Receive();

    		SendSocketData();
    		int sent = udpClient.SendTo(multicastEndPoint);
    		//Debug.Log(sent + " bytes sent");
    		Thread.Sleep(NETWORK_SLEEP);
    	}
    	
    	udpClient.Close();
    }
    


	private void ReadDataFromSocket(){
		lock(receiveLock){
			receiveBufferSize = udpClient.receiveBufferSize;
			for(int i=0; i<receiveBufferSize; i++){
				receiveBuffer[i] = udpClient.receiveBuffer[i];
			}
		}
    }

    private void SendSocketData(){
		lock(sendLock){
			udpClient.sendBufferSize = sendBufferSize;
			for(int i=0; i<sendBufferSize; i++){
				udpClient.sendBuffer[i] = sendBuffer[i];
			}
		}
    }
}




}