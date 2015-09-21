using System;
using System.IO;
using System.Text;
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
				var builder = new StringBuilder();
				//NLua has weird exceptions for when .NET code is called, report as best we can.
				string title = "Internal Error";
				if(ex is NLua.Exceptions.LuaException) {
					title = "Script Error";
					builder.Append(ex.Source);
					builder.AppendLine(ex.Message);
					builder.AppendLine();
					if(ex.InnerException != null) {
						builder.AppendLine(ex.InnerException.Message);
					}
				} else {
					builder.AppendLine(ex.Message);
					builder.AppendLine(ex.StackTrace);
				}
				//Put it on the terminal too
				Console.WriteLine(title);
				Console.WriteLine(builder.ToString());
				MessageBox.ShowError(
					title,
					builder.ToString()
				);
			}
			#endif
		}
	}
}
