using System;

namespace BubbleEngine
{
	public class TestGame : GameBase
	{
		SpriteBatch spriteBatch;

		public TestGame ()
		{
			Window.Title = "Bubble Test";
		}

		protected override void Load ()
		{
			spriteBatch = new SpriteBatch (Window);
		}

		protected override void Draw (GameTime gameTime)
		{
			//Test: red, blue, green + white rectangles
			spriteBatch.Begin ();
			spriteBatch.FillRectangle (new Rectangle (0, 0, 400, 300), Color4.Red);
			spriteBatch.FillRectangle (new Rectangle (400, 0, 400, 300), Color4.Blue);
			spriteBatch.FillRectangle (new Rectangle (0, 300, 400, 300), Color4.Green);
			spriteBatch.FillRectangle (new Rectangle (400, 300, 400, 300), Color4.White);
			spriteBatch.End ();
		}
	}
}

