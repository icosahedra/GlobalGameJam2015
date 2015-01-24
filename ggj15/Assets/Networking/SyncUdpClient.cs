using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;


namespace Icosahedra.Net{



///This class contains all the theaded nastiness
[System.Serializable]
public class SyncUdpClient : System.IDisposable{

	public enum NetworkStatus{None, Active}

    private static readonly IPAddress multicastIP = new IPAddress(new byte[4]{239,255,255,255});
    int listenPort = 8051;
    CustomIPEndPoint localEndPoint;
	CustomIPEndPoint multicastEndPoint;

	CustomIPEndPoint target;

	bool initialized = false;
	public bool Initialized{
		get{
			return initialized;
		}
	}


	public void Initialize(){
		Initialize(IPAddress.Loopback);
	}

	public void Initialize(IPAddress address){
		status = NetworkStatus.Active;
		localEndPoint = new CustomIPEndPoint(IPAddress.Any, listenPort);

		multicastEndPoint = new CustomIPEndPoint(multicastIP, listenPort);
		target = new CustomIPEndPoint(address, listenPort);

		InitializeNetwork();
		initialized = true;
	}


	public void Dispose(){
		udpClient.Close();
		status = NetworkStatus.None;
	}

	private NetworkStatus status = NetworkStatus.None;

    ////Networking Thread
    /* **************   */
    BufferedUdpClient udpClient;
    


    private void InitializeNetwork(){
    	udpClient = new BufferedUdpClient(localEndPoint);
    	//udpClient.MulticastLoopback = true;
		//udpClient.JoinMulticastGroup(multicastIP);
		
		udpClient.Client.Blocking = false;
		//udpClient.Client.Bind(localEndPoint);
		udpClient.remoteEP = (EndPoint)target;
		//udpClient.Client.Connect(loopback);
    }

    public int ReceiveData(){
		return udpClient.Receive();
	}

	public void SendData(byte[] buffer, int size){
		
		udpClient.sendBufferSize = size;
		for(int i=0; i<size; i++){
			udpClient.sendBuffer[i] = buffer[i];
		}
		int sent = udpClient.SendTo(target);
	}

	
}

///This is here only to cache the socket address to reduce memory consumption
public class CustomIPEndPoint : IPEndPoint{
		SocketAddress cachedAddress;
		public CustomIPEndPoint(IPAddress ip, int port) : base(ip, port){

		}

		public override SocketAddress Serialize(){
			if(cachedAddress == null){
				cachedAddress = base.Serialize();
			}
			return cachedAddress;
		}
	}


    

}



