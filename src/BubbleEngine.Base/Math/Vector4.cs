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
	public struct Vector4
	{
		//static
		public static readonly Vector4 UnitX = new Vector4 (1, 0, 0, 0);
		public static readonly Vector4 UnitY = new Vector4 (0, 1, 0, 0);
		public static readonly Vector4 UnitZ = new Vector4 (0, 0, 1, 0);
		public static readonly Vector4 UnitW = new Vector4 (0, 0, 0, 1);
		//instance
		public float X;
		public float Y;
		public float Z;
		public float W;

		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
	}
}

