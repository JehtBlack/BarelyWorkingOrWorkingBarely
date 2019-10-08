using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManagerInstance))]
public class GameManagerInstanceEditor : Editor
{
    public override void OnInspectorGUI() {

        GameManagerInstance manager = (GameManagerInstance)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Kill player"))
            manager.KillPlayer();

        if (GUILayout.Button("Toggle Selected Feature"))
            manager.SetUnlockState(manager.UnlockID, !manager.GetUnlockState(manager.UnlockID));
    }
}
