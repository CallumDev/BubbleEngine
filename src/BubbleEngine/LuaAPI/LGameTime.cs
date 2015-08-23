using System;

namespace BubbleEngine.LuaAPI
{
	public class LGameTime
	{
		public double elapsed;
		public double total;
		public LGameTime (GameTime gameTime)
		{
			elapsed = gameTime.ElapsedTime.TotalSeconds;
			total = gameTime.TotalTime.TotalSeconds;
		}
	}
}

