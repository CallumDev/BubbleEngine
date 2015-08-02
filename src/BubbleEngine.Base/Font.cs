using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

namespace BubbleEngine
{
	public class Font
	{
		const int PAGE_SIZE = 512; //Size of each texture

		class GlyphInfo
		{
			public IntPtr Face;
			public uint CharIndex;
			public Texture Texture;
			public Rectangle Rectangle;
			public bool Render; //Does this glyph have texture data? (space and tab do not.)
			public int AdvanceX;
			public int AdvanceY;
			public int HorizontalAdvance;
			public int XOffset;
			public int YOffset;
			//Big constructor!
			public GlyphInfo(
				Texture t, Rectangle r, int advanceX, 
				int advanceY, int horizontalAdvance, 
				int xoffset, int yoffset,  uint index, 
				IntPtr face
			)
			{
				Texture = t;
				Rectangle = r;
				Render = true;
				AdvanceX = advanceX;
				AdvanceY = advanceY;
				HorizontalAdvance = horizontalAdvance;
				XOffset = xoffset;
				YOffset = yoffset;
				CharIndex = index;
				Face = face;
			}
			//Little constructor for space + tab
			public GlyphInfo(int advanceX, int advanceY, uint index, IntPtr face)
			{
				Render = false;
				AdvanceX = advanceX;
				AdvanceY = advanceY;
				CharIndex = index;
				Face = face;
			}
		}

		IntPtr facePtr;
		FontContext context;
		List<Texture> pages = new List<Texture>();

		public Font (FontContext ctx, string filename, float size)
		{
			context = ctx;
			if (!File.Exists (filename))
				throw new FileNotFoundException ("Font not found " + filename, filename);

			var err = FT.FT_New_Face (context.Library, filename, 0, out facePtr);
			if (err != 0)
				throw new Exception ("Freetype Error");
			
			FT.FT_Set_Char_Size (facePtr,
				FTMath.To26Dot6 (0),
				FTMath.To26Dot6 (size),
				0,
				96
			);

			pages.Add (new Texture (
				PAGE_SIZE, PAGE_SIZE
			));

			//Rasterize standard ASCII
			for (int i = 32; i < 127; i++) {
				AddCharacter ((uint)i);
			}
		}
			
		void AddCharacter(uint codepoint)
		{
			
		}
	}
}

