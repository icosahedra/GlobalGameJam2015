using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
public class IPDebug : MonoBehaviour {


	public string LocalIPAddress()
	 {
	   IPHostEntry host;
	   string localIP = "";
	   host = Dns.GetHostEntry(Dns.GetHostName());
	   foreach (IPAddress ip in host.AddressList)
	   {
	     if (ip.AddressFamily == AddressFamily.InterNetwork)
	     {
	       localIP = ip.ToString();
	       break;
	     }
	   }
	   return localIP;
	 }

	public UnityEngine.UI.Text text;
	void Update () {
		text.text = Network.player.ipAddress;//LocalIPAddress();
	}
}
