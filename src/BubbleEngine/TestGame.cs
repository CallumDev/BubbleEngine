using System;

namespace BubbleEngine
{
	public class TestGame : GameBase
	{
		SpriteBatch spriteBatch;
		Font font;
		public TestGame ()
		{
			Window.Title = "Bubble Test";
		}

		protected override void Load ()
		{
			spriteBatch = new SpriteBatch (Window);
			//load fonts
			FontContext.LoadFallback("../../TestAssets/DroidSansFallback.ttf");
			font = new Font (FontContext, "../../TestAssets/OpenSans-Regular.ttf", 48);
		}

		protected override void Draw (GameTime gameTime)
		{
			//Test: red, blue, green + white rectangles
			spriteBatch.Begin ();
			spriteBatch.FillRectangle (new Rectangle (0, 0, 400, 300), Color4.Red);
			spriteBatch.FillRectangle (new Rectangle (400, 0, 400, 300), Color4.Blue);
			spriteBatch.FillRectangle (new Rectangle (0, 300, 400, 300), Color4.Green);
			spriteBatch.FillRectangle (new Rectangle (400, 300, 400, 300), Color4.White);
			//test text rendering
			font.DrawString(spriteBatch, "Hello World!",30,30, Color4.Black);
			font.DrawString(spriteBatch, "Hello World!",28,28, Color4.White);
			spriteBatch.End ();
		}
	}
}

