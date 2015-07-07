#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;

namespace BubbleEngine
{
	public struct Rectangle
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

		public Rectangle(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}
}

