#region

using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    // Simple script that randomizes the rotation of the selected GameObjects.
    // It also lists which objects are currently selected.

    public class FloatingWindow : EditorWindow
    {
    #region Public Variables

        public float  rotationAmount = 0.33f;
        public string selected       = "";

    #endregion

    #region Private Variables

        private GameObject           gameObject;
        private GameObject           cacheGameObject;
        private Editor               gameObjectEditor;
        private PreviewRenderUtility previewUtility;

    #endregion

    #region Public Methods

        public static void Create(GameObject gameObject)
        {
            var window = GetWindow<FloatingWindow>();
            if (HasOpenInstances<FloatingWindow>() == false)
            {
                window              = GetWindowWithRect<FloatingWindow>(new Rect(0 , 0 , 256 , 256));
                window.titleContent = new GUIContent("My Window Title");
                window.Show();
            }

            window.gameObject = gameObject;
        }

        public void OnDisable()
        {
            if (previewUtility is not null)
            {
                previewUtility.Cleanup();
                previewUtility = null;
            }
        }

        public void OnEnable()
        {
            // var root = rootVisualElement;
            //
            // VisualElement label = new Label("Hello World!");
            // root.Add(label);
            previewUtility ??= new PreviewRenderUtility();
        }

    #endregion

    #region Private Methods

        private void OnGUI()
        {
            var bgColor = new GUIStyle();
            bgColor.normal.background = Texture2D.blackTexture;

            if (gameObject != null)
            {
                if (gameObjectEditor == null || cacheGameObject != gameObject)
                {
                    gameObjectEditor = Editor.CreateEditor(gameObject);
                    Repaint();
                }

                var rect = GUILayoutUtility.GetRect(256 , 256);
                gameObjectEditor.OnInteractivePreviewGUI(rect , bgColor);
                cacheGameObject = gameObject;
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        // [UnityEditor.MenuItem("Example/Randomize Children In Selection _g")]
        private static void RandomizeWindow()
        {
            if (HasOpenInstances<FloatingWindow>())
            {
                GetWindow<FloatingWindow>().Close();
            }
            else
            {
                var window = GetWindowWithRect<FloatingWindow>(new Rect(0 , 0 , 256 , 256));
                window.titleContent = new GUIContent("My Window Title");
                window.Show();
            }
        }

    #endregion
    }
}