using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Icosahedra.IO{
public class BinaryWriter {



	////Integer Write Methods
	public static void Write(byte data, byte[] memoryStream, int startPosition){
		memoryStream[startPosition] = data;
	}
	public static void Write(ushort data, byte[] memoryStream, int startPosition){
		UInt16ToBytes convert = new UInt16ToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
	}
	public static void Write(uint data, byte[] memoryStream, int startPosition){
		UInt32ToBytes convert = new UInt32ToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
		memoryStream[startPosition+2] = convert.byte2;
		memoryStream[startPosition+3] = convert.byte3;
	}
	public static void Write(ulong data, byte[] memoryStream, int startPosition){
		UInt64ToBytes convert = new UInt64ToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
		memoryStream[startPosition+2] = convert.byte2;
		memoryStream[startPosition+3] = convert.byte3;
		memoryStream[startPosition+4] = convert.byte4;
		memoryStream[startPosition+5] = convert.byte5;
		memoryStream[startPosition+6] = convert.byte6;
		memoryStream[startPosition+7] = convert.byte7;
	}
	public static void Write(sbyte data, byte[] memoryStream, int startPosition){
		SbyteToBytes convert = new SbyteToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;

	}
	public static void Write(short data, byte[] memoryStream, int startPosition){
		Int16ToBytes convert = new Int16ToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
	}
	public static void Write(int data, byte[] memoryStream, int startPosition){
		Int32ToBytes convert = new Int32ToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
		memoryStream[startPosition+2] = convert.byte2;
		memoryStream[startPosition+3] = convert.byte3;
	}
	public static void Write(long data, byte[] memoryStream, int startPosition){
		Int64ToBytes convert = new Int64ToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
		memoryStream[startPosition+2] = convert.byte2;
		memoryStream[startPosition+3] = convert.byte3;
		memoryStream[startPosition+4] = convert.byte4;
		memoryStream[startPosition+5] = convert.byte5;
		memoryStream[startPosition+6] = convert.byte6;
		memoryStream[startPosition+7] = convert.byte7;
	}
	public static void Write(char data, byte[] memoryStream, int startPosition){
		CharToBytes convert = new CharToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
	}

	///float write methods
	public static void Write(float data, byte[] memoryStream, int startPosition){
		FloatToBytes convert = new FloatToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
		memoryStream[startPosition+2] = convert.byte2;
		memoryStream[startPosition+3] = convert.byte3;
	}

	public static void Write(double data, byte[] memoryStream, int startPosition){
		DoubleToBytes convert = new DoubleToBytes();
		convert.value = data;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;
		memoryStream[startPosition+2] = convert.byte2;
		memoryStream[startPosition+3] = convert.byte3;
		memoryStream[startPosition+4] = convert.byte4;
		memoryStream[startPosition+5] = convert.byte5;
		memoryStream[startPosition+6] = convert.byte6;
		memoryStream[startPosition+7] = convert.byte7;
	}

	
	////String, prepend length
	public static void Write(string data, byte[] memoryStream, int startPosition){
		int stringCount = data.Length;
		ushort lengthPrefix = (ushort)(2*stringCount);


		UInt16ToBytes convert = new UInt16ToBytes();
		convert.value = lengthPrefix;
		memoryStream[startPosition] = convert.byte0;
		memoryStream[startPosition+1] = convert.byte1;


		int offset = startPosition+4;

		for(int i=0; i<stringCount; i++){
			char c = data[i];
			CharToBytes charConvert = new CharToBytes();
			charConvert.value = c;
			memoryStream[offset] = charConvert.byte0;
				offset++;
			memoryStream[offset] = charConvert.byte1;
				offset++;
		}
	}


	/*public static byte[] INT2LE(int data)
  	{
     byte[] b = new byte[4];
     b[0] = (byte)data;
     b[1] = (byte)((data >> 8) & 0xFF);
     b[2] = (byte)((data >> 16) & 0xFF);
     b[3] = (byte)((data >> 24) & 0xFF);
     return b;
  	}*/

  	[StructLayout(LayoutKind.Explicit)]
	public struct CharToBytes
	{

	    [FieldOffset(0)]
	    public char value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;
	}

  	[StructLayout(LayoutKind.Explicit)]
	public struct FloatToBytes
	{

	    [FieldOffset(0)]
	    public float value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	    [FieldOffset(2)]
	    public byte byte2;

	    [FieldOffset(3)]
	    public byte byte3;
	}
	[StructLayout(LayoutKind.Explicit)]
	public struct DoubleToBytes
	{
		
	    [FieldOffset(0)]
	    public double value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	    [FieldOffset(2)]
	    public byte byte2;

	    [FieldOffset(3)]
	    public byte byte3;

	    [FieldOffset(4)]
	    public byte byte4;

	    [FieldOffset(5)]
	    public byte byte5;

	    [FieldOffset(6)]
	    public byte byte6;

	    [FieldOffset(7)]
	    public byte byte7;
	}
	

	[StructLayout(LayoutKind.Explicit)]
	public struct SbyteToBytes
	{

	    [FieldOffset(0)]
	    public sbyte value;

	    [FieldOffset(0)]
	    public byte byte0;

	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Int16ToBytes
	{

	    [FieldOffset(0)]
	    public short value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	}
	[StructLayout(LayoutKind.Explicit)]
	public struct Int32ToBytes
	{

	    [FieldOffset(0)]
	    public int value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	    [FieldOffset(2)]
	    public byte byte2;

	    [FieldOffset(3)]
	    public byte byte3;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Int64ToBytes
	{

	    [FieldOffset(0)]
	    public long value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	    [FieldOffset(2)]
	    public byte byte2;

	    [FieldOffset(3)]
	    public byte byte3;

	    [FieldOffset(4)]
	    public byte byte4;

	    [FieldOffset(5)]
	    public byte byte5;

	    [FieldOffset(6)]
	    public byte byte6;

	    [FieldOffset(7)]
	    public byte byte7;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct UInt16ToBytes
	{

	    [FieldOffset(0)]
	    public ushort value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	}
	[StructLayout(LayoutKind.Explicit)]
	public struct UInt32ToBytes
	{

	    [FieldOffset(0)]
	    public uint value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	    [FieldOffset(2)]
	    public byte byte2;

	    [FieldOffset(3)]
	    public byte byte3;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct UInt64ToBytes
	{

	    [FieldOffset(0)]
	    public ulong value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;

	    [FieldOffset(2)]
	    public byte byte2;

	    [FieldOffset(3)]
	    public byte byte3;

	    [FieldOffset(4)]
	    public byte byte4;

	    [FieldOffset(5)]
	    public byte byte5;

	    [FieldOffset(6)]
	    public byte byte6;

	    [FieldOffset(7)]
	    public byte byte7;
	}
}

}
