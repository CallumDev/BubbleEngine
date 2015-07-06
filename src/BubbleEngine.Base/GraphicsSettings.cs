using System;

namespace BubbleEngine
{
	public class GraphicsSettings
	{
		public int RequestedWidth { get; set; }
		public int RequestedHeight { get; set; }
		public bool Fullscreen { get; set; }

		internal GraphicsSettings ()
		{
			//Sane defaults
			RequestedWidth = 800;
			RequestedHeight = 600;
			Fullscreen = false;
		}
	}
}

