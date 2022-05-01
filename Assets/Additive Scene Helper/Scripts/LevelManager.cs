using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

namespace AdditiveSceneHelper
{
    [CreateAssetMenu(fileName = "LevelManager", menuName = "Additive Scene Manager/LevelManager", order = 0)]
    public class LevelManager : ScriptableObject
    {
        [SerializeField] string MAIN_SCENE_NAME = "_Main Scene";

        public ScenesInfo[] sceneInfos;
        [System.NonSerialized] string[] levelScenePaths; // Scene paths in the levels folder in EditorBuildSettings.
        [System.NonSerialized] bool hasInit = false;

        public ScenesInfo LevelSceneInfo => sceneInfos[0];

        public void Init()
        {
            if (!hasInit)
            {
                hasInit = true;
                levelScenePaths = Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
                  .Where(index => string.Compare(LevelSceneInfo.sceneFolderPath, SceneUtility.GetScenePathByBuildIndex(index).GetFolderPathFromScenePath(), false) == 0)
                  .Select(index => SceneUtility.GetScenePathByBuildIndex(index)).ToArray();
            }
        }

        public void JumpNextLevel()
        {
            IncrementLevelIndex();
            ReloadScene();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(MAIN_SCENE_NAME);
            SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath(levelScenePaths[LevelSceneInfo.LevelIndex]), LoadSceneMode.Additive);
        }

        void IncrementLevelIndex() => LevelSceneInfo.LevelIndex = (LevelSceneInfo.LevelIndex + 1) % levelScenePaths.Length;

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
            public int LevelIndex
            {
                get => PlayerPrefs.GetInt(name + "Key");
                set => PlayerPrefs.SetInt(name + "Key", value);
            }
        }
    }
}
