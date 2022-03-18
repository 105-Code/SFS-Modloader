using SFS.IO;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ModLoader.IO
{

	public class Console : MonoBehaviour
	{
		public static Console Main;
		public static GameObject root;

		private ConsoleGUI _consoleGui;
		public ConsoleGUI ConsoleGui
		{
			get { return this._consoleGui; }
		}
		private const int _maxLines = 150;
		private FilePath logFile;
		private Queue<string> _queue;
		private string _lastLog;
		private StringBuilder _logs;


		private void Awake()
		{
			Console.Main = this;
			this._queue = new Queue<string>();
			this._consoleGui = this.gameObject.AddComponent<ConsoleGUI>();
			this._logs = new StringBuilder();
			string date = string.Format("{0:yyyy-MM-dd}", DateTime.UtcNow);
			this.logFile = FileLocations.BaseFolder.Extend("logs").CreateFolder().ExtendToFile(date + ".txt");
			//Clear log content on startup
			logFile.WriteText("");
		}

		private void OnEnable()
		{
			Application.logMessageReceivedThreaded += this.handleLog;
		}

		private void OnDisable()
		{
			Application.logMessageReceivedThreaded -= this.handleLog;
		}

		/// <summary>
		/// process all game logs
		/// </summary>
		/// <param name="message">log message </param>
		/// <param name="stackTrace"> error stackTrace </param>
		/// <param name="type"> what type of log is it</param>
		private void handleLog(string message, string stackTrace, LogType type)
		{

			if (message == this._lastLog)
			{
				return;
			}

			// Delete oldest message
			if (this._queue.Count >= _maxLines)
			{
				this._queue.Dequeue();
			}
			string time = DateTime.UtcNow.ToString("HH:mm:ss");
			string log;
			switch (type)
			{
				case LogType.Error:
					log = $"[{time}] ERROR: {message}\n";
					break;
				case LogType.Exception:
					log = $"[{time}] EXCEPTION: {message}\n{stackTrace}";
					break;
				case LogType.Warning:
					log = $"[{time}] WARNING: {message}\n{stackTrace}";
					break;
				default:
					log = $"[{time}] LOG: {message}\n";
					break;
			}

			this._queue.Enqueue(log);
			this.logFile.AppendText(log);
			this._lastLog = message;
			this._logs.Clear();
			foreach (string logString in this._queue)
			{
				this._logs.Append(logString);
			}
			this._consoleGui.Logs = this._logs.ToString();

		}
		
	}
}