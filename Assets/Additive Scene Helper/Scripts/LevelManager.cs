using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

namespace AdditiveSceneHelper
{
    [CreateAssetMenu(fileName = "LevelManager", menuName = "Additive Scene Helper/LevelManager", order = 0)]
    public class LevelManager : ScriptableObject
    {
        [SerializeField] string MAIN_SCENE_NAME = "_Main Scene";

        // [NonReorderable] // If there is array overlapping problem, As a temporary fix till its fixed 
        // Unity bug https://answers.unity.com/questions/1828499/nested-class-lists-inspector-overlapping-bug.html 
        public ScenesInfo[] sceneInfos;

        public ScenesInfo LevelSceneInfo => sceneInfos[0];

        public void JumpNextLevel()
        {
            IncrementLevelIndex();
            ReloadScene();
        }

        public void ReloadScene()
        {
            LoadSceneAccordingInfo();
        }

        public void LoadSceneAccordingInfo()
        {
            SceneManager.LoadScene(MAIN_SCENE_NAME);
            SceneManager.LoadScene(LevelSceneInfo.CurrSceneName, LoadSceneMode.Additive);
        }

        void IncrementLevelIndex() => LevelSceneInfo.LevelIndex = (LevelSceneInfo.LevelIndex + 1) % LevelSceneInfo.NumScenes;

        public void RunShortcutCoroutine(MonoBehaviour monoBehaviour) => monoBehaviour.StartCoroutine(UpdateShortCut());
        IEnumerator UpdateShortCut()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.R)) ReloadScene();
                else if (Input.GetKeyDown(KeyCode.N)) JumpNextLevel();
#if UNITY_EDITOR
                else if (Input.GetKeyDown(KeyCode.P)) UnityEditor.EditorApplication.isPaused = true;
#endif
                yield return null;
            }
        }

        [System.Serializable]
        public class ScenesInfo
        {
            public string name;
            public string sceneFolderPath;

            [SerializeField] Object[] sceneObjects = new Object[] { }; // Error during build on iOS "sceneObjects[i].name"
            [HideInInspector, SerializeField] string[] sceneNames;

            public string[] SceneNames
            {
                get
                {
#if UNITY_EDITOR
                    sceneNames = new string[sceneObjects.Length];
                    for (int i = 0; i < sceneNames.Length; i++) sceneNames[i] = sceneObjects[i].name;
#endif
                    return sceneNames;
                }
            }

            public int NumScenes => SceneNames.Length;
            public string CurrSceneName => SceneNames[LevelIndex];
            public string CurrScenePath => sceneFolderPath + "/" + SceneNames[LevelIndex] + ".unity";
            public int SceneFolderPathHashCode => sceneFolderPath.GetHashCode();

            public int LevelIndex
            {
                get => PlayerPrefs.GetInt(name + "Key");
                set => PlayerPrefs.SetInt(name + "Key", value);
            }
        }
    }
}
