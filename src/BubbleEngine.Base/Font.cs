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
			public bool Kerning;
			//Big constructor!
			public GlyphInfo(
				Texture t, Rectangle r, int advanceX, 
				int advanceY, int horizontalAdvance, 
				int xoffset, int yoffset,  uint index, 
				IntPtr face, bool kerning
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
				Kerning = kerning;
			}
			//Little constructor for space + tab
			public GlyphInfo(int advanceX, int advanceY, uint index, IntPtr face, bool kerning)
			{
				Render = false;
				AdvanceX = advanceX;
				AdvanceY = advanceY;
				CharIndex = index;
				Face = face;
				Kerning = kerning;
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

		public int LineHeight {
			get {
				return lineHeight;
			}
		}

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

		public Point MeasureString(string text)
		{
			if (text == "") //Skip empty strings
				return new Point (0, 0);
			
			var iter = new CodepointIterator (text);
			float penX = 0, penY = 0;

			while (iter.Iterate ()) {
				uint c = iter.Codepoint;
				if (c == (uint)'\n') {
					penY += lineHeight;
					penX = 0;
					continue;
				}
				var glyph = GetGlyph (c);
				if (glyph.Render) {
					penX += glyph.HorizontalAdvance;
					penY += glyph.AdvanceY;
				} else {
					penX += glyph.AdvanceX;
					penY += glyph.AdvanceY;
				}
				if (glyph.Kerning && iter.Index < iter.Count - 1) {
					var g2 = GetGlyph (iter.PeekNext ());
					if (g2.Face == glyph.Face) {
						FT.FT_Vector vec;
						FT.FT_Get_Kerning (glyph.Face, glyph.CharIndex, g2.CharIndex, 2, out vec);
						var krn = FTMath.From26Dot6 (vec.x);
						penX += krn;
					}
				}
			}
			return new Point ((int)penX, (int)penY);
		}

		public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color4 color)
		{
			DrawString (spriteBatch, text, (int)position.X, (int)position.Y, color);
		}

		public void DrawString(SpriteBatch spriteBatch, string text, int x, int y, Color4 color)
		{
			if (text == "") //Skip empty strings
				return;
			
			var iter = new CodepointIterator (text);
			float penX = x, penY = y;

			while (iter.Iterate ()) {
				uint c = iter.Codepoint;
				if (c == (uint)'\n') {
					penY += lineHeight;
					penX = x;
				}
				var glyph = GetGlyph (c);
				if (glyph.Render) {
					spriteBatch.Draw (
						glyph.Texture,
						glyph.Rectangle,
						new Rectangle (
							(int)penX + glyph.XOffset,
							(int)penY + (LineHeight - glyph.YOffset),
							glyph.Rectangle.Width,
							glyph.Rectangle.Height
						),
						color
					);
					penX += glyph.HorizontalAdvance;
					penY += glyph.AdvanceY;
				} else {
					penX += glyph.AdvanceX;
					penY += glyph.AdvanceY;
				}
				if (glyph.Kerning && iter.Index < iter.Count - 1) {
					var g2 = GetGlyph (iter.PeekNext ());
					if (g2.Face == glyph.Face) {
						FT.FT_Vector vec;
						FT.FT_Get_Kerning (glyph.Face, glyph.CharIndex, g2.CharIndex, 2, out vec);
						var krn = FTMath.From26Dot6 (vec.x);
						penX += krn;
					}
				}
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
				glyphs.Add (codepoint, new GlyphInfo (spaceGlyph.AdvanceX * 4, spaceGlyph.AdvanceY, spaceGlyph.CharIndex, spaceGlyph.Face, spaceGlyph.Kerning));
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
			//not exactly the right spot, but this is the only place we access the members of the face
			bool kerning = (((long)faceRec.face_flags) & FT.FT_FACE_FLAG_KERNING) == FT.FT_FACE_FLAG_KERNING;
			FT.FT_Render_Glyph (faceRec.glyph, FT.FT_RENDER_MODE_NORMAL);
			var glyphRec = Marshal.PtrToStructure<FT.GlyphSlotRec> (faceRec.glyph);
			if (glyphRec.bitmap.width == 0 || glyphRec.bitmap.rows == 0) {
				glyphs.Add (codepoint,
					new GlyphInfo (
						(int)Math.Ceiling(FTMath.From26Dot6 (glyphRec.advance.x)),
						(int)Math.Ceiling(FTMath.From26Dot6 (glyphRec.advance.y)),
						index,
						face,
						kerning
					)
				);
			} else {
				var colors = new ByteColor[glyphRec.bitmap.width * glyphRec.bitmap.rows];
				if (glyphRec.bitmap.pixel_mode == 2) {
					byte* data = (byte*)glyphRec.bitmap.buffer;
					for (int i = 0; i < glyphRec.bitmap.width * glyphRec.bitmap.rows; i++) {
						//TODO: 4 bytes used for 1 byte of alpha data? investigate compression with GL_RED and shader.
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
				glyphs.Add (codepoint, 
					new GlyphInfo (
						tex, 
						rect, 
						(int)Math.Ceiling (FTMath.From26Dot6 (glyphRec.advance.x)),
						(int)Math.Ceiling (FTMath.From26Dot6 (glyphRec.advance.y)),
						(int)Math.Ceiling (FTMath.From26Dot6 (glyphRec.metrics.horiAdvance)),
						glyphRec.bitmap_left,
						glyphRec.bitmap_top,
						index,
						face,
						kerning
					)
				);
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

