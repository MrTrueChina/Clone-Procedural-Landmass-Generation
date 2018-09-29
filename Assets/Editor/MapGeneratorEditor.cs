using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]                        //允许多重编辑？允许编辑多个？总之遇到不能多重编辑的时候就加上他。似乎又遇不到了
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawGenerateButton();
    }
    void DrawGenerateButton()
    {
        MapGenerator generator = (MapGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            generator.GenerateMap();
        }
    }
}
