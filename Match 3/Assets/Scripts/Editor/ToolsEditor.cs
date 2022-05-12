using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(ToolsEditor))]
public class ToolsEditor : EditorWindow
{
    public UnityEngine.Object go = null;

    [MenuItem("MyTools/EditorTest")]
    public static void ConfigDialog() {
        EditorWindow.GetWindow(typeof(ToolsEditor));
    }


    void OnGUI() {

        GUILayout.Label("Label Test", EditorStyles.boldLabel);
        go = EditorGUILayout.ObjectField(go, typeof(UnityEngine.Object), true);

        if (GUILayout.Button("Show")) {
            Debug.Log(go.name);
        }
    }

}
