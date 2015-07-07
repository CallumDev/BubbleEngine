#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Color4
	{
		//static
		public static readonly Color4 White = new Color4(1,1,1,1);
		public static readonly Color4 Black = new Color4 (0, 0, 0, 1);
		public static readonly Color4 Red = new Color4 (1, 0, 0, 1);
		public static readonly Color4 Green = new Color4(0,1,0,1);
		public static readonly Color4 Blue = new Color4(0,0,1,0);

		//instance
		public float R;
		public float G;
		public float B;
		public float A;

		public Color4(float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}
	}
}

