using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// SceneViewWindow class.
/// </summary>
public class SceneViewWindow : EditorWindow
{
    /// <summary>
    /// Tracks scroll position.
    /// </summary>
    private Vector2 scrollPos;

    /// <summary>
    /// Initialize window state.
    /// </summary>
    [MenuItem("Window/Scene View")]
    static void Init()
    {
        // EditorWindow.GetWindow() will return the open instance of the specified window or create a new
        // instance if it can't find one. The second parameter is a flag for creating the window as a
        // Utility window; Utility windows cannot be docked like the Scene and Game view windows.

        SceneViewWindow window = EditorWindow.GetWindow(typeof(SceneViewWindow), false, "Scene View") as SceneViewWindow;
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 200f, 400f);
    }

#if (UNITY_EDITOR || !UNITY_PS4)
    /// <summary>
    /// Called on GUI events.
    /// </summary>
    internal void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);

        GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            if (scene.enabled)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scene.path);

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(i + ": " + sceneName, new GUIStyle(EditorStyles.miniButtonLeft) { alignment = TextAnchor.MiddleLeft }))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }
                if (GUILayout.Button("Additive", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                {
                    EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        GUILayout.Label("Other Scenes", EditorStyles.boldLabel);
        string mapEditorScenePath = "Assets/Nova/Scenes/Map Editor.unity";
        string mapEditorSceneName = "Map Editor";
        if (GUILayout.Button(mapEditorSceneName, EditorStyles.miniButton))
        {
            EditorSceneManager.OpenScene(mapEditorScenePath, OpenSceneMode.Single);
        }

        string widgetLibraryScenePath = "Assets/Nova/Scenes/UI Widget Library.unity";
        string widgetLibrarySceneName = "UI Widget Library";
        if (GUILayout.Button(widgetLibrarySceneName, EditorStyles.miniButton))
        {
            EditorSceneManager.OpenScene(widgetLibraryScenePath, OpenSceneMode.Single);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
#endif  // #if (UNITY_EDITOR || !UNITY_PS4)
}