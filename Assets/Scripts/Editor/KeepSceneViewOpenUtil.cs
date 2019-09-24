#if UNITY_EDITOR
    using UnityEngine;

    public class KeepSceneViewOpenUtil : MonoBehaviour
    {
        public bool useSceneView = false;
        private void Awake()
        {
            if (useSceneView)
            {
                UnityEditor.EditorWindow.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
            }
        }
    }
#endif