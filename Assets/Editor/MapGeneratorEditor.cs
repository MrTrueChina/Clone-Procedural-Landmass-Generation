using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator generator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            generator.GenerateMap();
        }

        if (GUILayout.Button("Generator"))
        {
            generator.GenerateMap();
        }
    }
}
