using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;

public class UDPTest : MonoBehaviour {
	// Socket s;
	UdpClient server;
	UdpClient client;
	static int listenPort = 8051;
	static IPEndPoint localEP;

	IAsyncResult result;
	// Thread mReceiveThread;

	// Use this for initialization
	void Start () {
		// s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp );
		localEP = new IPEndPoint(IPAddress.Loopback, listenPort);
		server = new UdpClient(listenPort);
		result = server.BeginReceive(null, null);
		// ReceiveMessages();
		
		client = new UdpClient();

		// mReceiveThread = new Thread(new ThreadStart(ReceiveData));
   		// mReceiveThread.IsBackground = true;
    	// mReceiveThread.Start();
   }
    
//    private void ReceiveData() {
//     try {
//     	IPEndPoint mListener = new IPEndPoint(IPAddress.Loopback, 0);
//         //  Setup UDP client.
//        UdpClient mClient = new UdpClient(30020);
//         mClient.Client.ReceiveTimeout = 250;

//         //  While thread is still alive.
//         while(Thread.CurrentThread.IsAlive) {
//             try {
//                 //  Grab the data.
//                 byte[] data = mClient.Receive(ref mListener);
//                 Debug.Log(data.Length);
//                 //  Convert the data.
//                 /* REDACTED */

//                 //  Separate out the data.
//                 /* REDACTED */

//                 //  Store data in the DataSource.
//                 /* REDACTED */
//             } catch(SocketException e) {
//                 Debug.Log(e.ToString());
//                 continue;
//             }
//         }
//     } catch(Exception e) {
//         Debug.Log(e.ToString());
//     }
// }
     
	
	
	// Update is called once per frame
	void Update () {
		Byte [] sendBytes = Encoding.ASCII.GetBytes("Hello, world world");
	  	client.Send(sendBytes, sendBytes.Length, localEP);
		
		IPEndPoint e = new IPEndPoint(IPAddress.Any, listenPort);
	  	if(result.IsCompleted){
	  		Debug.Log("msg");
	  		Byte[] receiveBytes = server.EndReceive(result, ref e);
	  		Debug.Log(receiveBytes.Length);
	  	}

		if(server.Client.Poll(-1, SelectMode.SelectWrite)){
			System.Console.WriteLine("This Socket is writable.");
		}
		else if (server.Client.Poll(-1, SelectMode.SelectRead)){
			System.Console.WriteLine("This Socket is readable." );
		}
		else if (server.Client.Poll(-1, SelectMode.SelectError)){
			System.Console.WriteLine("This Socket has an error.");
		}
	}

	// public static bool messageReceived = false;

	// static void SendMessage1(IPEndPoint server, string message)
	// {

	//   // create the udp socket
	//   UdpClient u = new UdpClient();

	//   u.Connect(server);
	//   Byte [] sendBytes = Encoding.ASCII.GetBytes(message);
	//   u.Send(sendBytes, sendBytes.Length);
	//   // send the message 
	//   // the destination is defined by the call to .Connect()
	//   // u.BeginSend(sendBytes, sendBytes.Length, 
	//               // new AsyncCallback(SendCallback), u);
	// // Debug.Log("sending message");
	  
	// }
	// public static void SendCallback(IAsyncResult ar)
	// {
	//   UdpClient u = (UdpClient)ar.AsyncState;

	//   Console.WriteLine("number of bytes sent: {0}", u.EndSend(ar));
	//   // messageSent = true;
	// }

	// public static void ReceiveMessages()
	// {
		
	//   // Receive a message and write it to the console.
	//   IPEndPoint e = localEP;
	//   UdpClient u = new UdpClient(e);

	//   UdpState s = new UdpState();
	//   s.e = e;
	//   s.u = u;

	//   Console.WriteLine("listening for messages");
	//   u.BeginReceive(new AsyncCallback(ReceiveCallback), u);

	//   // Do some work while we wait for a message. For this example, 
	//   // we'll just sleep 
	//   // while (!messageReceived)
	//   // {
	//   //   Thread.Sleep(100);
	//   // }
	// }

	// public static void ReceiveCallback(IAsyncResult ar)
	// {
	//   UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
	//   IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

	//   Byte[] receiveBytes = u.EndReceive(ar, ref e);
	//   string receiveString = Encoding.ASCII.GetString(receiveBytes);

	//   Console.WriteLine("Received: {0}", receiveString);
	//   // messageReceived = true;
	// }
}

public class UdpState{
	public UdpClient u;
	public IPEndPoint e;
}
