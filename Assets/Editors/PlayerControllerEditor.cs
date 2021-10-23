using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Short Cuts", EditorStyles.boldLabel);
        
        PlayerController myScript = (PlayerController)target;
        
        if(GUILayout.Button("Dead")) myScript.Dead();
    }
}