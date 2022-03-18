

using SFS.Input;
using SFS.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ModLoader.UI
{
    /// <summary>
    /// This class is not used at the moment, because it is better to use it to display mod information and not UI for
    /// all mod interactions. Is necesary rework
    /// </summary>
    public class ModsMenu : BasicMenu
	{
		public static ModsMenu Main;
		
		private void Awake()
		{
			Main = this;
			base.menuHolder = new GameObject();
			base.menuHolder.SetActive(false);
			base.menuHolder.AddComponent<ModsMenuGUI>();
		}


		/*public void OpenMenu()
		{
			List<MenuElement> list = new List<MenuElement>();
			list.Add(TextBuilder.CreateText().Text(() => "Mods Installed"));

			SizeSyncerBuilder.Carrier sizeSync;
			list.Add(new SizeSyncerBuilder(out sizeSync).VerticalMode(SizeMode.MaxChildSize));


			foreach (SFSMod mod in Loader.main.getMods())
			{
				foreach (SFSMod mod2 in Loader.main.getMods())
				{
					list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod2.Name} {mod2.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
				}
				list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod.Name} {mod.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
			}

			foreach (SFSMod mod in Loader.main.getMods())
			{
				foreach (SFSMod mod2 in Loader.main.getMods())
				{
					list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod2.Name} {mod2.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
				}
				list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod.Name} {mod.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
			}

			foreach (SFSMod mod in Loader.main.getMods())
			{
				foreach (SFSMod mod2 in Loader.main.getMods())
				{
					list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod2.Name} {mod2.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
				}
				list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod.Name} {mod.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
			}

			foreach (SFSMod mod in Loader.main.getMods())
			{
				foreach (SFSMod mod2 in Loader.main.getMods())
				{
					list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod2.Name} {mod2.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
				}
				list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod.Name} {mod.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
			}
			foreach (SFSMod mod in Loader.main.getMods())
			{
				foreach (SFSMod mod2 in Loader.main.getMods())
				{
					list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod2.Name} {mod2.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
				}
				list.Add(ButtonBuilder.CreateButton(sizeSync, () => $"{mod.Name} {mod.Version}", () => { Debug.Log("Open COnfig"); }, CloseMode.None));
			}
			MenuGenerator.OpenMenu(CancelButton.Close, CloseMode.Current, list.ToArray());
		}*/

	}


	
}
