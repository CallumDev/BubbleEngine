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
	//Used in creating Textures
	[StructLayout(LayoutKind.Sequential)]
	public struct ByteColor
	{
		//colors
		public static readonly ByteColor White = new ByteColor(255,255,255,255);
		public static readonly ByteColor Black = new ByteColor(0,0,0,255);
		//instance
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public ByteColor(byte r, byte g, byte b, byte a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}
	}
}

