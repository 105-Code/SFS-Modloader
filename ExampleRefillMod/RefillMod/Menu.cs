
using SFS.Input;
using SFS.Parts.Modules;
using SFS.World;
using System;
using UnityEngine;
using static UnityEngine.GUI;

namespace RefillMod
{
    class Menu : MonoBehaviour
    {

        private const float MENU_HEIGHT =90f;
        private const float MENU_WIDTH = 200f;

        public static Rocket playerRocket;

        public Rect windowRect = new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.05f, MENU_WIDTH, MENU_HEIGHT);

        private bool isVisible;
        private WindowFunction windosDraw;

        private void Start()
        {
            KeysNode keysNode = GameManager.main.world_Input.keysNode;
            KeybindingsPC.Key menuAction = KeyCode.O;
            keysNode.AddOnKeyDown(menuAction, new Action(this.toggleMenu));
            this.isVisible = true;
            this.windosDraw = new GUI.WindowFunction(this.windowFunc);
        }

        public void toggleMenu()
        {
            this.isVisible = !this.isVisible;
        }

        public void OnGUI()
        {
            if (this.isVisible)
            {
                windowRect = GUI.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, this.windosDraw, "RefillMod(Beta)");
            }
        }

        public void windowFunc(int windowID)
        {
            GUI.Label(new Rect(10f, 20f, MENU_WIDTH - 10f, 20f), "Hide this: Press 'O'");
            if (GUI.Button(new Rect(10f, 50f, MENU_WIDTH - 20f, 20f), "Refill tanks"))
            {
                foreach (ResourceModule tank in Menu.playerRocket.partHolder.GetModules<ResourceModule>())
                {
                    tank.AddResource(1000f);
                }
            }
            GUI.DragWindow();
        }

    }


}
