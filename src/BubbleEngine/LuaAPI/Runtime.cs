using System;
using System.Reflection;
namespace BubbleEngine.LuaAPI
{
	public class Runtime
	{
		public void logStringContents(string str)
		{
			foreach (var c in str) {
				Console.Write("{0}: {1}, ", c, (int)c);
			}
			Console.WriteLine ();
		}
	}
}

