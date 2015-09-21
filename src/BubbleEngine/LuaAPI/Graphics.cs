using System;
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
		public void drawString(LuaFont font, string text, int x, int y, LuaTable color)
		{
			font.Font.DrawString (sb, text, x, y, Util.ColorFromTable(color));
		}
		public void draw(LuaTexture tex, double x, double y, LuaTable color)
		{
			sb.Draw (tex.Texture, new Vector2 ((float)x, (float)y), Util.ColorFromTable(color));
		}
		public void drawSource(LuaTexture tex, LuaTable source, double x, double y, LuaTable color)
		{
			sb.Draw (tex.Texture, Util.RectangleFromTable (source), new Vector2((float)x, (float)y), Util.ColorFromTable (color));
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

