using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[System.Serializable]
public abstract class FieldData {
	
	byte fieldId;  //- for networking

	///just the type used for deserialization?
	public abstract FieldType Type{get;}
	public abstract string ToString();
	//public abstract byte this[int index]{get;}
	//public abstract int length{get;}
	//public byte[] GetBytes(){

	//}

	public abstract bool TryParse(string value);
}



[System.Serializable]
public class ByteData : FieldData{
	public byte value;

	public override FieldType Type{
		get{ return FieldType.Byte; }
	}

	public override string ToString(){
		return value.ToString();		
	}

	public override bool TryParse(string text){
		byte val;
		bool success = System.Byte.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}

}
[System.Serializable]
public class UInt16Data : FieldData{
	public ushort value;

	public override FieldType Type{
		get{ return FieldType.UInt16; }
	}
	public override string ToString(){
		return value.ToString();			
	}

	public override bool TryParse(string text){
		ushort val;
		bool success = System.UInt16.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class UInt32Data : FieldData{
	public uint value;

	public override FieldType Type{
		get{ return FieldType.UInt32; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		uint val;
		bool success = System.UInt32.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class UInt64Data : FieldData{
	public ulong value;

	public override FieldType Type{
		get{ return FieldType.UInt64; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		ulong val;
		bool success = System.UInt64.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}

[System.Serializable]
public class SByteData : FieldData{
	public sbyte value;

	public override FieldType Type{
		get{ return FieldType.SByte; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		sbyte val;
		bool success = System.SByte.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class Int16Data : FieldData{
	public short value;

	public override FieldType Type{
		get{ return FieldType.Int16; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		short val;
		bool success = System.Int16.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class Int32Data : FieldData{
	public int value;

	public override FieldType Type{
		get{ return FieldType.Int32; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		int val;
		bool success = System.Int32.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class Int64Data : FieldData{
	public long value;

	public override FieldType Type{
		get{ return FieldType.Int64; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		long val;
		bool success = System.Int64.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class SingleData : FieldData{
	public float value;

	public override FieldType Type{
		get{ return FieldType.Single; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		float val;
		bool success = System.Single.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class DoubleData : FieldData{
	public double value;

	public override FieldType Type{
		get{ return FieldType.Double; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		double val;
		bool success = System.Double.TryParse(text, out val);
		if(success){
			value = val;
		}
		return success;
	}
}
[System.Serializable]
public class StringData : FieldData{
	public string value;
	public override FieldType Type{
		get{ return FieldType.String; }
	}
	public override string ToString(){
		return value;		
	}
	public override bool TryParse(string text){
		
		value = text;
		
		return true;
	}
}



/*
[System.Serializable]
public class IntArrayData : FieldData{
	public int[] value;
	public override FieldType Type{
		get{ return FieldType.Int32_Array; }
	}
	public override string ToString(){
		return value.ToString();			
	}
	public override bool TryParse(string text){
		///TODO: uh, yeah, real parsing, maybe?
		return false;
	}
}*/