/*
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace Icosahedra.Net{

[System.Serializable]
public class UDP  {

	BufferedUdpClient client;
	private System.IAsyncResult currentAsyncResult = null;
	IPEndPoint localEndPoint;// = new IPEndPoint(IPAddress.Any, listenPort);
	IPEndPoint multicastEndPoint;
	int listenPort = 8051;


 	//FOR MULTICASTING, use 224.0.1.0 to 239.255.255.255.
	private static readonly IPAddress multicastIP = new IPAddress(new byte[4]{239,255,255,255});
	///IPAddress.Loopback

	byte[] data = new byte[10000];

	bool networkActive = false;

	public void Activate(){
		InitializeClient();
		StartListening();
	}

	

	public void SendNetworkTest(byte[] testData){

		SendMessage(testData);
	}


	void InitializeClient(){

		localEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
		multicastEndPoint = new IPEndPoint(multicastIP, listenPort);

		client = new BufferedUdpClient(localEndPoint);
		client.MulticastLoopback = true;
		client.JoinMulticastGroup(multicastIP);
	}

    private void StartListening(){
    	if(client == null){
    		InitializeClient();
    	}
        networkActive = true;

        //UdpState s = new UdpState(localEndPoint, client);
        currentAsyncResult = client.BeginReceive(new System.AsyncCallback(ReceiveCallback), client);
        
    }

    private void SendMessage(byte[] sendBytes){
    	client.BeginSend(sendBytes, sendBytes.Length, multicastEndPoint,
              new System.AsyncCallback(SendCallback), client);
    }
    private void SendCallback(System.IAsyncResult ar){
		BufferedUdpClient u = (BufferedUdpClient)ar.AsyncState;

		Debug.Log("number of bytes sent: "+ u.EndSend(ar));
		//messageSent = true;
	}

    private void ReceiveCallback(System.IAsyncResult ar)
    {	//Profiler.BeginSample("Receive Callback");
        
        if (ar == currentAsyncResult){
        	BufferedUdpClient c = (BufferedUdpClient)ar.AsyncState;
  			//IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

        	int byteCount = client.EndReceive(ar);
        	c.CopyBuffer(data, byteCount);

        	if ( networkActive ){
        		//UdpState s = new UdpState(e, c);
        		currentAsyncResult = c.BeginReceive(new System.AsyncCallback(ReceiveCallback), c );
        	}
			else {
				client.Close();
			}

			Debug.Log(byteCount);
        }
       // Profiler.EndSample();
    }

  //  private class UdpState : Object{
   //     public UdpState(IPEndPoint e, UdpClient c) { this.e = e; this.c = c; }
  //      public IPEndPoint e;
   //     public UdpClient c;
  //  }

    

}
	
}*/