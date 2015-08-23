using System;
using System.IO;
using System.Reflection;

namespace BubbleEngine
{
	public static class EmbeddedResources
	{
		public static string GetString(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();

			using (Stream stream = assembly.GetManifestResourceStream(name))
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}