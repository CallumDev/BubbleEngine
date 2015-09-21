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
		public int height {
			get {
				return Texture.Height;
			}
		}
	}
	public struct LuaSize
	{
		public int width;
		public int height;
		public LuaSize(Point p)
		{
			width = p.X;
			height = p.Y;
		}
	}
	public class Graphics
	{
		public SpriteBatch Batch;
		GraphicsSettings settings;
		internal Graphics (SpriteBatch spriteBatch, GraphicsSettings gsettings)
		{
			Batch = spriteBatch;
			settings = gsettings;
		}
		public void drawString(LuaFont font, string text, int x, int y, LuaTable color)
		{
			font.Font.DrawString (Batch, text, x, y, Util.ColorFromTable(color));
		}
		public LuaSize measureString(LuaFont font, string text)
		{
			return new LuaSize (font.Font.MeasureString (text));
		}
		public void draw(LuaTexture tex, double x, double y, LuaTable color)
		{
			Batch.Draw (tex.Texture, new Vector2 ((float)x, (float)y), Util.ColorFromTable(color));
		}
		public void drawSource(LuaTexture tex, LuaTable source, double x, double y, LuaTable color)
		{
			Batch.Draw (tex.Texture, Util.RectangleFromTable (source), new Vector2((float)x, (float)y), Util.ColorFromTable (color));
		}
		public void fillRectangle(double x, double y, double w, double h, LuaTable color)
		{
			if (color == null) {
				Console.WriteLine ("Null color");
				throw new Exception ();
			}
			Batch.FillRectangle (
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
		public void setResolution(int width, int height)
		{
			settings.RequestedWidth = width;
			settings.RequestedHeight = height;
		}
	}
}

