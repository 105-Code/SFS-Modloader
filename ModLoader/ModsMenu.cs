

using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModLoader
{
    public class ModsMenu : MonoBehaviour
    {
        private ModLoader.Console _console;
        // Events
        public static event EventHandler OnMainWindowGUI_Start;
        public static event EventHandler OnMainWindowGUI_Build;
        public static event EventHandler OnMainWindowGUI_World;

        public static event EventHandler OnSideWindowGUI_Start;
        public static event EventHandler OnSideWindowGUI_Build;
        public static event EventHandler OnSideWindowGUI_World;


        // MAIN WINDOWS
        public static Rect startWindowRect = new Rect(Screen.width / 4, Screen.height / 4, Screen.width/2, Screen.height/2);
        public static Rect buildWindowRect = new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2);
        public static Rect worldWindowRect = new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2);

        // Small Side Windows
        public static Rect startSideWindowRect = new Rect(Screen.width - 200f, 80f, 190f, 300f);
        public static Rect buildSideWindowRect = new Rect(Screen.width - 200f, 80f, 190f, 300f);
        public static Rect worldSideWindowRect = new Rect(Screen.width - 200f, 80f, 190f, 300f);


        // WINDOW IDS
        static int startWindowID = GUIUtility.GetControlID(FocusType.Passive);
        static int buildWindowID = GUIUtility.GetControlID(FocusType.Passive);
        static int worldWindowID = GUIUtility.GetControlID(FocusType.Passive);

        static int startSideWindowID = GUIUtility.GetControlID(FocusType.Passive);
        static int buildSideWindowID = GUIUtility.GetControlID(FocusType.Passive);
        static int worldSideWindowID = GUIUtility.GetControlID(FocusType.Passive);

        // Window Bools
        static bool showStartWindow = false;
        static bool showBuildWindow = false;
        static bool showWorldWindow = false;

        // Scroll Positions
        static Vector2 startMainScrollPosition = Vector2.zero;
        static Vector2 buildMainScrollPosition = Vector2.zero;
        static Vector2 worldMainScrollPosition = Vector2.zero;

        static Vector2 startSideScrollPosition = Vector2.zero;
        static Vector2 buildSideScrollPosition = Vector2.zero;
        static Vector2 worldSideScrollPosition = Vector2.zero;

        public void Start()
        {
            this._console= ModLoader.Console.root.GetComponent<ModLoader.Console>();
            Create_Textures();
        }

        /// <summary>
        /// Runs the GUI
        /// </summary>
        public void OnGUI()
        {
            if (this._console.ConsoleGui.IsVisible)
            {

            
            switch (Helper.currentScene)
            {
                case scene.Base:
                    if (showStartWindow)
                    {
                        // Show main window
                        startWindowRect = GUI.Window(startWindowID, startWindowRect, StartMainWindow, "", windowBackgroundStyle);
                    }
                    else
                    {
                        // Show side window
                        startSideWindowRect = GUI.Window(startSideWindowID, startSideWindowRect, StartSideWindow, "", windowBackgroundStyle);
                    }
                    break;

                case scene.Home:
                    if (showStartWindow)
                    {
                        // Show main window
                        startWindowRect = GUI.Window(startWindowID, startWindowRect, StartMainWindow, "", windowBackgroundStyle);
                    }
                    else
                    {
                        // Show side window
                        startSideWindowRect = GUI.Window(startSideWindowID, startSideWindowRect, StartSideWindow, "", windowBackgroundStyle);
                    }
                    break;

                case scene.Build:
                    if (showBuildWindow)
                    {
                        // Show main window
                        buildWindowRect = GUI.Window(buildWindowID, buildWindowRect, BuildMainWindow, "", windowBackgroundStyle);
                    }
                    else
                    {
                        // Show side window
                        buildSideWindowRect = GUI.Window(buildSideWindowID, buildSideWindowRect, BuildSideWindow, "", windowBackgroundStyle);
                    }
                    break;

                case scene.World:
                    if (showWorldWindow)
                    {
                        // Show main window
                        worldWindowRect = GUI.Window(worldWindowID, worldWindowRect, WorldMainWindow, "", windowBackgroundStyle);
                    }
                    else
                    {
                        // Show side window
                        worldSideWindowRect = GUI.Window(worldSideWindowID, worldSideWindowRect, WorldSideWindow, "", windowBackgroundStyle);
                    }
                    break;
            }
            }
        }

        /// <summary>
        /// Side window for the Start (Base_PC/ Home_PC) Scene
        /// </summary>
        /// <param name="windowID"></param>
        private void StartSideWindow(int windowID)
        {
            GUILayout.BeginVertical();
            showStartWindow = GUILayout.Toggle(showStartWindow, "Main Window", gradientButtonStyle, GUILayout.Height(25));
            OnSideWindowGUI_Start?.Invoke(this, EventArgs.Empty);
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>
        /// Main window for the Start Scene
        /// </summary>
        /// <param name="windowID"></param>
        private void StartMainWindow(int windowID)
        {
            GUILayout.BeginVertical();
            startMainScrollPosition = GUILayout.BeginScrollView(startMainScrollPosition);
            // PUT MAIN GUI CONTENT HERE
            GUILayout.Label("Mod Menus", windowTitleStyle);

            OnMainWindowGUI_Start?.Invoke(this, EventArgs.Empty);

            GUILayout.EndScrollView();

            // Close Button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            showStartWindow = GUILayout.Toggle(showStartWindow, "Close", gradientButtonStyle, GUILayout.Height(30), GUILayout.Width(175));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>
        /// Side window for the Build Scene
        /// </summary>
        /// <param name="windowID"></param>
        private void BuildSideWindow(int windowID)
        {
            GUILayout.BeginVertical();
            showBuildWindow = GUILayout.Toggle(showBuildWindow, "Main Window", gradientButtonStyle, GUILayout.Height(25));
            OnSideWindowGUI_Build?.Invoke(this, EventArgs.Empty);
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>
        /// Main window for the Build Scene
        /// </summary>
        /// <param name="windowID"></param>
        private void BuildMainWindow(int windowID)
        {
            GUILayout.BeginVertical();
            buildMainScrollPosition = GUILayout.BeginScrollView(buildMainScrollPosition);
            // PUT MAIN GUI CONTENT HERE
            GUILayout.Label("Mod Menus", windowTitleStyle);

            OnMainWindowGUI_Build?.Invoke(this, EventArgs.Empty);

            GUILayout.EndScrollView();

            // Close Button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            showBuildWindow = GUILayout.Toggle(showBuildWindow, "Close", gradientButtonStyle, GUILayout.Height(30), GUILayout.Width(175));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>
        /// Side window for the World Scene
        /// </summary>
        /// <param name="windowID"></param>
        private void WorldSideWindow(int windowID)
        {
            GUILayout.BeginVertical();
            showWorldWindow = GUILayout.Toggle(showWorldWindow, "Main Window", gradientButtonStyle, GUILayout.Height(25));
            OnSideWindowGUI_World?.Invoke(this, EventArgs.Empty);
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>
        /// Main window for the World Scene
        /// </summary>
        /// <param name="windowID"></param>
        private void WorldMainWindow(int windowID)
        {
            GUILayout.BeginVertical();
            worldMainScrollPosition = GUILayout.BeginScrollView(worldMainScrollPosition);
            // PUT MAIN GUI CONTENT HERE
            GUILayout.Label("Mod Menus", windowTitleStyle);

            OnMainWindowGUI_World?.Invoke(this, EventArgs.Empty);

            GUILayout.EndScrollView();

            // Close Button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            showWorldWindow = GUILayout.Toggle(showWorldWindow, "Close", gradientButtonStyle, GUILayout.Height(30), GUILayout.Width(175));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }


        // GUI Styles
        public static GUIStyle windowTitleStyle = new GUIStyle();
        public static GUIStyle windowBackgroundStyle = new GUIStyle();
        public static GUIStyle gradientButtonStyle = new GUIStyle();
        public static GUIStyle h1Style = new GUIStyle();
        public static GUIStyle h2Style = new GUIStyle();
        public static GUIStyle whiteStyle = new GUIStyle();
        public static GUIStyle gradientVerticalStyle = new GUIStyle();



        // GUI Textures
        public static Texture2D bgBlueTexture = new Texture2D(128, 128);
        public static Texture2D darkBlueTexture = new Texture2D(128, 128);
        public static Texture2D buttonGradientTexture = new Texture2D(128, 128);
        public static Texture2D whiteTexture = new Texture2D(128, 128);
        public static Texture2D blackTexture = new Texture2D(128, 128);
        public static Texture2D transparentTexture = new Texture2D(128, 128);


        // VARIABLES: OLD Styles and textures

        public static GUIStyle desciptionStyle = new GUIStyle();
        public static GUIStyle theRestStyle = new GUIStyle();
        public static GUIStyle rewardsCostStyle = new GUIStyle();

        public static GUIStyle greenStyle = new GUIStyle();
        public static Texture2D greenTexture = new Texture2D(128, 128);
        public static GUIStyle blackStyle = new GUIStyle();

        public static GUIStyle blankStyle = new GUIStyle();
        public static GUIStyle CareerModeButtonStyle = new GUIStyle();
        public static Texture2D greyTexture = new Texture2D(128, 128);

        //Create textures for all of the GUI.Styles
        public void Create_Textures()
        {
            for (int y = 0; y < whiteTexture.height; ++y)
            {
                for (int x = 0; x < whiteTexture.width; ++x)
                {
                    Color color = Color.white;
                    whiteTexture.SetPixel(x, y, color);
                }
            }
            whiteTexture.Apply();

            for (int y = 0; y < transparentTexture.height; ++y)
            {
                for (int x = 0; x < transparentTexture.width; ++x)
                {
                    Color color = new Color32(0,0,0,0);
                    transparentTexture.SetPixel(x, y, color);
                }
            }
            transparentTexture.Apply();

            for (int y = 0; y < bgBlueTexture.height; ++y)
            {
                for (int x = 0; x < bgBlueTexture.width; ++x)
                {
                    Color color = new Color32(36, 42, 49, 255);
                    bgBlueTexture.SetPixel(x, y, color);
                }
            }
            bgBlueTexture.Apply();

            for (int y = 0; y < buttonGradientTexture.height; ++y)
            {
                int cornerRaduis = 10;
                Color color = Color32.Lerp(new Color32(12,21,34,255), new Color32(39, 48, 62, 255), (float)y / (float)buttonGradientTexture.height);
                for (int x = 0; x < buttonGradientTexture.width; ++x)
                {
                    buttonGradientTexture.SetPixel(x, y, color);
                }
            }
            buttonGradientTexture.Apply();

            for (int y = 0; y < darkBlueTexture.height; ++y)
            {
                int cornerRaduis = 10;
                Color color = new Color32(12, 21, 34, 255);
                for (int x = 0; x < darkBlueTexture.width; ++x)
                {
                    darkBlueTexture.SetPixel(x, y, color);
                }
            }
            darkBlueTexture.Apply();

            windowBackgroundStyle.normal.background = bgBlueTexture;
            windowBackgroundStyle.alignment = TextAnchor.MiddleCenter;

            gradientButtonStyle.normal.background = buttonGradientTexture;
            gradientButtonStyle.normal.textColor = Color.white;
            gradientButtonStyle.hover.background = whiteTexture;
            gradientButtonStyle.hover.textColor = Color.black;
            gradientButtonStyle.fontSize = 16;
            gradientButtonStyle.alignment = TextAnchor.MiddleCenter;
            gradientButtonStyle.fontStyle = FontStyle.Bold;
            gradientButtonStyle.margin = new RectOffset(10, 10, 10,10);

            gradientVerticalStyle.normal.background = buttonGradientTexture;
            gradientVerticalStyle.margin = new RectOffset(10, 10, 10, 10);
            gradientVerticalStyle.padding = new RectOffset(10, 10, 0, 0);

            h1Style.normal.background = transparentTexture;
            h1Style.normal.textColor = Color.white;
            h1Style.fontSize = 20;
            h1Style.alignment = TextAnchor.UpperCenter;
            h1Style.fontStyle = FontStyle.Bold;
            h1Style.margin = new RectOffset(10, 10, 10, 0);

            h2Style.normal.background = transparentTexture;
            h2Style.normal.textColor = Color.white;
            h2Style.fontSize = 16;
            h2Style.alignment = TextAnchor.UpperCenter;
            h2Style.fontStyle = FontStyle.Bold;
            h2Style.margin = new RectOffset(10, 10, 10, 0);

            windowTitleStyle.normal.background = darkBlueTexture;
            windowTitleStyle.normal.textColor = Color.white;
            windowTitleStyle.fontSize = 24;
            windowTitleStyle.alignment = TextAnchor.MiddleCenter;
            windowTitleStyle.fontStyle = FontStyle.Bold;
            windowTitleStyle.margin = new RectOffset(30, 30, 10, 10);
            windowTitleStyle.padding = new RectOffset(10, 10, 10, 10);

            //// OLD


            for (int y = 0; y < greenTexture.height; ++y)
            {
                for (int x = 0; x < greenTexture.width; ++x)
                {
                    Color color = Color.green;
                    greenTexture.SetPixel(x, y, color);
                }
            }
            greenTexture.Apply();

            for (int y = 0; y < blackTexture.height; ++y)
            {
                for (int x = 0; x < blackTexture.width; ++x)
                {
                    Color color = Color.black;
                    blackTexture.SetPixel(x, y, color);
                }
            }
            blackTexture.Apply();
            for (int y = 0; y < greyTexture.height; ++y)
            {
                for (int x = 0; x < greyTexture.width; ++x)
                {
                    Color color = new Color(1f, 1f, 1f, 0.5f);
                    greyTexture.SetPixel(x, y, color);
                }
            }
            greyTexture.Apply();

            whiteStyle.normal.background = whiteTexture;
            whiteStyle.normal.textColor = Color.black;
            whiteStyle.alignment = TextAnchor.MiddleCenter;
            greenStyle.normal.background = greenTexture;
            greenStyle.alignment = TextAnchor.MiddleCenter;
            greenStyle.normal.textColor = Color.black;
            blackStyle.normal.background = blackTexture;

            GUI.skin.verticalScrollbar.fixedWidth = Screen.width * 0.06f;
            GUI.skin.verticalScrollbarThumb.fixedWidth = Screen.width * 0.06f;
        }
    }
}
