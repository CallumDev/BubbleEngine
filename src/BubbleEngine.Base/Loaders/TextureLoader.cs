using System;
using System.IO;
namespace BubbleEngine
{
	public static class TextureLoader
	{
		public static Texture FromFile(string filename)
		{
			using (var s = File.OpenRead (filename)) {
				//load png
				if (PngLoader.IsPng (s)) {
					return PngLoader.LoadPng (s);
				}
				//unrecognised file format
				throw new Exception("Unrecognised file format " + filename);
			}
		}
	}
}

