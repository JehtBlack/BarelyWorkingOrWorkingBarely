using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMapController))]
public class TileMapControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        TileMapController controller = (TileMapController)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Toggle Tileset"))
            controller.SwapTileset();
    }
}
