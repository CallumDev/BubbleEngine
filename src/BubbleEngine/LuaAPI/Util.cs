using System;
using System.Text;
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
		public static void RegisterEnum(Type t, Lua state)
		{
			var typeName = LowerFirst (t.Name);
			state.DoString (typeName + " = {}");
			foreach (var val in Enum.GetValues(t)) {
				var n = Enum.GetName (t, val);
				state.DoString (typeName + "." + LowerFirst (n) + " = " + (int)val);
			}
			//getstring
			var builder = new StringBuilder ();
			builder.Append ("function ").Append (typeName).AppendLine (".getString (v)");
			builder.Append ("    return runtime:GetEnumString(\"");
			builder.Append (t.Assembly.FullName).Append ("\", \"");
			builder.Append (t.FullName).AppendLine ("\", v)");
			builder.AppendLine ("end");
			Console.WriteLine (builder.ToString ());
			state.DoString (builder.ToString ());
		}
		public static string LowerFirst(string str)
		{
			var b = new StringBuilder (str);
			b [0] = char.ToLowerInvariant (b [0]);
			return b.ToString ();
		}
	}
}

