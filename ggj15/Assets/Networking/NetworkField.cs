using UnityEngine;
using System.Collections;

public enum FieldType : byte{
	Byte=0, 
	UInt16=1, 
	UInt32=2, 
	UInt64=3,
	SByte = 4,
	Int16=5, 
	Int32=6, 
	Int64=7,
	Char =8, 
	Single=10, 
	Double=11,

	////Variable Types
	String=12,
	Byte_Array =13,
	UInt16_Array=14, 
	UInt32_Array=15, 
	UInt64_Array=16,
	SByte_Array = 17,
	Int16_Array=18, 
	Int32_Array=19, 
	Int64_Array=20,
	Char_Array =21, 
	Single_Array=22, 
	Double_Array=23,
}
/*
public struct NetworkField {

	////this could just be constructed as 2 bytes, but if we do 3, we can 'persist' these identifiers
	ushort objectId; //2 bytes, unique object id
	byte fieldId;	//1 byte, object field id


	FieldType fieldType; ///1 byte
	ushort fieldLength;	///2 bytes, only used for variable - length types

	///data

	///how to structure time stamps?
}*/

public struct NetworkHeader{
	ushort uniqueApplicationId;  ///uniquely identifies this application from other random nonsense
	ushort sequenceId;  ///the order of the packet
}


public struct NetData{
	ushort networkId;
	///data
}