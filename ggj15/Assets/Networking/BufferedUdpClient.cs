using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Icosahedra.Net{





public class BufferedUdpClient : UdpClient{

		public BufferedUdpClient(IPEndPoint endPoint) : base(endPoint){
			
		}

		public int Receive(){
			//Debug.Log(Client.Available);
			int count = 0;
            if(Client.Available > 0){

            	try{
            		count = Client.ReceiveFrom(receiveBuffer, ref remoteEP);
                	//count = Client.Receive(receiveBuffer);
                	//Debug.Log(count + " bytes received");
                }
                catch(System.Exception e){
                	Debug.Log(e);
                }

                receiveBufferSize = count;
            }
            return count;
		}

		
		public int Send(){
			if(sendBufferSize > 0){
				int sent = 0;
				try{
					sent = Client.Send(sendBuffer,sendBufferSize, SocketFlags.None);
					//Debug.Log(sent + " bytes sent");
				}
                catch(System.Exception e){
                	Debug.Log(e);
                }
				return sent;
			}
			else{
				return 0;
			}
		}

		public int SendTo(IPEndPoint endPoint){
			if(sendBufferSize > 0){
				int sent = 0;
				try{
					sent = Client.SendTo(sendBuffer,sendBufferSize, SocketFlags.None,  (EndPoint)endPoint);
					//Debug.Log(sent + " bytes sent");
				}
                catch(System.Exception e){
                	Debug.Log(e);
                }
				return sent;
			}
			else{
				return 0;
			}
		}

    	public byte[] receiveBuffer = new byte[1024];
    	public int receiveBufferSize = 0;

    	//508 = 576 MTU - 60 IP - 8 UDP
    	public byte[] sendBuffer = new byte[508];
    	public int sendBufferSize = 0;

 		public EndPoint remoteEP;

    }

}