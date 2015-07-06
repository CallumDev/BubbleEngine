#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;

namespace BubbleEngine
{
	public class Window
	{
		string title = "Game Window";
		internal IntPtr Handle = IntPtr.Zero;
		public string Title {
			get {
				return title;
			} set {
				title = value;
				if (Handle != IntPtr.Zero)
					SDL2.SDL_SetWindowTitle (Handle, title);
			}
		}
		internal Window ()
		{
		}
	}
}

