/*
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Icosahedra.Net{

[System.Serializable]
public class UdpSocket  {


    bool listening = false;
    private byte[] message = new byte[100];

    public void Initialize(){

        BeginListening();
    }

    public void ReadBufferData(){
        lock(theLock){
            if(newData){
                for(int i=0; i<messageBuffer.Length; i++){
                    message[i] = messageBuffer[i];
                }
                newData = false;
            }
        }
    }



    void BeginListening(){
        listening = true;
        Thread listenerThread = new Thread(new ThreadStart(Listen));
        listenerThread.Start();
    }





    private static Object theLock = new Object();
    private byte[] messageBuffer = new byte[100];
    private bool newData = false;


    /////// LISTENER THREAD - probably should be a class onto itself.

    private static readonly IPAddress multicastIP = new IPAddress(new byte[4]{239,255,255,255});

    Socket socket;
    private int TIMEOUT = 3000; ///in milliseconds
    byte[] readBuffer = new byte[100];
    byte[] sendBuffer = new byte[100];
    int listenPort = 8051;
    EndPoint senderRemote;
    IPEndPoint multicastEndPoint;

    void InitializeSocket(){
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, TIMEOUT);

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, listenPort);
        senderRemote = (EndPoint)sender;
        multicastEndPoint = new IPEndPoint(multicastIP, listenPort);
        socket.Bind(senderRemote);
        socket.MulticastLoopback = true;

        ///try setting socket.blocking 
    }


    void Listen(){
        InitializeSocket();
        int x = 0;
        while(listening){
            Debug.Log(socket.Available);
            if(socket.Available > 0){

                socket.ReceiveFrom(readBuffer, ref senderRemote);
                Debug.Log("read data");
                for(int i=0; i<readBuffer.Length; i++){
                    x++;

                    readBuffer[i] = (byte)(x%256);
                }
                WriteNetworkData(readBuffer);
            }

            socket.SendTo(sendBuffer,  multicastEndPoint);
            
            Thread.Sleep(50);

        }
    }

    void WriteNetworkData( byte[] readBuffer){
        lock(theLock){
            newData = true;
            for(int i=0; i<readBuffer.Length; i++){
                messageBuffer[i] = readBuffer[i];
            }

        }


    }




}
}


////TOMORROW - TODO - instead of raw sockets, take the subclassed UDPClient, and use that in a blocking, threaded way
//// FIX SERVER SHUTDOWN
//// CHECK MEMORY USAGE ACROSS BOARD


*/