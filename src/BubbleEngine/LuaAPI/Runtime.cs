using System;
using System.Reflection;
namespace BubbleEngine.LuaAPI
{
	public class Runtime
	{
		public string GetEnumString(string assemblyName, string fullName, int value)
		{
			var asm = Assembly.Load (assemblyName);
			var t = asm.GetType (fullName);
			return Util.LowerFirst (Enum.GetName (t, value));
		}
	}
}

