using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class PersistentDataWindow : EditorWindow{



	[MenuItem ("Window/Persistent Data")]
    public static void ShowWindow () {
      EditorWindow  blah = EditorWindow.GetWindow(typeof(PersistentDataWindow));
        blah.title = "Persistent Data";
    }

    void Update () {
    	if(Application.isPlaying){
    		Repaint();
    	}
    }

    void OnGUI () {
    	EditorGUILayout.BeginHorizontal();
    	//GUILayout.FlexibleSpace();
    	EditorGUILayout.BeginVertical();

        if(PersistentData.Data != null){
        	for(int i=0; i<PersistentData.Data.Count; i++){
        		KeyValuePair<string, ObjectData> kvp = PersistentData.Data[i];

        		EditorGUILayout.LabelField (kvp.Key, EditorStyles.boldLabel);
        		
        		if(kvp.Value != null){
                    EditorGUI.indentLevel++;
                    List< KeyValuePair<string, FieldData> > data = kvp.Value.Data;
                    for(int j=0; j<data.Count; j++){
                        KeyValuePair<string, FieldData> kvpd = data[j];
                        EditorGUILayout.BeginHorizontal("box");
                            
                            EditorGUILayout.LabelField(kvpd.Key + " : <"+ kvpd.Value.Type.ToString()+">");
                            string current = kvpd.Value.ToString();
                            string val = EditorGUILayout.TextField(current, GUILayout.MinWidth(200));
                            if(current != val){
                                //Undo.RecordObject(kvpd, "Edit Global Data");
                                //kvpd.Value.TryParse(val);
                            }
                            
                            
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;
                    
        		}
        		
        	}
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}