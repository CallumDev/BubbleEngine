using System;

namespace BubbleEngine
{
	public class MouseEventArgs
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public MouseButtons Buttons { get; private set; }
		public MouseEventArgs (int x, int y, MouseButtons buttons)
		{
			X = x;
			Y = y;
			Buttons = buttons;
		}
	}
}

