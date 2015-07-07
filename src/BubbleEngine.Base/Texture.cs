#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;

namespace BubbleEngine
{
	public class Texture
	{
		internal uint Id { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public Texture (int width, int height)
		{
			Width = width;
			Height = height;
			uint _id;
			GL.glGenTextures (1, out _id);
			Id = _id;
			GL.glBindTexture (GL.GL_TEXTURE_2D, Id);
			GL.glTexParameteri (GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
			GL.glTexParameteri (GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
			GL.glTexImage2D (GL.GL_TEXTURE_2D, 0, GL.GL_RGBA, width, height, 0, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, IntPtr.Zero);
		}

		public void SetData(IntPtr data, Rectangle? rect)
		{
			Rectangle r;
			if (rect.HasValue) {
				r = rect.Value;
			} else {
				r = new Rectangle (0, 0, Width, Height);
			}
			GL.glBindTexture (GL.GL_TEXTURE_2D, Id);
			GL.glTexSubImage2D (
				GL.GL_TEXTURE_2D,
				0,
				r.X,
				r.Y,
				r.Width,
				r.Height,
				GL.GL_RGBA,
				GL.GL_UNSIGNED_BYTE,
				data
			);
		}
		public unsafe void SetData(ByteColor[] data, Rectangle? rect)
		{
			fixed(ByteColor *ptr = data) {
				SetData ((IntPtr)ptr, rect);
			}
		}
	}
}

