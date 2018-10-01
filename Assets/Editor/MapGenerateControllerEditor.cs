using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MepGenerateController))]
public class MapGenerateControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawGenerateButton();
    }

    void DrawGenerateButton()
    {
        if (GUILayout.Button("Generate"))
        {
            MepGenerateController generator = (MepGenerateController)target;
            generator.GenerateMap();
        }
    }
}
