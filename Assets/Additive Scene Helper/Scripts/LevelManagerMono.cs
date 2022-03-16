using UnityEngine;

namespace AdditiveSceneManager
{
    public class LevelManagerMono : MonoBehaviour
    {
        public LevelManager levelManager;

        private void Start()
        {
            levelManager.Init();
#if UNITY_EDITOR
            levelManager.RunShortcutCoroutine(this);
#endif
        }
    }
}