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
		IntPtr size26d6;
		FontContext context;
		List<Texture> pages = new List<Texture>();
		Dictionary<uint, GlyphInfo> glyphs = new Dictionary<uint, GlyphInfo>();
		int currentX = 0;
		int currentY = 0;
		int lineHeight;
		public Font (FontContext ctx, string filename, float size)
		{
			context = ctx;
			size26d6 = FTMath.To26Dot6 (size);
			if (!File.Exists (filename))
				throw new FileNotFoundException ("Font not found " + filename, filename);

			var err = FT.FT_New_Face (context.Library, filename, 0, out facePtr);
			if (err != 0)
				throw new Exception ("Freetype Error");
			
			FT.FT_Set_Char_Size (facePtr,
				FTMath.To26Dot6 (0),
				size26d6,
				0,
				96
			);
			//get metrics
			var faceRec = Marshal.PtrToStructure<FT.FaceRec> (facePtr);
			var szRec = Marshal.PtrToStructure<FT.SizeRec> (faceRec.size);
			lineHeight = (int)FTMath.From26Dot6 (szRec.metrics.height);
			pages.Add (new Texture (
				PAGE_SIZE, PAGE_SIZE
			));

			//Rasterize standard ASCII
			for (int i = 32; i < 127; i++) {
				AddCharacter ((uint)i);
			}

		}
		GlyphInfo GetGlyph(uint codepoint)
		{
			if (!glyphs.ContainsKey (codepoint))
				AddCharacter (codepoint);
			return glyphs [codepoint];
		}
		int lineMax = 0;
		unsafe void AddCharacter(uint codepoint)
		{
			if (codepoint == (uint)'\t') {
				var spaceGlyph = GetGlyph ((uint)' ');
				glyphs.Add (codepoint, new GlyphInfo (spaceGlyph.AdvanceX * 4, spaceGlyph.AdvanceY, spaceGlyph.CharIndex, spaceGlyph.Face));
				return;
			}
			IntPtr face;
			uint index;
			if (!GetFace (codepoint, out index, out face)) {
				if (codepoint == (uint)'?')
					throw new Exception ("Font does not have required ASCII characters");
				var qmGlyph = GetGlyph ((uint)'?');
				glyphs.Add (codepoint, qmGlyph);
				return;
			}
			FT.FT_Load_Glyph (face, index, FT.FT_LOAD_DEFAULT | FT.FT_LOAD_TARGET_NORMAL);
			var faceRec = Marshal.PtrToStructure<FT.FaceRec> (face);
			FT.FT_Render_Glyph (faceRec.glyph, FT.FT_RENDER_MODE_NORMAL);
			var glyphRec = Marshal.PtrToStructure<FT.GlyphSlotRec> (faceRec.glyph);
			if (glyphRec.bitmap.width == 0 || glyphRec.bitmap.rows == 0) {
				glyphs.Add (codepoint,
					new GlyphInfo (
						(int)Math.Ceiling(FTMath.From26Dot6 (glyphRec.advance.x)),
						(int)Math.Ceiling(FTMath.From26Dot6 (glyphRec.advance.y)),
						index,
						face
					)
				);
			} else {
				var colors = new ByteColor[glyphRec.bitmap.width * glyphRec.bitmap.rows];
				if (glyphRec.bitmap.pixel_mode == 2) {
					byte* data = (byte*)glyphRec.bitmap.buffer;
					for (int i = 0; i < glyphRec.bitmap.width * glyphRec.bitmap.rows; i++) {
						//TODO: 4 bytes used for 1 byte of alpha? investigate compression with GL_RED and shader.
						colors [i] = new ByteColor (255, 255, 255, data [i]);
					}
				} else {
					throw new NotImplementedException ();
				}
				if (currentX + glyphRec.bitmap.width > PAGE_SIZE) {
					currentX = 0;
					currentY += lineMax;
					lineMax = 0;
				}
				if (currentY + glyphRec.bitmap.rows > PAGE_SIZE) {
					currentX = 0;
					currentY = 0;
					lineMax = 0;
					pages.Add (new Texture (PAGE_SIZE, PAGE_SIZE));
					//TODO: new font page! should probably log this
				}
				lineMax = (int)Math.Max (lineMax, glyphRec.bitmap.rows);
				var rect = new Rectangle (currentX, currentY, glyphRec.bitmap.width, glyphRec.bitmap.rows);
				var tex = pages [pages.Count - 1];
				tex.SetData (colors, rect);
				currentX += glyphRec.bitmap.width;

			}
		}
		bool GetFace(uint codepoint, out uint index, out IntPtr face)
		{
			//use our font!
			index = FT.FT_Get_Char_Index (facePtr, codepoint);
			if (index != 0) {
				face = facePtr;
				return true;
			}
			//try fallback font
			if (context.Fallback == IntPtr.Zero) {
				face = IntPtr.Zero;
				return false;
			}
			FT.FT_Set_Char_Size (context.Fallback,
				FTMath.To26Dot6 (0),
				size26d6,
				0,
				96
			);
			index = FT.FT_Get_Char_Index (context.Fallback, codepoint);
			if (index != 0) {
				face = context.Fallback;
				return true;
			}
			//none of the fonts have this mysterious character
			face = IntPtr.Zero;
			return false;
		}
	}
}

