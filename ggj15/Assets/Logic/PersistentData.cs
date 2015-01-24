using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
/// object type, object name, field id

public enum DataType : byte {None=0, Int32 =1, Single = 2, String = 3, Object = 4};
public enum DataState : byte {None, Changed};

public static class PersistentData{


	static List< KeyValuePair<string, ObjectData> > data = new List< KeyValuePair<string, ObjectData> >();

		#if UNITY_EDITOR
			public static List< KeyValuePair<string, ObjectData> > Data{
				get{ return data;}
			}
		#endif


	public static T GetData<T>(string objectName, string fieldName) where T : FieldData,new(){
		ObjectData osd = GetObjectData(objectName);
		return osd.GetData<T>(fieldName);
	}



	static ObjectData GetObjectData(string objectName){
		ObjectData osd = null;
		for(int i=0; i<data.Count; i++){
			if(data[i].Key == objectName){
				osd = data[i].Value;
			}
		}

		///we didn't find the object, so create it
		if(osd == null){
			osd = new ObjectData();
			data.Add(new KeyValuePair<string, ObjectData>(objectName, osd));
		}

		return osd;
	}


	public static void Save(){
		//string fileName = Application.persistentDataPath + "/savegame";
		//WriteBinaryData(fileName);

		string fileName = Application.persistentDataPath + "/savegame.xml";
		WriteTextData(fileName);
	}

	public static void Load(){
		//string fileName = Application.persistentDataPath + "/savegame";
		//LoadData(fileName);

		string fileName = Application.persistentDataPath + "/savegame.xml";
		LoadTextData(fileName);
	}

	/*
	static void LoadData(string fileName){
		if (File.Exists(fileName))
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
            	int length = (int)reader.BaseStream.Length;
           		string currentObjectName = "";
            	while (reader.BaseStream.Position != length){
            		ReadObject(reader, ref currentObjectName);
            	}
            
            }
        }
	}


	static void ReadObject(BinaryReader reader, ref string objectName){
		
		DataType nextType = (DataType)reader.ReadByte();
		if(nextType == DataType.Object){
			objectName = reader.ReadString();
		}
		if(nextType == DataType.Int32){
			string fieldName = reader.ReadString();
			int value = reader.ReadInt32();
			//Debug.Log("int32 " + objectName + " : " + fieldName + " : " + value);
			IntData id = GetIntData(objectName, fieldName, value);
			id.value = value;
		}
		else if(nextType == DataType.Single){
			string fieldName = reader.ReadString();
			float value = reader.ReadSingle();
			//Debug.Log("float " + objectName + " : " + fieldName + " : " + value);
			FloatData fd = GetFloatData(objectName, fieldName, value);
			fd.value = value;
		}
		else if(nextType == DataType.String){
			string fieldName = reader.ReadString();
			string value = reader.ReadString();
			//Debug.Log("string " + objectName + " : " + fieldName + " : " + value);
			StringData sd = GetStringData(objectName, fieldName, value);
			sd.value = value;
		}

	}


 ///File.WriteAllBytes(string path, byte[])

	static void WriteBinaryData(string fileName){
		using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
        {
		    	for(int i=0; i<data.Count; i++){

					writer.Write( (byte)DataType.Object);
					writer.Write(data[i].Key);

					ObjectData osd = data[i].Value;

					
					DataStore<IntData> intData= osd.IntegerData;
					if(intData.Data != null && intData.Data.Count > 0){
						writer.Write( (byte) DataType.Int32);
						for(int j=0; j<intData.Data.Count; j++){
							writer.Write(intData.Data[j].Key);
							writer.Write(intData.Data[j].Value.value);
						}
					}

					DataStore<FloatData> floatData= osd.FloatData;
					if(floatData.Data != null && floatData.Data.Count > 0){
						writer.Write( (byte) DataType.Single);
						for(int j=0; j<floatData.Data.Count; j++){
							writer.Write(floatData.Data[j].Key);
							writer.Write(floatData.Data[j].Value.value);
						}
					}

					DataStore<StringData> stringData= osd.StringData;
					if(stringData.Data != null && stringData.Data.Count > 0){
						writer.Write( (byte) DataType.String);
						for(int j=0; j<stringData.Data.Count; j++){
							writer.Write(stringData.Data[j].Key);
							writer.Write(stringData.Data[j].Value.value);
						}
					}
				}
        }
		
	}*/

	const string INDENT = "  ";
	const string INDENT_2 = "    ";
	const string INDENT_3 = "      ";

	static void WriteTextData(string fileName){

		using (StreamWriter writer = new StreamWriter(File.Open(fileName, FileMode.Create)))
        {

        		writer.Write("<save>");
        		writer.WriteLine();
		    	for(int i=0; i<data.Count; i++){

		    		writer.Write(INDENT);
		    		writer.Write("<object name=\"");
						writer.Write(data[i].Key);
					writer.Write("\">");
					writer.WriteLine();

					ObjectData osd = data[i].Value;
					List< KeyValuePair<string, FieldData> > fields = osd.Data;
					for(int j=0; j<fields.Count; j++){
						writer.Write(INDENT_2);
						writer.Write( "<field key=\"");
						writer.Write(fields[j].Key);
						writer.Write( "\" type=\"");
						writer.Write(fields[j].Value.Type);
						writer.Write( "\" value=\"");
						writer.Write(fields[j].Value.ToString());
						writer.Write("\"/>");
						writer.WriteLine();
					}
					writer.Write(INDENT);
					writer.Write("</object>");
					writer.WriteLine();



				}
				writer.Write("</save>");
        }
		
	}

//

	static void LoadTextData(string fileName){
		if (File.Exists(fileName))
        {

        	System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
			settings.ConformanceLevel = System.Xml.ConformanceLevel.Fragment;
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			
			
            using (FileStream fs = (File.Open(fileName, FileMode.Open)))
            {
            	System.Xml.XmlReader reader = System.Xml.XmlReader.Create(fs, settings);
            	string objectName = "";
            	while (reader.Read())
		        {
		            switch (reader.NodeType)
		            {
		                case System.Xml.XmlNodeType.Element:
		                    
		                    switch (reader.Name){
		                    	case "object":
		                    		objectName = reader.GetAttribute("name");
		                    		//Debug.Log("element" + reader.Name + reader.AttributeCount);
		                    	break;

		                    	case "field":
		                    		string key = reader.GetAttribute("key");
		                    		FieldType type = (FieldType)System.Enum.Parse(typeof(FieldType), reader.GetAttribute("type"));
		                    		string value = reader.GetAttribute("value");
		                    		bool success;
		                    		switch(type){
		                    			case FieldType.Byte:
		                    				ByteData byteData = GetData<ByteData>(objectName, key);
		                    				byteData.TryParse(value);
		                    				break;

		                    			case FieldType.UInt16:
		                    				UInt16Data uint16Data = GetData<UInt16Data>(objectName, key);
		                    				uint16Data.TryParse(value);
		                    				break;

		                    			case FieldType.UInt32:
		                    				UInt32Data uint32Data = GetData<UInt32Data>(objectName, key);
		                    				uint32Data.TryParse(value);
		                    				break;

		                    			case FieldType.UInt64:
		                    				UInt64Data uint64Data = GetData<UInt64Data>(objectName, key);
		                    				uint64Data.TryParse(value);
		                    				break;

		                    			case FieldType.SByte:
		                    				SByteData sbyteData = GetData<SByteData>(objectName, key);
		                    				sbyteData.TryParse(value);
		                    				break;

		                    			case FieldType.Int16:
		                    				Int16Data int16Data = GetData<Int16Data>(objectName, key);
		                    				int16Data.TryParse(value);
		                    				break;

		                    			case FieldType.Int32:
		                    				Int32Data int32Data = GetData<Int32Data>(objectName, key);
		                    				int32Data.TryParse(value);
		                    				break;

		                    			case FieldType.Int64:
		                    				Int64Data int64Data = GetData<Int64Data>(objectName, key);
		                    				int64Data.TryParse(value);
		                    				break;

		                    			case FieldType.Single:
		                    				SingleData singleData = GetData<SingleData>(objectName, key);
		                    				singleData.TryParse(value);
		                    				break;

		                    			case FieldType.Double:
		                    				DoubleData doubleData = GetData<DoubleData>(objectName, key);
		                    				doubleData.TryParse(value);
		                    				break;

		                    			case FieldType.String:
		                    				StringData stringData = GetData<StringData>(objectName, key);
		                    				stringData.TryParse(value);
		                    				break;
		                    		}
		                    		
		                    	break;

		                    }
		                    break;
		                //case System.Xml.XmlNodeType.Text:
		                   // Debug.Log("text" + reader.Value);
		              //      break;

		               // case System.Xml.XmlNodeType.EndElement:
		                    //writer.WriteFullEndElement();
		               //     break;
		            }
		        }
            
            }
        }
	}



}



public class ObjectData{

	List< KeyValuePair<string, FieldData> > data = new List< KeyValuePair<string, FieldData> >();
	//#if UNITY_EDITOR
	///?TODO Fix access
		public List< KeyValuePair<string, FieldData> > Data{
			get{
				return data;
			}
		}
	//#endif

	public T GetData<T>(string fieldName) where T : FieldData, new(){
		for(int i=0; i<data.Count; i++){
			if(fieldName == data[i].Key){
				T result = data[i].Value as T;
				if(result == null){
					Debug.LogError("Incorrect Type, Field Exists");
				}
				return result;
			}
		}
		T datum = new T();
		KeyValuePair<string,FieldData> newEntry = new KeyValuePair<string,FieldData>(fieldName, datum);
		data.Add(newEntry);

		return datum;
	}

}









/*

public class DataStore<T> where T : Data, new(){
	List< KeyValuePair<string, T> > data;
	#if UNITY_EDITOR
		public List< KeyValuePair<string, T> > Data{
			get{
				return data;
			}
		}
	#endif

	public T GetData(string fieldName) {
		
		if(data == null){
			data = new List< KeyValuePair<string, T> >();
		}

		for(int i=0; i<data.Count; i++){
			if(fieldName == data[i].Key){
				return data[i].Value;
			}
		}

		///Since we didn't find anything, add a record
		T datum = new T();
		KeyValuePair<string,T> newEntry = new KeyValuePair<string,T>(fieldName, datum);
		data.Add(newEntry);

		return datum;
	}


}

[StructLayout(LayoutKind.Explicit)]
	public struct CharB
	{

		public byte this[int index]{
			get{
				return 0;
				switch(index){
					case 0:
						return byte0;
					case 1:
						return byte1;	
					default:
						throw new System.IndexOutOfRangeException();
				}
			}
		}
		//unsafe{
			//fixed byte backing[2];
		//}
	    [FieldOffset(0)]
	    public char value;

	    [FieldOffset(0)]
	    public byte byte0;

	    [FieldOffset(1)]
	    public byte byte1;
	}
[System.Serializable]
public class Vector3Field{

	private Vector3Data data;
	private Vector3 defaultValue;

	public Vector3Field(string objectName, string fieldName, Vector3 defaultValue){
		data = PersistentData.GetVector3Data(objectName, fieldName, defaultValue);		
	}

	public Vector3 Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}

}
[System.Serializable]
public class QuaternionField{

	private QuaternionData data;
	private Quaternion defaultValue;

	public QuaternionField(string objectName, string fieldName, Quaternion defaultValue){
		data = PersistentData.GetQuaternionData(objectName, fieldName, defaultValue);		
	}

	public Quaternion Value{
		get{
			return data.value;
		}
		set{
			data.value = value;
		}
	}

}*/
