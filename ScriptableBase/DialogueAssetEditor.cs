
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueAsset))]
public class DialogueAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (DialogueAsset)target;

        if (GUILayout.Button("Preview", GUILayout.Height(20), GUILayout.Width(100)))
        {
            script.SplitFile();
            EditorUtility.SetDirty(script);
        }

    }
}
#endif