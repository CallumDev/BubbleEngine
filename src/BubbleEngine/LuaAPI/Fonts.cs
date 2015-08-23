using System;

namespace BubbleEngine.LuaAPI
{
	public class Fonts
	{
		FontContext ctx;
		internal Fonts (FontContext context)
		{
			ctx = context;
		}
		public LuaFont load(string filename, int size)
		{
			return new LuaFont (new Font (ctx, filename, size));
		}
	}
	public class LuaFont
	{
		internal Font Font;
		internal LuaFont(Font fnt)
		{
			Font = fnt;
		}
	}
}

