using UnityEditor;
using UnityEngine;

public class ProjectNavigationHelperEditorWindow : EditorWindow
{
    [MenuItem("Tools/Project Navigation Helper Settings")]
    public static void ShowWindow()
    {
        GetWindow<ProjectNavigationHelperEditorWindow>("Project Navigation Helper Settings");
    }

    private void OnGUI()
    {
        GUILayout.Label("Settings for Project Navigation Helper", EditorStyles.boldLabel);
        ProjectNavigationHelper.CanPing = EditorGUILayout.Toggle("Enable History Update", ProjectNavigationHelper.CanPing);
    }
}