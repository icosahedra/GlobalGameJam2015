/*
public enum NetworkFieldType{Byte, Short, Int, Long, SByte, UShort, UInt, ULong, Float, Double}

public static class FieldType{
	public const byte Byte = 1;
	public const byte Short = 2;
	public const byte Int = 3;
	public const byte Long = 4;
	public const byte SByte = 5;
	public const byte UShort = 6;
	public const byte UInt = 7;
	public const byte ULong = 8;
	public const byte Float = 9;
	public const byte Double = 10;

}

public interface INetworkField  {


	//the unique networking id of this object
	short FieldID{
		get;
	}
	///The total number of bytes this field requires
	int FieldLength{
		get;
	}
	byte[] Payload{
		get;
	}
	NetworkFieldType NetworkFieldType{
		get;
	}
}

public class IntNetworkField : INetworkField  {

	private short fieldID;
	private byte[] payload = new byte[fieldLength];
	const int fieldLength = 6;

	public IntNetworkField(){
		payload[0] = (byte)NetworkFieldType;
	}

	public short FieldID{
		get{
			return fieldID;
		}
	}
	public int FieldLength{
		get{
			return fieldLength;
		}
	}
	public byte[] Payload{
		get{
			return payload;
		}
	}
	public NetworkFieldType NetworkFieldType{
		get{
			return NetworkFieldType.Int;
		}
	}
	

}
*/