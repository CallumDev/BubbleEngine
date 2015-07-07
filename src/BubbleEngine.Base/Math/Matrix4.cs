#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 * Original code adapted from OpenTK, also MIT Licensed.
 */
#endregion
using System;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix4
	{
		public static readonly Matrix4 Identity = new Matrix4 (Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

		public Vector4 Row0;
		public Vector4 Row1;
		public Vector4 Row2;
		public Vector4 Row3;

		public Matrix4 (Vector4 r0, Vector4 r1, Vector4 r2, Vector4 r3)
		{
			Row0 = r0;
			Row1 = r1;
			Row2 = r2;
			Row3 = r3;
		}

		public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
		{
			var result = Identity;
			float invRL = 1.0f / (right - left);
			float invTB = 1.0f / (top - bottom);
			float invFN = 1.0f / (zFar - zNear);

			result.Row0.X = 2 * invRL;
			result.Row1.Y = 2 * invTB;
			result.Row2.Z = -2 * invFN;

			result.Row3.X = -(right + left) * invRL;
			result.Row3.Y = -(top + bottom) * invTB;
			result.Row3.Z = -(zFar + zNear) * invFN;
			return result;
		}
	}
}

