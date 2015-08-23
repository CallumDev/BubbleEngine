using System;
using NLua;

namespace BubbleEngine.LuaAPI
{
	public static class Util
	{
		public static Color4 ColorFromTable(LuaTable l)
		{
			Color4 c = Color4.White;
			c.R = (float)(double)l [1];
			c.G = (float)(double)l [2];
			c.B = (float)(double)l [3];
			c.A = (float)(double)l [4];
			return c;
		}
	}
}

