using System;
using System.IO;
using System.Diagnostics;
namespace BubbleEngine
{
	public static class MessageBox
	{
		public static void ShowError(string title, string text, Window parent = null)
		{
			//output to stderr
			Console.Error.WriteLine("Error: {0}", text);
			//try and show a dialog box
			if (Platform.CurrentPlatform == Platforms.Linux) {
				//For some reason, SDL2 doesn't provide SDL2_ShowSimpleMessageBox in Fedora
				if (File.Exists ("/usr/bin/zenity")) {
					var p = Process.Start ("/usr/bin/zenity", "--error --text=\"" + text + "\"");
					p.WaitForExit ();
				} else if (File.Exists ("/usr/bin/xmessage")) {
					var p = Process.Start ("/usr/bin/xmessage", "\"" + text + "\"");
					p.WaitForExit ();
				}
			} else {
				ShowError_SDL (title, text, parent);
			}
		}
		static void ShowError_SDL(string title, string text, Window parent = null)
		{
			if (!SDL2.Loaded)
				SDL2.Load ();
			var result = SDL2.SDL_ShowSimpleMessageBox (SDL2.SDL_MESSAGEBOX_ERROR, title, text, parent == null ? IntPtr.Zero : parent.Handle);
			if (result < 0) {
				throw new Exception (SDL2.GetErrorString ());
			}
		}
	}
}

