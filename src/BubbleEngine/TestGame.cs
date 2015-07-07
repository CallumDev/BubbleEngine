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
			//Test: 400x300 red rectangle in top left corner
			spriteBatch.Begin ();
			spriteBatch.FillRectangle (new Rectangle (0, 0, 400, 300), Color4.Red);
			spriteBatch.End ();
		}
	}
}

