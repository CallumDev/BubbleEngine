using System;
using System.IO;
namespace BubbleEngine
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 1) {
				MessageBox.ShowError ("Bubble Engine", "Incorrect number of arguments");
				return;
			}
			if (!File.Exists (args [0])) {
				MessageBox.ShowError (
					"Bubble Engine",
					string.Format ("Could not find file: {0}", Path.GetFullPath (args [0]))
				);
				return;
			}
			#if DEBUG
			new LuaGame (args[0]).Run ();
			#else
			try {
				new LuaGame(args[0]).Run();
			} catch (Exception ex) {
				MessageBox.ShowError(
					"Error",
					string.Format("{0}\nStack Trace:\n{1}",ex.Message, ex.StackTrace)
				);
			}
			#endif
		}
	}
}
