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
	public struct Vector2
	{
		//static
		public static readonly Vector2 Zero = new Vector2(0,0);
		//instance
		public float X;
		public float Y;

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
		//functions
		public static float Distance(Vector2 a, Vector2 b)
		{
			return (float)Math.Sqrt ((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
		}
		//operators
		public static bool operator ==(Vector2 a, Vector2 b)
		{
			return (a.X == b.X) && (a.Y == b.Y);
		}

		public static bool operator !=(Vector2 a, Vector2 b)
		{
			return (a.X != b.X) || (a.Y != b.Y);
		}

		public override bool Equals (object obj)
		{
			if (!(obj is Vector2))
				return false;
			var other = (Vector2)obj;
			return (this == other);
		}

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2 (a.X + b.X, a.Y + b.Y);
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2 (a.X - b.X, a.Y - b.Y);
		}

		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			return new Vector2 (a.X * b.X, a.Y * b.Y);
		}

		public static Vector2 operator /(Vector2 a, Vector2 b)
		{
			return new Vector2 (a.X / b.X, a.Y / b.Y);
		}
	}
}

