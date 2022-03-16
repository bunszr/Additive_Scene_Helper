using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;

namespace AdditiveSceneManager
{
    public class SceneInfoInEditor
    {
        int oldLevelIndex;
        readonly LevelManager.ScenesInfo scenesInfo;
        readonly EditorBuildSettingsScene[] scenes;
        readonly List<string> openedSceneListOnHierarchy;

        public SceneInfoInEditor(LevelManager.ScenesInfo scenesInfo, string[] sceneFolderPath, List<string> openedSceneListOnHierarchy)
        {
            this.scenesInfo = scenesInfo;
            this.oldLevelIndex = scenesInfo.LevelIndex;
            this.openedSceneListOnHierarchy = openedSceneListOnHierarchy;
            scenes = EditorBuildSettings.scenes.Where((x, i) => string.Compare(sceneFolderPath[i], scenesInfo.sceneFolderPath, false) == 0).Select(x => x).ToArray();
        }

        public void OnInspectorGUI()
        {
            scenesInfo.LevelIndex = EditorGUILayout.IntSlider(scenesInfo.name + " Index", scenesInfo.LevelIndex, 0, (scenes.Length - 1).NoNegative());

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
                UnityEngine.SceneManagement.Scene loadedSceneAtIndex = ESM.GetSceneAt(sceneIndexOnHierarchy);
                if (loadedSceneAtIndex.isDirty) ESM.SaveScene(loadedSceneAtIndex); // Save if the scene has changed
                ESM.CloseScene(loadedSceneAtIndex, true); // Close the existing scene at index
            }

            UnityEngine.SceneManagement.Scene openingScene = ESM.OpenScene(scenes[scenesInfo.LevelIndex].path, OpenSceneMode.Additive);
            if (!openedSceneListOnHierarchy.Contains(scenesInfo.sceneFolderPath)) openedSceneListOnHierarchy.Add(scenesInfo.sceneFolderPath);

            if (hasValidSceneAtIndex) ESM.MoveSceneAfter(openingScene, ESM.GetSceneAt(sceneIndexOnHierarchy - 1)); // The scene which loaded is highest index in the hierarchy(loaded at the bottom). Therebefore must rearrange laoded scenes.
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
            for (int i = 0; i < openedSceneListOnHierarchy.Count; i++)
            {
                if (string.Compare(openedSceneListOnHierarchy[i], scenesInfo.sceneFolderPath) == 0) return i;
            }
            return -1;
        }
    }
}