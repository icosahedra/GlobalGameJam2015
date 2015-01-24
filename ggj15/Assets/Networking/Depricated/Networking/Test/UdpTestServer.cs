using System;
using System.Net;
using System.Net.Sockets;

namespace Icosahedra.Net{

public class UdpEchoServer {

	public UdpEchoServer(IPEndPoint endPoint){
		// this.endPoint = endPoint;
		server = new UdpClient(endPoint);

	}

	public void StartServer(){
		if(!serverRunning){
			serverRunning = true;
			ServerLoop();
		}
	}

	private bool serverRunning = false;
	private UdpClient server;
	// private IPEndPoint endPoint;

	//Creates an IPEndPoint to record the IP Address and port number of the sender.  
	// The IPEndPoint will allow you to read datagrams sent from any source.
 	IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);



	public void ServerLoop(){
		// while(true){
			try{
				
				byte[] message = server.Receive( ref remoteIpEndPoint );
				server.Send(message, message.Length, remoteIpEndPoint);
				System.Console.WriteLine("Recieved " + message.Length);
			}
			catch(Exception e){
				System.Console.WriteLine(e);
			}
		// }
	}

	~UdpEchoServer()
	{ 
		Console.WriteLine("Closing socket");
	    server.Close();
	}	

}

}
