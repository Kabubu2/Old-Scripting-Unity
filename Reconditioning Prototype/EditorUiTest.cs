using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

//Attempted similification of certain asset creation methods.

public enum testDev { box, egg, cheese }
public class EditorUiTest : MonoBehaviour
{

    [HideInInspector]
    public bool hasPhone;
    [HideInInspector]
    public int number;
    [HideInInspector]
    public testDev items;

}
#if UNITY_EDITOR
[CustomEditor(typeof(EditorUiTest))]
public class EditorUiTest_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorUiTest script = (EditorUiTest)target;

        script.hasPhone = EditorGUILayout.Toggle("Has Phone", script.hasPhone);
        if (script.hasPhone)
        {
            script.number = EditorGUILayout.IntField("Number", script.number, EditorStyles.boldLabel);
            script.items = (testDev)EditorGUILayout.EnumPopup("Items", script.items, EditorStyles.boldLabel);
        }
    }
}
#endif