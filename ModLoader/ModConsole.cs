using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using SFS.IO;

namespace ModLoader
{

    public class ModConsole
    {
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("Kernel32.dll")]
		private static extern bool AllocConsole();

		private static FilePath logFile;

		private const int CONSOLE_HIDE = 0;

		private const int CONSOLE_SHOW = 5;

		private bool visible = false;

		public ModConsole()
		{
			ModConsole.AllocConsole();
			Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
			{
				AutoFlush = true
			});
			this.visible = true;
			DateTime current = new DateTime();
			ModConsole.logFile = FileLocations.BaseFolder.Extend("logs").CreateFolder().ExtendToFile(current.Year + "-" + current.Month + "-" + current.Day+".txt");
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

		/// <summary>
		/// if you want to print a error with format
		/// </summary>
		/// <param name="e">Exception of your error</param>
		public void logError(Exception e)
		{
			StackTrace stackTrace = new StackTrace(e, true);
			StackFrame frame = stackTrace.GetFrame(0);
			int fileColumnNumber = frame.GetFileColumnNumber();
			int fileLineNumber = frame.GetFileLineNumber();
			string fileName = frame.GetFileName();
			this.log("##[ERROR]##", "ErrorReporter", LogType.Error);
			this.log(e.Message , "ErrorReporter", LogType.Error);
			this.log(e.StackTrace, "ErrorReporter", LogType.Error);
			this.log(fileLineNumber+":"+ fileColumnNumber + "@" + fileName, "ErrorReporter", LogType.Error);
			this.log("##[ERROR]##", "ErrorReporter", LogType.Error);
		}

		/// <summary>
		/// Print message in console with format 
		/// </summary>
		/// <param name="msg">Message that you want to ptint</param>
		/// <param name="tag">tag to identify your log</param>
		/// <param name="type">what kind of log is it</param>
		public void log(string msg, string tag, LogType type = LogType.Log)
		{
			string logMessage = "[" + tag + "]: " + msg;
			Console.WriteLine(logMessage);
			logFile.AppendText(logMessage+"\n");
		}

		/// <summary>
		///  Print message in console
		/// </summary>
		/// <param name="msg">message that you want to print</param>
		public void log(string msg)
		{
			this.log(msg, "Unkwn");
		}

	}
}
