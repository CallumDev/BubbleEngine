#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;

namespace BubbleEngine
{
	public delegate void MouseWheelEventHandler(int amount);
	public delegate void MouseEventHandler(MouseEventArgs e);
	public class Mouse
	{
		public int X { get; internal set; }
		public int Y { get; internal set; }
		public MouseButtons Buttons { get; internal set; }

		public event MouseWheelEventHandler MouseWheel;
		public event MouseEventHandler MouseMove;
		public event MouseEventHandler MouseDown;
		public event MouseEventHandler MouseUp;

		internal Mouse ()
		{
		}

		internal void OnMouseMove()
		{
			if (MouseMove != null)
				MouseMove (new MouseEventArgs (X, Y, Buttons));
		}

		internal void OnMouseDown(MouseButtons b)
		{
			if (MouseDown != null)
				MouseDown (new MouseEventArgs (X, Y, b));
		}

		internal void OnMouseUp (MouseButtons b)
		{
			if (MouseUp != null)
				MouseUp (new MouseEventArgs (X, Y, b));
		}

		internal void OnMouseWheel(int amount)
		{
			if (MouseWheel != null)
				MouseWheel (amount);
		}
	}
}

