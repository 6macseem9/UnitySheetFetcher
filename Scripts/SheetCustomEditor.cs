#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SheetFetcher),true), CanEditMultipleObjects]
public class SheetCustomEditor : Editor
{
    private bool _foldout;

    public override void OnInspectorGUI()
    {
        var sheet = serializedObject;

        _foldout = EditorGUILayout.Foldout(_foldout, "Sheet Data");

        if (_foldout)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(sheet.FindProperty("SheetsID"));

            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(sheet.FindProperty("SheetName"));
            EditorGUILayout.LabelField(" ", "*Leave blank for first sheet");

            EditorGUILayout.Space(7);

            if (GUILayout.Button("Fetch data"))
                SheetManager.FetchSelected();

            EditorGUILayout.Space(20);

            serializedObject.ApplyModifiedProperties();
        }

        DrawDefaultInspector();
    }
}
#endif