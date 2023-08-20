#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneOpener : EditorWindow {
    Vector2 scrollpos;
    List<sceneData> allScenes;
    string projectPath;
    private string query = "";

    [MenuItem ("Tools/Scene Opener %`")]
    public static void SceneOpenerWizard () {
        SceneOpener sowindow = (SceneOpener) GetWindow (typeof (SceneOpener));
        sowindow.minSize = new Vector2 (280, 250);
    }

    private void OnEnable () {
        projectPath = Application.dataPath;
        RefreshList ();
    }

    private void OnProjectChange () {
        RefreshList ();
        Repaint ();
    }

    private void RefreshList () {
        allScenes = new List<sceneData> ();
        string[] paths = Directory.GetFiles (projectPath, "*.unity", SearchOption.AllDirectories);
        foreach (var Sc in paths) {
            sceneData scd = new sceneData ();
            scd.relativePath = Sc.Substring (Application.dataPath.Length - 6);
            scd.name = scd.relativePath.Substring (scd.relativePath.LastIndexOfAny (new [] { '/', '\\' }) + 1)
                .Replace (".unity", "");
            // if (scd.name.Contains("game_scene") || scd.name == "UtilityScene" || scd.name.Contains("lobby_scene")) {
            allScenes.Add (scd);
            // }
        }

        allScenes = allScenes.OrderBy (_x => _x.name).ToList ();
    }

    private void OnGUI () {
        bool needsRepaint = false;
        EditorGUILayout.BeginHorizontal ("Toolbar"); {
            if (GUILayout.Button ("Refresh", "ToolbarButton", GUILayout.Width (50))) {
                RefreshList ();
            }
            GUILayout.Space (5);

            Rect textFieldRect = EditorGUILayout.GetControlRect (false, 17, "ToolbarSeachTextField", GUILayout.MinWidth (100),
                GUILayout.ExpandWidth (true));
            query = GUI.TextField (textFieldRect, query, "ToolbarSeachTextField");
            string styeName = "ToolbarSeachCancelButton";
            if (string.IsNullOrEmpty (query) || string.IsNullOrWhiteSpace (query)) {
                styeName = "ToolbarSeachCancelButtonEmpty";
            }

            if (GUILayout.Button ("", styeName)) {
                query = "";
                needsRepaint = true;
            }
        }
        EditorGUILayout.EndHorizontal ();
        bool filter = !string.IsNullOrEmpty (query);

        scrollpos = EditorGUILayout.BeginScrollView (scrollpos, false, false); {
            foreach (var scene in allScenes) {
                if (filter && !scene.name.StartsWith (query)) {
                    continue;
                }
                EditorGUILayout.BeginVertical ("Box"); {
                    EditorGUILayout.BeginHorizontal (); {
                        bool shrinkText = Screen.width < 300;
                        int btnSize = shrinkText ? 20 : 40;
                        GUIStyle style = new GUIStyle (EditorStyles.boldLabel);
                        EditorGUILayout.LabelField (scene.name, EditorStyles.boldLabel, GUILayout.MinWidth (100));
                        if (GUILayout.Button (shrinkText ? "P" : "Ping", GUILayout.Width (btnSize))) {
                            Debug.Log (scene.name);
                            EditorGUIUtility.PingObject (AssetDatabase.LoadAssetAtPath (scene.relativePath,
                                typeof (SceneAsset)));
                        }

                        if (GUILayout.Button (shrinkText ? "O" : "Open", GUILayout.Width (btnSize))) {
                            if (EditorSceneManager.GetActiveScene ().isDirty) {
                                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
                            }

                            EditorSceneManager.OpenScene (scene.relativePath, OpenSceneMode.Single);
                        }

                        if (GUILayout.Button (shrinkText ? "A" : "Add", GUILayout.Width (btnSize))) {
                            if (EditorSceneManager.GetActiveScene ().isDirty) {
                                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
                            }

                            EditorSceneManager.OpenScene (scene.relativePath, OpenSceneMode.Additive);
                        }
                    }
                    EditorGUILayout.EndHorizontal ();
                    GUIStyle pathTextStyle = new GUIStyle ();
                    pathTextStyle.wordWrap = true;
                    pathTextStyle.fontSize = 10;
                    EditorGUILayout.LabelField (scene.relativePath, pathTextStyle);
                }
                EditorGUILayout.EndVertical ();
            }
        }
        EditorGUILayout.EndScrollView ();

        if (needsRepaint) {
            Repaint ();
        }
    }
}

public struct sceneData {
    public string name;
    public string relativePath;
}

#endif