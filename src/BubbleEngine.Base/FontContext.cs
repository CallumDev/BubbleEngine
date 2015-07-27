using System;
using System.IO;

namespace BubbleEngine
{
	//Provides an FT_Library object
	public class FontContext : IDisposable
	{
		internal IntPtr Library = IntPtr.Zero;
		internal IntPtr Fallback = IntPtr.Zero;

		internal FontContext ()
		{
			if (!FT.Loaded) //Load freetype bindings if they aren't already loaded
				FT.Load ();
			
			var err = FT.FT_Init_FreeType (out Library);
			if (err != 0)
				throw new Exception ("Freetype Error");
		}

		//Load a font face to fallback on for unsupported characters
		public void LoadFallback(string filename)
		{
			if (!File.Exists (filename))
				throw new FileNotFoundException ("Font not found " + filename, filename);
			
			var err = FT.FT_New_Face (Library, filename, 0, out Fallback);
			if (err != 0)
				throw new Exception ("Freetype Error");
		}

		public void Dispose ()
		{
			FT.FT_Done_FreeType (Library);
			Library = IntPtr.Zero;
			Fallback = IntPtr.Zero;

		}
	}
}

