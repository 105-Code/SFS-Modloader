using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using SFS.IO;
using UnityEngine.SceneManagement;

namespace ModLoader
{

    public class Console : MonoBehaviour
	{
		public static GameObject root;
		private ConsoleGUI _consoleGui;
		public ConsoleGUI ConsoleGui
		{
			get
			{
				return this._consoleGui;
			}
		}

		private FilePath logFile;


		private void Awake()
		{
			Console.root.AddComponent<ConsoleGUI>();
			this._consoleGui = Console.root.GetComponent<ConsoleGUI>();
			string date = string.Format("{0:yyyy-MM-dd}", DateTime.UtcNow);
			this.logFile = FileLocations.BaseFolder.Extend("logs").CreateFolder().ExtendToFile(date+".txt");
		}
		

		private void OnEnable()
		{
			Application.logMessageReceivedThreaded += this.handleLog;
		}

		private void OnDisable()
		{
			Application.logMessageReceivedThreaded -= this.handleLog;
		}


		private void log(string time, string message)
		{
			this._consoleGui.Logs = $"[{time}] LOG: {message}\n";
			this.logFile.AppendText($"[{time}] LOG: {message} \n");
		}

		private void error(string time, string message)
		{
			this._consoleGui.Logs = $"[{time}] ERROR: {message}\n";
			this.logFile.AppendText($"[{time}] ERROR: {message}\n");
		}

		private void exception(string time, string message, string stackTrace)
		{
			this._consoleGui.Logs = $"[{time}] EXCEPTION: {message}\n";
			this._consoleGui.Logs = stackTrace;
			this.logFile.AppendText($"[{time}] EXCEPTION: {message}\n");
			this.logFile.AppendText(stackTrace);
		}

		private void warning(string time, string message, string stackTrace)
		{
			this._consoleGui.Logs = $"[{time}] WARNING: {message}\n";
			this._consoleGui.Logs = stackTrace;
			this.logFile.AppendText($"[{time}] WARNING: {message}\n");
			this.logFile.AppendText(stackTrace);
		}

		private void handleLog(string message, string stackTrace, LogType type)
		{
			string time = DateTime.UtcNow.ToString("HH:mm");
			switch (type)
			{
				case LogType.Error:
					this.error(time, message);
					break;
				case LogType.Exception:
					this.exception(time, message, stackTrace);
					break;
				case LogType.Warning:
					this.warning(time, message, stackTrace);
					break;
				default:
					this.log(time, message);
					break;
			}
			
		}

	}
}
