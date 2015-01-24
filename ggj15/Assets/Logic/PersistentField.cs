using UnityEngine;
using System.Collections;

public abstract class PersistentField  {

	protected string objectName;
	protected string fieldName;
}


public class SByteField : PersistentField{

	private SByteData data;
	private sbyte defaultValue;

	public SByteField(string objectName, string fieldName, sbyte defaultValue){
		data = PersistentData.GetData<SByteData>(objectName, fieldName);	

	}

	public sbyte Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}
public class Int16Field : PersistentField{

	private Int16Data data;
	private short defaultValue;

	public Int16Field(string objectName, string fieldName, short defaultValue){
		data = PersistentData.GetData<Int16Data>(objectName, fieldName);		
	}

	public short Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class Int32Field : PersistentField{

	private Int32Data data;
	private int defaultValue;

	public Int32Field(string objectName, string fieldName, int defaultValue){
		//this.objectName = objectName;
		//this.name = fieldName;
		data = PersistentData.GetData<Int32Data>(objectName, fieldName);		
	}

	public int Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class Int64Field : PersistentField{

	private Int64Data data;
	private long defaultValue;

	public Int64Field(string objectName, string fieldName, long defaultValue){
		data = PersistentData.GetData<Int64Data>(objectName, fieldName);		
	}

	public long Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class ByteField : PersistentField{

	private ByteData data;
	private byte defaultValue;

	public ByteField(string objectName, string fieldName, byte defaultValue){
		data = PersistentData.GetData<ByteData>(objectName, fieldName);		
	}

	public byte Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class UInt16Field : PersistentField{

	private UInt16Data data;
	private ushort defaultValue;

	public UInt16Field(string objectName, string fieldName, ushort defaultValue){
		data = PersistentData.GetData<UInt16Data>(objectName, fieldName);		
	}

	public ushort Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class UInt32Field : PersistentField{

	private UInt32Data data;
	private uint defaultValue;

	public UInt32Field(string objectName, string fieldName, uint defaultValue){
		//this.objectName = objectName;
		//this.name = fieldName;
		data = PersistentData.GetData<UInt32Data>(objectName, fieldName);		
	}

	public uint Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class UInt64Field : PersistentField{

	private UInt64Data data;
	private ulong defaultValue;

	public UInt64Field(string objectName, string fieldName, ulong defaultValue){
		data = PersistentData.GetData<UInt64Data>(objectName, fieldName);		
	}

	public ulong Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class SingleField : PersistentField{

	private SingleData data;
	private float defaultValue;

	public SingleField(string objectName, string fieldName, float defaultValue){
		data = PersistentData.GetData<SingleData>(objectName, fieldName);		
	}

	public float Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class DoubleField : PersistentField{

	private DoubleData data;
	private double defaultValue;

	public DoubleField(string objectName, string fieldName, double defaultValue){
		data = PersistentData.GetData<DoubleData>(objectName, fieldName);		
	}

	public double Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}
}

public class StringField : PersistentField{

	private StringData data;
	private string defaultValue;

	public StringField(string objectName, string fieldName, string defaultValue){
		data = PersistentData.GetData<StringData>(objectName, fieldName);		
	}

	public string Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}

}

/*
[System.Serializable]
public class IntArrayField : PersistentField{

	private IntArrayData data;
	private string defaultValue;

	public IntArrayField(string objectName, string fieldName, int[] defaultValue){
		data = PersistentData.GetData<IntArrayData>(objectName, fieldName);		
	}

	public int[] Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}

}*/