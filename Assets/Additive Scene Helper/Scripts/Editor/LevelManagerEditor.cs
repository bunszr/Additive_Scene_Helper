using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;

namespace AdditiveSceneManager
{
    [CustomEditor(typeof(LevelManager))]
    public partial class LevelManagerEditor : Editor
    {
        LevelManager levelManager;
        SceneInfoInEditor[] scenesInfoInEditor;
        List<string> openedSceneListOnHierarchy;

        private void OnEnable()
        {
            levelManager = target as LevelManager;
            string[] sceneFolderPathList = EditorBuildSettings.scenes.Select(x => x.path.Substring(0, Utility.GetSceneFolderPathCount(x.path))).ToArray();
            openedSceneListOnHierarchy = Enumerable.Range(0, ESM.sceneCount).Select(i => ESM.GetSceneAt(i).path.GetFolderPathFromScenePath()).ToList();

            scenesInfoInEditor = new SceneInfoInEditor[levelManager.sceneInfos.Length];
            for (int i = 0; i < scenesInfoInEditor.Length; i++)
                scenesInfoInEditor[i] = new SceneInfoInEditor(levelManager.sceneInfos[i], sceneFolderPathList, openedSceneListOnHierarchy);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying) return;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reinitialize")) OnEnable();

            if (GUILayout.Button("Remove All Additive Scene"))
            {
                Enumerable.Range(1, ESM.sceneCount - 1).Reverse().ToList().ForEach(i => ESM.CloseScene(ESM.GetSceneAt(i), true));
                OnEnable();
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < scenesInfoInEditor.Length; i++) scenesInfoInEditor[i].OnInspectorGUI();
        }
    }
}