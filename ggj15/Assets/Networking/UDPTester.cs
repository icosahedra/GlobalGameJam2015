using UnityEngine;
using System.Collections;
using Icosahedra.Net;
using System.IO;

public class UDPTester : MonoBehaviour {


	//AsyncUdpClient udp = new AsyncUdpClient();
	SyncUdpClient udp = new SyncUdpClient();
	MemoryStream memoryStream;
	BinaryWriter writer;

	void Start () {
		udp.Initialize();

		//Initialize a publicly visible memory stream, that is writeable, so the buffer is accessible
		memoryStream = new MemoryStream(testData, 0, testData.Length, true, true);
		//MemoryStream.GetBuffer();
		//writer = new BinaryWriter(memoryStream);


		Icosahedra.IO.BinaryWriter.Int32ToBytes ib = new Icosahedra.IO.BinaryWriter.Int32ToBytes();
		ib.value = -13451345;
		byte[] a = System.BitConverter.GetBytes(ib.value);
		Debug.Log(a[0] + " : " + a[1] + " : " + a[2] + " : " + a[3]);
		Debug.Log(ib.byte0 + " : " + ib.byte1 + " : " + ib.byte2 + " : " + ib.byte3);

		Icosahedra.IO.BinaryWriter.FloatToBytes fb = new Icosahedra.IO.BinaryWriter.FloatToBytes();
		fb.value = -12341234234.1234f;
		a = System.BitConverter.GetBytes(fb.value);
		Debug.Log(a[0] + " : " + a[1] + " : " + a[2] + " : " + a[3]);
		Debug.Log(fb.byte0 + " : " + fb.byte1 + " : " + fb.byte2 + " : " + fb.byte3);

		Icosahedra.IO.BinaryWriter.CharToBytes cb = new Icosahedra.IO.BinaryWriter.CharToBytes();
		cb.value = 'g';
		a = System.BitConverter.GetBytes(cb.value);
		Debug.Log(a[0] + " : " + a[1]);
		Debug.Log(cb.byte0 + " : " + cb.byte1);


		Icosahedra.IO.BinaryWriter.Write("hello, world", testData, 0);

		
		//Debug.Log(System.BitConverter.IsLittleEndian);
	}
	byte[] testData = new byte[1000];
	void Update(){
		///reset the writer to the start of the stream
		//writer.Seek(0, SeekOrigin.Begin);
		//writer.Write(Random.value);
		udp.ReceiveData();
		udp.SendData(testData,100);

		


	}

	void OnDisable(){
		udp.Dispose();
	}
}
