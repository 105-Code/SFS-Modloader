using UnityEngine;
using static UnityEngine.GUI;

namespace ModLoader
{
   
    public class ConsoleGUI : MonoBehaviour
    {
        private const string WINDOW_NAME = "Mod loader console";
        private const float MENU_HEIGHT = 0.6f;
        private const float MENU_WIDTH = 0.4f;

        // show console initial position and size
        private Rect _consoleRect;

        //draw windows Gui items
        private WindowFunction _windosDraw;

        /// show in what position is the scroll of console
        private Vector2 _scrollConsole = Vector2.zero;
        private Vector2 _scrollScene = Vector2.zero;
        private Vector2 _scrollMods = Vector2.zero;

        private bool _isVisible;
        // show if the console is visible for user
        public bool IsVisible
        {
            get { return this._isVisible; }
            set { this._isVisible = value; }
        }

        // identify what tab render
        private int _activeTab;

        // tab list
        public string[] tabs = new string[] { "Console", "Scene" };

        // store all logs in game
        private GUIContent _logs;
        public string Logs
        {
            set
            {
                this._logs.text += value;
                if (this._activeTab == 0 && this._scrollConsole.y > this._consoleheight * 0.2)
                {
                    this._scrollConsole.y = this._consoleheight;
                }
               
                
            }
        }

        private GUIContent _sceneGameObjects;
        public GameObject[] SceneGameObjects
        {
            set
            {
                this._sceneGameObjects = new GUIContent();
                Component[] components;
                foreach (GameObject sceneObject in value)
                {
                    this._sceneGameObjects.text += sceneObject.name + "\n";
                    components = sceneObject.GetComponents(typeof(Component));
                    foreach (Component component in components)
                    {
                        this._sceneGameObjects.text += "\t" + component.ToString() + "\n";
                    }
                }
            }
        }

        private GUIContent _mods;
        public SFSMod[] Mods
        {
            set
            {
                this._mods = new GUIContent();
                foreach (SFSMod mod in value)
                {
                    this._mods.text += "Name:" + mod.Name + "\n";
                    this._mods.text += "Id:" + mod.ModId + "\n";
                    this._mods.text += "Author:" + mod.Author + "\n";
                    this._mods.text += "Description:\n";
                    this._mods.text += mod.Description + "\n\n";
                }
            }
        }

        // console styles like background and text color
        private GUIStyle _consoleStyle;

        // this make resize console
        private float _consoleheight;

        void Awake()
        {
            this._isVisible = true;
            this._consoleStyle = new GUIStyle();
            this._logs = new GUIContent();
            this._sceneGameObjects = new GUIContent();
            this._mods = new GUIContent();
            this._consoleRect = new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * MENU_WIDTH, Screen.height * MENU_HEIGHT);
            this._windosDraw = new GUI.WindowFunction(this.windowFunction);
        }

        void Start()
        {
            this._consoleStyle.normal.textColor = Color.white;
            this._consoleStyle.normal.background = this.makeTexture(Color.black);
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
            if (Input.GetKeyDown(KeyCode.F1)){
                this._isVisible = !this._isVisible;
            }
        }

        async void OnGUI()
        {

            if (this._isVisible)
            {
                this._consoleRect = GUI.Window(GUIUtility.GetControlID(FocusType.Passive), this._consoleRect, this._windosDraw, WINDOW_NAME);
            }
        }

        private void windowFunction(int windowID)
        {
            this._activeTab = GUI.Toolbar(new Rect(10, 20, this._consoleRect.width - 20f, 30), this._activeTab, this.tabs);
            switch (this._activeTab)
            {
                case 0:
                    this.consoleTab();
                    break;
                case 1:
                    this.sceneTab();
                    break;
                /*case 2:
                    this.modsTab();
                    break;*/
            }
            GUI.DragWindow();
        }

        private void consoleTab()
        {
            this._consoleheight = this._consoleStyle.CalcHeight(this._logs, this._consoleRect.width - 20f);
            if (this._consoleheight < this._consoleRect.height - 60f)
            {
                this._scrollConsole.y = this._consoleheight = this._consoleRect.height - 60f;
            }

            this._scrollConsole = GUI.BeginScrollView(new Rect(0, 55, this._consoleRect.width, this._consoleRect.height - 60f), this._scrollConsole, new Rect(0, 0, this._consoleRect.width - 20f, this._consoleheight));

            GUI.Box(new Rect(10, 0, this._consoleRect.width - 30f, this._consoleheight), this._logs, this._consoleStyle);

            GUI.EndScrollView();
        }

        /*private void modsTab()
        {
            this._consoleheight = this._consoleStyle.CalcHeight(this._mods, this._consoleRect.width - 20f);
            if (this._consoleheight < this._consoleRect.height - 60f)
            {
                this._scrollMods.y = this._consoleheight = this._consoleRect.height - 60f;
            }

            this._scrollMods = GUI.BeginScrollView(new Rect(0, 55, this._consoleRect.width, this._consoleRect.height - 60f), this._scrollMods, new Rect(0, 0, this._consoleRect.width - 20f, this._consoleheight));

            GUI.Box(new Rect(10, 0, this._consoleRect.width - 30f, this._consoleheight), this._mods, this._consoleStyle);

            GUI.EndScrollView();
        }*/

        private void sceneTab()
        {
            this._consoleheight = this._consoleStyle.CalcHeight(this._sceneGameObjects, this._consoleRect.width - 20f);
            if (this._consoleheight < this._consoleRect.height - 60f)
            {
                this._scrollScene.y = this._consoleheight = this._consoleRect.height - 60f;
            }

            this._scrollScene = GUI.BeginScrollView(new Rect(0, 55, this._consoleRect.width, this._consoleRect.height - 60f), this._scrollScene, new Rect(0, 0, this._consoleRect.width - 20f, this._consoleheight));

            GUI.Box(new Rect(10, 0, this._consoleRect.width - 30f, this._consoleheight), this._sceneGameObjects, this._consoleStyle);

            GUI.EndScrollView();
        }
    }

}
