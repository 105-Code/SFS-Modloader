using UnityEngine;

namespace ModLoader.IO
{

    public class ConsoleGUI : MonoBehaviour
    {
        // show in what position is the scroll of console
        private Vector2 _scrollConsole = Vector2.zero;

        // show if the console is visible for user
        private bool _isVisible;
        public bool IsVisible
        {
            get { return this._isVisible; }
        }

        // store all records in the game
        private GUIContent _logs;
        public string Logs
        {
            set
            {
                this._logs.text = value;
                this.checkScroll();
            }
        }

        // console styles like background and text color
        private GUIStyle _consoleStyle;
        private GUIStyle _containerStyle;


        private void Awake()
        {
            this._isVisible = false;
            this._consoleStyle = new GUIStyle();
            this._containerStyle = new GUIStyle();
            this._logs = new GUIContent();
        }

        private void Start()
        {
            this._containerStyle.normal.background = this.makeTexture(new Color32(0, 0, 0, 150));

            this._consoleStyle.normal.textColor = Color.white;
            this._consoleStyle.fontSize = 16;
            this._consoleStyle.padding.left = 10;

        }

        /// <sumary>
        /// Create solid color texture
        ///</sumary>
        private Texture2D makeTexture(Color col)
        {
            Color[] pix = new Color[1];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(1, 1);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                this._isVisible = !this._isVisible;
                this.checkScroll();
            }
        }

        private void OnGUI()
        {
            if (this._isVisible)
            {
                float consoleHeigth = Screen.height * 0.5f;

                GUI.BeginGroup(new Rect(0, 0, Screen.width, consoleHeigth), this._containerStyle);
                this._scrollConsole = GUILayout.BeginScrollView(this._scrollConsole, GUILayout.Width(Screen.width), GUILayout.Height(consoleHeigth));
                GUILayout.Box(this._logs, this._consoleStyle);
                GUILayout.EndScrollView();
                GUI.EndGroup();
            }
        }

        /// <summary>
        /// check if scroll is at 70% of console height
        /// </summary>
        private void checkScroll()
        {
            if (this._isVisible)
            {
                float consoleHeight = this._consoleStyle.CalcHeight(this._logs, Screen.width);
                if (this._scrollConsole.y > consoleHeight * 0.7)
                {
                    this._scrollConsole.y = consoleHeight;
                }
            }

        }

    }
}