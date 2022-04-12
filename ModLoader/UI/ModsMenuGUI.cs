
using SFS.Translations;
using UnityEngine;

namespace ModLoader.UI
{
    class ModsMenuGUI : MonoBehaviour
    {

        private Vector2 _scroll;

        private GUIStyle _windowStyle;
        private GUIStyle _modInformationStyle;
        private GUIStyle _titleStyle;
        private GUIStyle _modInformationTextStyle;

        private Rect _windowsPosition;
        private const float _margin = 0.2f;

        private void Awake()
        {
            this._scroll = Vector2.zero;

            this._windowStyle = new GUIStyle();
            this._modInformationStyle = new GUIStyle();
            this._titleStyle = new GUIStyle();
            this._modInformationTextStyle = new GUIStyle();

        }

        private void Start()
        {
            this._windowStyle.normal.background = this.makeTexture(new Color32(14, 17,27,230));

            this._modInformationStyle.normal.background = this.makeTexture(new Color32(27, 32, 50, 240));
            this._modInformationStyle.padding = new RectOffset(10,10,10,10);
            this._modInformationStyle.margin.top = 10;

            this._modInformationTextStyle.fontSize = 20;
            this._modInformationTextStyle.padding.top = 3;
            this._modInformationTextStyle.normal.textColor = Color.white;

            this._titleStyle.fontSize = 30;
            this._titleStyle.fontStyle = FontStyle.Bold;
            this._titleStyle.normal.textColor = Color.white;
            this._titleStyle.alignment = TextAnchor.MiddleCenter;

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

        private void OnEnable()
        {

            float windowsHeigth = Screen.height * (1 - _margin - _margin);
            float windowsWidth = Screen.width * (1 - _margin - _margin);
            float windowsX = Screen.width * _margin;
            float windowsY = Screen.height * _margin;

            this._windowsPosition = new Rect(windowsX, windowsY, windowsWidth,windowsHeigth);
    }


        private void OnGUI()
        {
            GUI.BeginGroup(this._windowsPosition, this._windowStyle);
            GUILayout.Label("Installed Mods",this._titleStyle);
            this._scroll = GUILayout.BeginScrollView(this._scroll, GUILayout.Width(this._windowsPosition.width),GUILayout.Height(this._windowsPosition.height - 70));

            foreach(SFSMod mod in Loader.main.getMods())
            {
                this.modInformation(mod);
            }

            GUILayout.EndScrollView();
            bool click = GUILayout.Button(Loc.main.Close);
            GUI.EndGroup();
            if (click)
            {
                ModsMenu.Main.Close();
                ModsMenu.Main.OnClose();
            }
        }

        private void modInformation(SFSMod mod)
        {

            GUILayout.BeginVertical(this._modInformationStyle,GUILayout.Width(this._windowsPosition.width-20));

            GUILayout.Label($"Mod: {mod.Name} {mod.Version}",this._modInformationTextStyle);
            GUILayout.Label($"Author: {mod.Author}", this._modInformationTextStyle);
            string description = mod.Description != "" ? mod.Description : "N/A";
            GUILayout.Label($"Description:\n{description}", this._modInformationTextStyle);
            //bool click = GUILayout.Button("Config");
            GUILayout.EndVertical();
        }
    }
}
