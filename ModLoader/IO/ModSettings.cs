using ModLoader.Mod;
using SFS.Input;
using SFS.IO;
using SFS.Parsers.Json;
using SFS.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace ModLoader.IO
{
    /// <summary>
    ///     This class handles the Keybinding buttons in the settings menu. You can also load your configuration and store it if it changes.
    /// </summary>
    public class ModSettings : MonoBehaviour
    {
        /// <summary>
        ///     Main instance of ModSettings. Use this to load settings or add the Keybidings buttons.
        /// </summary>
        public static ModSettings main;

        private Transform _keybindingsHolder;
        private GameObject _textPrefab;
        private GameObject _spacePrefab;
        private GameObject _keybindingPrefab;
        private Dictionary<string, List<KeyBinder>> _elements = new Dictionary<string, List<KeyBinder>>();

        /// <summary>
        ///    This prefab is used to add text in the settings menu
        /// </summary>
        public GameObject TextPrefab
        {
            set { this._textPrefab = value; }
        }

        /// <summary>
        ///     This prefab is used to add space in the settings menu
        /// </summary>
        public GameObject SpacePrefab
        {
            set { this._spacePrefab = value; }
        }

        /// <summary>
        ///     This prefab is used to add keybinding buttons in the settings menu
        /// </summary>
        public GameObject KeybindingPrefab
        {
            set { this._keybindingPrefab = value; }
        }

        /// <summary>
        ///     This transforms keep all keybiding buttons.
        /// </summary>
        public Transform KeybindingsHolder
        {
            set { this._keybindingsHolder = value; }
        }

        private void Awake()
        {
            ModSettings.main = this;

        }

        /// <summary>
        ///     This method adds simple hotkey buttons in the settings menu
        /// </summary>
        /// <param name="key"> Your current key value </param>
        /// <param name="defaultKey"> Your default key value </param>
        /// <param name="name"> Your key name.</param>
        /// <param name="modSettings"> Your mod settings. This used to store your settings</param>
        /// <example>
        ///     ModSettings.main.addKeybinding( MyLoadedSettings.My_Key, MyDefaultSettings.My_Key, "My_Key", MyLoadedSettings);
        /// </example>
        /// <exception cref="NullReferenceException">If you call this method on early_load</exception>
        public void addKeybinding(KeybindingsPC.Key key, KeybindingsPC.Key defaultKey, string name, SFSSettings modSettings)
        {
            this.addKeybinding(new KeybindingsPC.Key[]
            {
                key
            }, new KeybindingsPC.Key[]
            {
                defaultKey
            }, name, modSettings);
        }

        /// <summary>
        ///     This method adds multiple hotkey buttons in the settings menu
        /// </summary>
        /// <param name="keys"> Your current key values </param>
        /// <param name="defaultKeys"> Your default key values </param>
        /// <param name="name"> Your key name.</param>
        /// <param name="modSettings"> Your mod settings. This used to store your settings</param>
        /// <example>
        ///     ModSettings.main.addKeybinding( MyLoadedSettings.My_Key, MyDefaultSettings.My_Key, "My_Key", MyLoadedSettings);
        /// </example>
        /// <exception cref="NullReferenceException">If you call this method on early_load</exception>
        public void addKeybinding(KeybindingsPC.Key[] keys, KeybindingsPC.Key[] defaultKeys, string name, SFSSettings modSettings)
        {
            // Code taken from SFS.Input.KeybindingsPC
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._keybindingPrefab, this._keybindingsHolder);
            
            // change name format
            if (name.Contains("_"))
            {
                StringBuilder stringBuilder = new StringBuilder(name[0].ToString());
                for (int i = 1; i < name.Length; i++)
                {
                    stringBuilder.Append(name[i].ToString().ToLower());
                }
                gameObject.GetComponentInChildren<TMP_Text>().text = stringBuilder.Replace("_", " ").ToString();
            }
            else
            {
                gameObject.GetComponentInChildren<TMP_Text>().text = name;
            }

            List<KeyBinder> list = new List<KeyBinder>
            {
                gameObject.GetComponentInChildren<KeyBinder>()
            };
            int num = 0;
            
            while (list.Count < keys.Length && num++ < 20)
            {
                list.Add(UnityEngine.Object.Instantiate<KeyBinder>(list[0], gameObject.transform));
            }
          
            for (int j = 0; j < keys.Length; j++)
            {
                KeybindingsPC.Key key = keys[j];
                KeybindingsPC.Key defaultKey = defaultKeys[j];

                // this action saves the key value when it changes
                Action save = () =>
                {
                    this.save(modSettings);
                };

                list[j].Initialize(Menu.settings.menuHolder, key, defaultKey, save );
            }

            List<KeyBinder> modList;
            if(this._elements.TryGetValue(modSettings.getModId(), out modList)){
                modList.AddRange(list);
            }
            else
            {
                this._elements.Add(modSettings.getModId(), list);
            }
        }

        /// <summary>
        ///     This method add text in the settings menu
        /// </summary>
        /// <param name="text">Text you want to add</param>
        public void addText(string text)
        {
            UnityEngine.Object.Instantiate<GameObject>(this._textPrefab, this._keybindingsHolder).GetComponentInChildren<TMP_Text>().text = text;
        }

        /// <summary>
        ///     This function resets all mod keybindings
        /// </summary>
        public void resetKeybindings()
        {
            foreach (List<KeyBinder> keyBinders in this._elements.Values)
            {
                foreach(KeyBinder keyBinder in keyBinders)
                {
                    keyBinder.ResetToDefault();
                }
            }
        }

        private void save(SFSSettings modSettings)
        {
            FolderPath folder = new FolderPath(modSettings.getModFolder()).Extend("Settings").CreateFolder();
            FilePath settingsFile = folder.ExtendToFile("keybindings.json");
            string text = JsonWrapper.ToJson(modSettings, true);
            settingsFile.WriteText(text);
        }

        /// <summary>
        ///     This method load the current keybindings.
        /// </summary>
        /// <typeparam name="T">Your keybinding class. This class has all keybindings your mod needs</typeparam>
        /// <param name="mod">Your mod instance. This is used to get your modid and modfolder</param>
        /// <returns> Return your currents keybinding</returns>
        /// <example>
        ///     MySettings currentSettings = ModSettings.main.loadSettings<MySettings>(MyMod);
        /// </example>
        public T loadSettings<T>(SFSMod mod)
        {
            FolderPath folder = new FolderPath(mod.ModFolder).Extend("Settings").CreateFolder();
            FilePath settingsFile = folder.ExtendToFile("keybindings.json");
            this._elements.Add(mod.ModId, new List<KeyBinder>());
            if (settingsFile.FileExists())
            {
                return JsonWrapper.FromJson<T>(settingsFile.ReadText());
            }

            return Activator.CreateInstance<T>();
        }

    }
}
