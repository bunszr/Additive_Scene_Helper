using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;

namespace AdditiveSceneHelper
{
    public class SceneInfoInEditor
    {
        int oldLevelIndex;
        readonly LevelManager.ScenesInfo scenesInfo;
        readonly EditorBuildSettingsScene[] scenes;
        bool disableRemoveButton;

        public SceneInfoInEditor(LevelManager.ScenesInfo scenesInfo, string[] sceneFolderPath)
        {
            this.scenesInfo = scenesInfo;
            this.oldLevelIndex = scenesInfo.LevelIndex;
            this.scenes = EditorBuildSettings.scenes.Where((x, i) => string.Compare(sceneFolderPath[i], scenesInfo.sceneFolderPath, false) == 0).Select(x => x).ToArray();

            disableRemoveButton = !LevelManagerEditor.openedSceneFolderPathListOnHierarchy.Contains(scenesInfo.sceneFolderPath);
        }

        public void InspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            scenesInfo.LevelIndex = EditorGUILayout.IntSlider(scenesInfo.name + " Index", scenesInfo.LevelIndex, 0, (scenes.Length - 1).NoNegative());
            EditorGUI.BeginDisabledGroup(disableRemoveButton);
            if (GUILayout.Button("R", GUILayout.Width(25)))
            {
                disableRemoveButton = true;
                if (LevelManagerEditor.openedSceneFolderPathListOnHierarchy.Contains(scenesInfo.sceneFolderPath))
                {
                    SaveAndCloseOpenedScene(GetSceneIndex());
                    LevelManagerEditor.openedSceneFolderPathListOnHierarchy.Remove(scenesInfo.sceneFolderPath);
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (!LevelIndexIsChanged()) return;

            if (scenesInfo.name == "")
            {
                Debug.LogError("Scene Info name does not assign!");
                return;
            }

            int sceneIndexOnHierarchy = GetSceneIndex();
            bool hasValidSceneAtIndex = sceneIndexOnHierarchy != -1;
            if (hasValidSceneAtIndex)
            {
                SaveAndCloseOpenedScene(sceneIndexOnHierarchy);
            }

            UnityEngine.SceneManagement.Scene openingScene = ESM.OpenScene(scenes[scenesInfo.LevelIndex].path, OpenSceneMode.Additive);
            if (!LevelManagerEditor.openedSceneFolderPathListOnHierarchy.Contains(scenesInfo.sceneFolderPath)) LevelManagerEditor.openedSceneFolderPathListOnHierarchy.Add(scenesInfo.sceneFolderPath);

            if (hasValidSceneAtIndex) ESM.MoveSceneAfter(openingScene, ESM.GetSceneAt(sceneIndexOnHierarchy - 1)); // The scene which loaded is highest index in the hierarchy(loaded at the bottom). Therebefore must rearrange laoded scenes.

            disableRemoveButton = !LevelManagerEditor.openedSceneFolderPathListOnHierarchy.Contains(scenesInfo.sceneFolderPath);

        }

        bool LevelIndexIsChanged()
        {
            if (oldLevelIndex != scenesInfo.LevelIndex)
            {
                oldLevelIndex = scenesInfo.LevelIndex;
                return true;
            }
            return false;
        }

        int GetSceneIndex()
        {
            for (int i = 0; i < LevelManagerEditor.openedSceneFolderPathListOnHierarchy.Count; i++)
            {
                if (string.Compare(LevelManagerEditor.openedSceneFolderPathListOnHierarchy[i], scenesInfo.sceneFolderPath) == 0) return i;
            }
            return -1;
        }

        void SaveAndCloseOpenedScene(int sceneIndexOnHierarchy)
        {
            UnityEngine.SceneManagement.Scene loadedSceneAtIndex = ESM.GetSceneAt(sceneIndexOnHierarchy);
            if (loadedSceneAtIndex.isDirty) ESM.SaveScene(loadedSceneAtIndex); // Save if the scene has changed
            ESM.CloseScene(loadedSceneAtIndex, true); // Close the existing scene at index
        }
    }
}