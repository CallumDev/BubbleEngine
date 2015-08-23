﻿using System;
using NLua;
namespace BubbleEngine.LuaAPI
{
	public class LuaTexture
	{
		internal Texture Texture;
		internal LuaTexture(Texture tex)
		{
			Texture = tex;
		}
		public int width {
			get {
				return Texture.Width;
			}
		}
		public int Height {
			get {
				return Texture.Height;
			}
		}
	}
	public class Graphics
	{
		SpriteBatch sb;
		internal Graphics (SpriteBatch spriteBatch)
		{
			sb = spriteBatch;
		}
		public void drawString(LuaFont font, string text, int x, int y)
		{
			font.Font.DrawString (sb, text, x, y, Color4.White);
		}
		public void draw(LuaTexture tex, double x, double y)
		{
			sb.Draw (tex.Texture, new Vector2 ((float)x, (float)y), Color4.White);
		}
		public void fillRectangle(double x, double y, double w, double h, LuaTable color)
		{
			if (color == null) {
				Console.WriteLine ("Null color");
				throw new Exception ();
			}
			sb.FillRectangle (
				new Rectangle (
					(int)x, (int)y, (int)w, (int)h
				), 
				Util.ColorFromTable (color)
			);
		}
		public LuaTexture newTexture(string filename)
		{
			return new LuaTexture (TextureLoader.FromFile (filename));
		}
	}
}
