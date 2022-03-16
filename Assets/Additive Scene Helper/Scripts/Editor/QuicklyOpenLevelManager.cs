using UnityEngine;
using UnityEditor;

namespace AdditiveSceneManager
{
    public class QuicklyOpenLevelManager : EditorWindow
    {
        /*
            Alt + a => Quickly open/close LevelManager
        */

        static LevelManager levelManagerSo;
        static Editor levelManagerSoEditor;
        static EditorWindow window;

        [MenuItem("Tools/Level Manager &a")] // & == Alt
        private static void ShowWindow()
        {
            if (window != null)
            {
                window.Close();
                return;
            }

            levelManagerSo = Resources.Load<LevelManager>("Scriptables/LevelManager 00");
            levelManagerSoEditor = Editor.CreateEditor(levelManagerSo);

            window = GetWindow<QuicklyOpenLevelManager>();
            window.Show();
            window.titleContent = new GUIContent(levelManagerSo.name);
        }

        private void OnGUI()
        {
            levelManagerSoEditor.OnInspectorGUI();
        }
    }
}