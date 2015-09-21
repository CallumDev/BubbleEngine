using System;

namespace BubbleEngine
{
	public static class MessageBox
	{
		public static void ShowError(string title, string text, Window parent = null)
		{
			if (!SDL2.Loaded)
				SDL2.Load ();
			SDL2.SDL_ShowSimpleMessageBox (SDL2.SDL_MESSAGEBOX_ERROR, title, text, parent == null ? IntPtr.Zero : parent.Handle);
		}
	}
}

