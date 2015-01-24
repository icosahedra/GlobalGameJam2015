using UnityEngine;
using System.Collections;
using System.Net;
using Icosahedra.Net;

public class ServerTest : MonoBehaviour {

	UdpEchoServer echoServer;
	byte[] ipBytes = new byte[4]{10,0,1,6};
	int port = 7007;

	void Start () {
		IPAddress ipAddress = new IPAddress(ipBytes);
		IPEndPoint endPoint = new IPEndPoint(ipAddress,port);
		echoServer = new UdpEchoServer(endPoint);
		echoServer.StartServer();
	}
	
	void Update(){
		//echoServer.ServerLoop();
	}

}
