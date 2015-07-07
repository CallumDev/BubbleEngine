using System;

namespace BubbleEngine
{
	public class GameTime
	{
		public TimeSpan ElapsedTime { get; private set; }
		public TimeSpan TotalTime { get; private set; }
		public GameTime (TimeSpan elapsed, TimeSpan total)
		{
			ElapsedTime = elapsed;
			TotalTime = total;
		}
	}
}

