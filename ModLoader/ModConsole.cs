using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using SFS.IO;

namespace ModLoader
{

    public class ModConsole : MonoBehaviour
	{
		public static GameObject root;

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("Kernel32.dll")]
		private static extern bool AllocConsole();

		private FilePath logFile;

		private const int CONSOLE_HIDE = 0;

		private const int CONSOLE_SHOW = 5;

		private bool visible = false;

		private void Awake()
		{
			ModConsole.AllocConsole();
			Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
			{
				AutoFlush = true
			});
			this.visible = true;
			string date = string.Format("{0:yyyy_MM_dd}", DateTime.UtcNow);
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

		/// <summary>
		/// call this method if you want to hiden or show console
		/// </summary>
		public void toggleConsole()
		{
			if (this.visible)
			{
				ModConsole.ShowWindow(ModConsole.GetConsoleWindow(), CONSOLE_HIDE);
			}
			else
			{
				ModConsole.ShowWindow(ModConsole.GetConsoleWindow(), CONSOLE_SHOW);
			}
			this.visible = !this.visible;
		}

		private void log(string message)
		{
			Console.WriteLine($"LOG: {message}");
			this.logFile.AppendText($"LOG: {message} \n");
		}

		private void error(string message)
		{
			Console.WriteLine($"ERROR: {message}");
			this.logFile.AppendText($"ERROR: {message}\n");
		}

		private void exception(string message, string stackTrace)
		{
			Console.WriteLine($"EXCEPTION: {message}");
			Console.WriteLine(stackTrace);
			this.logFile.AppendText($"EXCEPTION: {message}\n");
			this.logFile.AppendText(stackTrace);
		}

		private void warning(string message, string stackTrace)
		{
			Console.WriteLine($"WARNING: {message}");
			Console.WriteLine(stackTrace);
			this.logFile.AppendText($"WARNING: {message}\n");
			this.logFile.AppendText(stackTrace);
		}

		private void handleLog(string message, string stackTrace, LogType type)
		{
			switch (type)
			{
				case LogType.Error:
					this.error(message);
					break;
				case LogType.Exception:
					this.exception(message, stackTrace);
					break;
				case LogType.Warning:
					this.warning(message, stackTrace);
					break;
				default:
					this.log(message);
					break;
			}
			
		}

	}
}
