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

		private void hideConsole()
		{
			ModConsole.ShowWindow(ModConsole.GetConsoleWindow(), 0);
			this.visible = false;
		}

		private void showConsole()
		{
			ModConsole.ShowWindow(ModConsole.GetConsoleWindow(), 5);
			this.visible = true;
		}

		public void toggleConsole()
		{
			bool flag = this.visible;
			if (flag)
			{
				this.hideConsole();
			}
			else
			{
				this.showConsole();
			}
		}

		public void logError(Exception e)
		{
			StackTrace stackTrace = new StackTrace(e, true);
			StackFrame frame = stackTrace.GetFrame(0);
			int fileColumnNumber = frame.GetFileColumnNumber();
			int fileLineNumber = frame.GetFileLineNumber();
			string fileName = frame.GetFileName();
			this.tryLogCustom("##[ERROR]##", "ErrorReporter", LogType.Error);
			this.tryLogCustom(e.Message , "ErrorReporter", LogType.Error);
			this.tryLogCustom(e.StackTrace, "ErrorReporter", LogType.Error);
			this.tryLogCustom(fileLineNumber+":"+ fileColumnNumber + "@" + fileName, "ErrorReporter", LogType.Error);
			this.tryLogCustom("##[ERROR]##", "ErrorReporter", LogType.Error);
		}

		public void log(string msg, string tag)
		{
			string logMessage = "[" + tag + "]: " + msg;
			Console.WriteLine(logMessage);
			logFile.AppendText(logMessage+"\n");
		}

		public void log(string msg)
		{
			this.log(msg, "Unkwn");
		}

		private void tryLogCustom(string msg, string tag, LogType type)
		{
			bool flag = this.logCustom == null;
			if (flag)
			{
				this.log(msg, tag);
			}
			else
			{
				msg = "[" + tag + "]: " + msg;
				try
				{
					this.logCustom(msg, type);
				}
				catch (Exception e)
				{
					this.logError(e);
				}
			}
		}

		private void tryLogCustom(string msg, LogType type)
		{
			this.tryLogCustom(msg, "Unkwn", type);
		}

		public void setLogger(Action<string, LogType> logfunc)
		{
			this.logCustom = logfunc;
		}

		private const int SW_HIDE = 0;

		private const int SW_SHOW = 5;

		private bool visible = false;


		private Action<string, LogType> logCustom;
	}
}
