using System;
using System.Text;
using NLua;

namespace BubbleEngine.LuaAPI
{

	static class Util
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
		public static Rectangle RectangleFromTable(LuaTable l)
		{
			Rectangle r = new Rectangle ();
			r.X = (int)(double)l [1];
			r.Y = (int)(double)l [2];
			r.Width = (int)(double)l [3];
			r.Height = (int)(double)l [4];
			return r;
		}
		delegate string GetStringDelegate(int v);
		static Random rand = new Random();
		public static void RegisterEnum(Type t, BubbleLua state)
		{
			string randName = "_" + rand.Next ();
			//create table
			var typeName = LowerFirst (t.Name);
			state.Lua.NewTable (randName);
			var table = (LuaTable)state.Lua [randName];
			//values
			foreach (var val in Enum.GetValues(t)) {
				var n = Enum.GetName (t, val);
				table [LowerFirst (n)] = (int)val;
			}
			//getString function
			table ["getString"] = new GetStringDelegate((v) => LowerFirst(Enum.GetName(t,v)));
			var result = state.Lua.DoString (string.Format (@"return bubbleinternal.readonlytable({0})",randName));
			state.Bubble [typeName] = (LuaTable)result [0];
			state.Lua.DoString (string.Format ("{0} = nil", randName));
		}

		public static string LowerFirst(string str)
		{
			var b = new StringBuilder (str);
			b [0] = char.ToLowerInvariant (b [0]);
			return b.ToString ();
		}
	}
}

