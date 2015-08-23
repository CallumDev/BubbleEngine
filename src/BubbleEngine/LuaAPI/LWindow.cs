using System;

namespace BubbleEngine.LuaAPI
{
	public class LWindow
	{
		Window win;
		public LWindow (Window window)
		{
			win = window;
		}
		public void setTitle(string text)
		{
			win.Title = text;
		}
	}
}

