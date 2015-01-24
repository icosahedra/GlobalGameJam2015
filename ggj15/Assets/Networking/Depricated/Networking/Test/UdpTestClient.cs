using System;
using System.Net;
using System.Net.Sockets;

namespace Icosahedra.Net{

public class UdpTestClient {

	public UdpTestClient(IPEndPoint endPoint){
		this.endPoint = endPoint;
		client = new UdpClient();
		message = new byte[3];
		message[0] = 1;
		message[1] = 2;
		message[2] = 3;
	}

	private UdpClient client;
	private IPEndPoint endPoint;
	private byte[] message;

	//Creates an IPEndPoint to record the IP Address and port number of the sender.  
	// The IPEndPoint will allow you to read datagrams sent from any source.
 	IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

	public void SendTestData(){
		try{
			client.Send(message, message.Length, endPoint);

			byte[] reply = client.Receive( ref remoteIpEndPoint );

			System.Console.WriteLine("Recieved " + reply.Length);
		}
		catch(Exception e){
			System.Console.WriteLine(e);
		}

	}

	~UdpTestClient()
	{ 
		Console.WriteLine("Closing socket");
	    client.Close();
	}
}

}
