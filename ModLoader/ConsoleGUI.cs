using UnityEngine;
using static UnityEngine.GUI;

namespace ModLoader
{

    public class ConsoleGUI : MonoBehaviour
    {
        // show console initial position and size
        private Rect _windowRect;
        private Rect _consoleRect;
        private Rect _consoleHeightRect;
        //private Rect _sceneRect;
        //private Rect _sceneHeightRect;
        //private Rect _toolbarRect;


        //draw windows Gui items
        private WindowFunction _windosDraw;

        /// show in what position is the scroll of console
        private Vector2 _scrollConsole = Vector2.zero;
        //private Vector2 _scrollScene = Vector2.zero;
        //private Vector2 _scrollMods = Vector2.zero;

        private bool _isVisible;
        // show if the console is visible for user
        public bool IsVisible
        {
            get { return this._isVisible; }
            set { this._isVisible = value; }
        }

        // identify which tab to render
        private int _activeTab;

        // tab list
        //public string[] tabs = new string[] { "Console System", "Scene Information" };

        // store all records in the game
        private GUIContent _logs;
        public string Logs
        {
            set
            {

                this._logs.text += value;
                float newHeight = this._consoleStyle.CalcHeight(this._logs, this._consoleRect.width);
                if (newHeight > this._consoleHeightRect.height)
                {
                    this._consoleHeightRect.height = newHeight;
                }
                
                if (this._activeTab == 0 && this._scrollConsole.y > this._consoleRect.height * 0.6)
                {
                   
                    this._scrollConsole.y = this._consoleHeightRect.height;
                }
            }
        }
        /*
        private GUIContent _sceneGameObjects;
        public GameObject[] SceneGameObjects
        {
            set
            {
                this._sceneGameObjects = new GUIContent();
                Component[] components;
                foreach (GameObject sceneObject in value)
                {
                    this._sceneGameObjects.text += "Object Name: " + sceneObject.name + "\nComponents:\n";
                    components = sceneObject.GetComponents(typeof(Component));
                    foreach (Component component in components)
                    {
                        this._sceneGameObjects.text += "\t" + component.ToString() + "\n";
                    }
                    this._sceneGameObjects.text += "\n";
                }
                this._sceneHeightRect.height = this._consoleStyle.CalcHeight(this._sceneGameObjects, this._sceneHeightRect.width);
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
        }*/

        // console styles like background and text color
        private GUIStyle _consoleStyle;

        private GUIStyle _windowsStyle;

        //private GUIStyle _toolbarStyle;

        void Awake()
        {
            this._isVisible = false;
            this._windowsStyle = new GUIStyle();
            this._consoleStyle = new GUIStyle();
            //this._toolbarStyle = new GUIStyle();

            this._logs = new GUIContent();
            //this._mods = new GUIContent();
            //this._sceneGameObjects = new GUIContent();

            this._windowRect = new Rect(0, 0, Screen.width, Screen.height * 0.7f);
            //this._toolbarRect = new Rect(0, 0, this._windowRect.width, 30);

            this._consoleRect = new Rect(0, 0, this._windowRect.width, this._windowRect.height);
            this._consoleHeightRect = new Rect(0, 0, this._windowRect.width - 20, this._windowRect.height);


            //this._sceneRect = new Rect(0, 30, this._windowRect.width, this._windowRect.height - 30);
            //this._sceneHeightRect = new Rect(0, 0, this._windowRect.width - 20, this._windowRect.height);

            this._windosDraw = new GUI.WindowFunction(this.windowFunction);
        }

        void Start()
        {
            this._consoleStyle.normal.textColor = Color.white;
            this._consoleStyle.fontSize = 16;
            this._consoleStyle.padding.left = 10;
            this._consoleStyle.normal.background = this.makeTexture(new Color32(0, 0, 0, 160));

            this._windowsStyle.normal.textColor = Color.white;
            this._windowsStyle.normal.background = this.makeTexture(new Color32(36, 42, 49, 160));

            //this._toolbarStyle.normal.textColor = Color.white;
            //this._toolbarStyle.alignment = TextAnchor.MiddleCenter;
            //this._toolbarStyle.fontSize = 16;
            //this._toolbarStyle.margin.right = 10;
            //this._toolbarStyle.normal.background = this.makeTexture(new Color32(27, 117, 222, 200));

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
            }
        }

        async void OnGUI()
        {

            if (this._isVisible)
            {
                GUI.Window(GUIUtility.GetControlID(FocusType.Passive), this._windowRect, this._windosDraw, "", this._windowsStyle);
            }
        }

        private void windowFunction(int windowID)
        {
            /*this._activeTab = GUI.Toolbar(this._toolbarRect, this._activeTab, this.tabs, this._toolbarStyle);
            switch (this._activeTab)
            {
                case 0:
                    this.consoleTab();
                    break;
                case 1:
                    this.sceneTab();
                    break;
                    case 2:
                        this.modsTab();
                        break;
        }*/
        this.consoleTab();
        GUI.DragWindow();
        }

        private void consoleTab()
        {
            this._scrollConsole = GUI.BeginScrollView(this._consoleRect, this._scrollConsole, this._consoleHeightRect);

            GUI.Box(this._consoleHeightRect, this._logs, this._consoleStyle);

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
        }

        private void sceneTab()
        {
            this._scrollScene = GUI.BeginScrollView(this._sceneRect, this._scrollScene, this._sceneHeightRect);

            GUI.Box(this._sceneHeightRect, this._sceneGameObjects, this._consoleStyle);

            GUI.EndScrollView();
        }*/

    }
}
