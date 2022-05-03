using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;

namespace AdditiveSceneHelper
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        public static List<int> openedSceneFolderPathHasCodeListOnHierarchy;

        LevelManager levelManager;
        SceneInfoInEditor[] scenesInfoInEditors;

        private void OnEnable()
        {
            Init();
            ESM.sceneClosed += OnSceneClosedMethod;
        }

        private void OnDisable()
        {
            ESM.sceneClosed -= OnSceneClosedMethod;
        }

        public void OnSceneClosedMethod(UnityEngine.SceneManagement.Scene closedScene)
        {
            if (openedSceneFolderPathHasCodeListOnHierarchy.Contains(closedScene.path.GetFolderPathFromScenePath().GetHashCode()))
                openedSceneFolderPathHasCodeListOnHierarchy.Remove(closedScene.path.GetFolderPathFromScenePath().GetHashCode());
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying) return;

            EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Reinitialize")) Init();

                if (GUILayout.Button("Remove All Additive Scene"))
                {
                    Enumerable.Range(1, ESM.sceneCount - 1).Reverse().ToList().ForEach(i => ESM.CloseScene(ESM.GetSceneAt(i), true));
                    Init();
                }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < scenesInfoInEditors.Length; i++) scenesInfoInEditors[i].InspectorGUI();
        }

        private void Init()
        {
            levelManager = target as LevelManager;
            string[] sceneFolderPathList = EditorBuildSettings.scenes.Select(x => x.path.Substring(0, Utility.GetSceneFolderPathCount(x.path))).ToArray();
            openedSceneFolderPathHasCodeListOnHierarchy = Enumerable.Range(0, ESM.sceneCount).Select(i => ESM.GetSceneAt(i).path.GetFolderPathFromScenePath().GetHashCode()).ToList();

            scenesInfoInEditors = new SceneInfoInEditor[levelManager.sceneInfos.Length];
            for (int i = 0; i < scenesInfoInEditors.Length; i++)
                scenesInfoInEditors[i] = new SceneInfoInEditor(levelManager.sceneInfos[i], sceneFolderPathList);
        }
    }
}