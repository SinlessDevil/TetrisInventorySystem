using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public class ProjectNavigationHelper
{
    private const string AssetsPath = "Assets";

    private static List<string> _history = new() { AssetsPath };
    private static int _currentIndex = 1;
    private static bool _isClicked;
    private static int _lastClickedButton;
    private static string _lastPath = AssetsPath;
    private static string _currentPath;
    private static Event _currentEvent;

    private static bool _isFirstRun;
    private static string _lastKnownPath;

    static ProjectNavigationHelper()
    {
        EditorApplication.update += UpdateDirectory;
        EditorApplication.projectWindowItemOnGUI += (_, _) => DetectMouseButtonsPress();
    }

    public static bool CanPing;

    private static void UpdateHistoryForCurrentPath()
    {
        _currentPath = GetCurrentProjectPath();

        if (!_isFirstRun && _currentPath == _lastKnownPath) return;

        if (_history.Count == 0 || _currentPath != _history[^1])
        {
            var paths = GetParentPaths(_currentPath);

            if (paths.Count > _history.Count)
            {
                _history = paths;
                _currentIndex = _history.Count - 1;
            }

            _lastKnownPath = _currentPath;

            if (_isFirstRun)
                _isFirstRun = false;
        }
    }

    private static List<string> GetParentPaths(string path)
    {
        List<string> paths = new List<string> { "Assets" };

        while (!string.IsNullOrEmpty(path) && path.Contains("/") && path != "Assets")
        {
            paths.Insert(1, path);
            path = path.Substring(0, path.LastIndexOf('/'));
        }

        return paths;
    }


    private static void UpdateDirectory()
    {
        UpdateHistoryForCurrentPath();

        if (_lastPath == _currentPath) return;

        _lastPath = _currentPath;
        int depth = GetPathDepth(_currentPath);

        if (_history.Count > depth)
        {
            if (_history[depth] != _currentPath)
            {
                _history[depth] = _currentPath;
                _history.RemoveRange(depth + 1, _history.Count - (depth + 1));
            }
        }
        else
        {
            _history.Add(_currentPath);
        }

        _currentIndex = depth + 1;
    }

    private static int GetPathDepth(string path) => path.Split('/').Length - 1;

    private static void DetectMouseButtonsPress()
    {
        _currentEvent = Event.current;
        if (_currentEvent.type == EventType.MouseDown && !_isClicked)
        {
            _isClicked = true;
            _lastClickedButton = _currentEvent.button;

            if (_lastClickedButton == 3)
            {
                GoBack();
                _currentEvent.Use();
            }

            if (_lastClickedButton == 4)
            {
                GoForward();
                _currentEvent.Use();
            }
        }
        else if (_currentEvent.type == EventType.MouseUp && _currentEvent.button == _lastClickedButton)
        {
            _isClicked = false;
        }
    }

    private static void GoBack()
    {
        if (_currentIndex <= 1) return;
        --_currentIndex;

        if (CanPing)
            PingObject(_history[_currentIndex]);
        else
            OpenDirectory(_history[--_currentIndex]);
    }

    private static void GoForward()
    {
        if (_history.Count <= _currentIndex) return;
        OpenDirectory(_history[_currentIndex++]);
    }

    private static void PingObject(string path)
    {
        var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
        if (obj == null) return;
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
    }

    private static void OpenDirectory(string path)
    {
        var asset = AssetDatabase.LoadMainAssetAtPath(path);

        if (asset == null) return;

        var projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
        var lastBrowser = projectBrowserType
            ?.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public)
            ?.GetValue(null);
        var showFolderMethod = projectBrowserType
            ?.GetMethod("ShowFolderContents", BindingFlags.NonPublic | BindingFlags.Instance);

        showFolderMethod?.Invoke(lastBrowser, new object[] { asset.GetInstanceID(), true });
    }

    private static string GetCurrentProjectPath()
    {
        var tryGetPathMethod =
            typeof(ProjectWindowUtil).GetMethod("TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        object[] args = { null };
        return tryGetPathMethod != null && (bool)tryGetPathMethod.Invoke(null, args) ? (string)args[0] : string.Empty;
    }
}