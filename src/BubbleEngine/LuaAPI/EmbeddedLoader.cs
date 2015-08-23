using System;
using System.Reflection;
using System.Linq;
namespace BubbleEngine.LuaAPI
{
	public class EmbeddedLoader
	{
		public bool hasFile(string name)
		{
			var f = "BubbleEngine." + name + ".lua";
			var asm = Assembly.GetExecutingAssembly ();
			var names = asm.GetManifestResourceNames ();
			return names.Contains (f);
		}
		public string loadFile(string name)
		{
			var f = "BubbleEngine." + name + ".lua";
			return EmbeddedResources.GetString (f);
		}
	}
}

