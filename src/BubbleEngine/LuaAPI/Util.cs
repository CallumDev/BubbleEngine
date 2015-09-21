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
		delegate string GetStringDelegate(int v);
		public static void RegisterEnum(Type t, BubbleLua state)
		{
			//create table
			var typeName = LowerFirst (t.Name);
			state.Lua.NewTable ("__" + typeName);
			var table = (LuaTable)state.Lua ["__" + typeName];
			//values
			foreach (var val in Enum.GetValues(t)) {
				var n = Enum.GetName (t, val);
				table [LowerFirst (n)] = (int)val;
			}
			//getString function
			table ["getString"] = new GetStringDelegate((v) => LowerFirst(Enum.GetName(t,v)));
			state.Lua.DoString (string.Format(@"
				{0} = bubbleinternal.readonlytable(__{0})
				__{0} = nil
			",typeName));
		}

		public static string LowerFirst(string str)
		{
			var b = new StringBuilder (str);
			b [0] = char.ToLowerInvariant (b [0]);
			return b.ToString ();
		}
	}
}

