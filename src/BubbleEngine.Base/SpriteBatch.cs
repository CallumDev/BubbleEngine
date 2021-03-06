﻿#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	public partial class SpriteBatch
	{
		//Utility
		[StructLayout(LayoutKind.Sequential)]
		struct Vertex2D
		{
			public Vector2 Position;
			public Vector2 TextureCoordinate;
			public Color4 Color;
			public Vertex2D(Vector2 pos, Vector2 texcoord, Color4 color)
			{
				Position = pos;
				TextureCoordinate = texcoord;
				Color = color;
			}
			public static readonly int Size = Marshal.SizeOf<Vertex2D>();
		}
		//Swap two vertices
		static void SwapVec(ref Vector2 a, ref Vector2 b)
		{
			var temp = a;
			a = b;
			b = temp;
		}

		//constants
		const int MAX_BATCH = 1365;
		const int MAX_VERTICES = 1365 * 4;
		const int MAX_INDICES = 1365 * 6;

		//resources
		Window window;
		uint vboID;
		Vertex2D[] vertices;
		uint iboID;
		uint vaoID;
		Texture dot; //used for drawing primitives
		Shader shader;
		//state
		bool active = false;
		int currentTexture = -1;
		int vertexCount = 0;
		int indexCount = 0;

		public unsafe SpriteBatch (Window win)
		{
			window = win;
			//Create shader and set texture unit to 0
			shader = new Shader (vertex_source, fragment_source);
			shader.SetInteger ("tex", 0);
			//create vbo
			GL.glGenBuffers (1, out vboID);
			GL.glBindBuffer (GL.GL_ARRAY_BUFFER, vboID);
			int vert2d_size = Marshal.SizeOf (typeof(Vertex2D));
			int vboSize = MAX_VERTICES * vert2d_size;
			GL.glBufferData (GL.GL_ARRAY_BUFFER, (IntPtr)vboSize, IntPtr.Zero, GL.GL_DYNAMIC_DRAW);
			//create vao and ibo
			GL.glGenVertexArrays(1, out vaoID);
			GL.glBindVertexArray (vaoID);
			GL.glGenBuffers (1, out iboID);
			GL.glBindBuffer (GL.GL_ELEMENT_ARRAY_BUFFER, iboID);
			int iboSize = MAX_INDICES * 2;
			//generate indices, they're all gonna be the same
			int iptr = 0;
			var indices = new ushort[MAX_INDICES];
			for (int i = 0; i < MAX_VERTICES; i += 4) {
				/* Triangle 1 */
				indices[iptr++] = (ushort)i;
				indices[iptr++] = (ushort)(i + 1);
				indices[iptr++] = (ushort)(i + 2);
				/* Triangle 2 */
				indices[iptr++] = (ushort)(i + 1);
				indices[iptr++] = (ushort)(i + 3);
				indices[iptr++] = (ushort)(i + 2);
			}
			fixed(ushort *ptr = indices) {
				GL.glBufferData (GL.GL_ELEMENT_ARRAY_BUFFER, (IntPtr)iboSize, (IntPtr)ptr, GL.GL_DYNAMIC_DRAW);
			}
			GL.glEnableVertexAttribArray (0);
			GL.glEnableVertexAttribArray (1);
			GL.glEnableVertexAttribArray (2);
			GL.glVertexAttribPointer (0, 2, GL.GL_FLOAT, false, vert2d_size, IntPtr.Zero);
			GL.glVertexAttribPointer (1, 2, GL.GL_FLOAT, false, vert2d_size, (IntPtr)(2 * sizeof(float)));
			GL.glVertexAttribPointer (2, 4, GL.GL_FLOAT, false, vert2d_size, (IntPtr)(4 * sizeof(float)));
			GL.glBindVertexArray (0);
			//create texture
			dot = new Texture(1,1);
			dot.SetData (new ByteColor[] { ByteColor.White }, null);
			//create arrays
			vertices = new Vertex2D[MAX_VERTICES];

		}

		public void Begin()
		{
			if (active) {
				throw new Exception ("Can't begin a spritebatch that's already active");
			}
			vertexCount = 0;
			indexCount = 0;
			currentTexture = -1;
			active = true;
		}

		public void End()
		{
			if (!active) {
				throw new Exception ("Can't end a spritebatch that hasn't been started");
			}
			Flush ();
			active = false;
		}
		public void Draw(Texture texture, Rectangle? sourceRect, Vector2 dest, Color4 color, Vector2 origin, float rotation)
		{
			Draw(texture, sourceRect, new Rectangle ((int)dest.X, (int)dest.Y, texture.Width, texture.Height), color, origin, rotation);
		}
		public void Draw(Texture texture, Rectangle? sourceRect, Rectangle destRect, Color4 color, Vector2 origin, float rotation)
		{
			DrawInternal (
				texture,
				sourceRect,
				destRect,
				color,
				-(origin.X),
				-(origin.Y),
				(float)Math.Sin (rotation),
				(float)Math.Cos (rotation));
		}
		public void Draw(Texture tex, Vector2 position, Color4 color)
		{
			Draw (tex, null, position, color);
		}
		public void Draw(Texture tex, Rectangle? sourceRect, Vector2 position, Color4 color)
		{
			var r = new Rectangle ((int)position.X, (int)position.Y, tex.Width, tex.Height);
			if (sourceRect != null) {
				r.Width = sourceRect.Value.Width;
				r.Height = sourceRect.Value.Height;
			}
			Draw (tex, sourceRect, r, color);
		}
		public void Draw(Texture tex, Rectangle? sourceRect, Rectangle destRect, Color4 color)
		{
			Draw (tex, sourceRect, destRect, color, SpriteEffects.None);
		}
		public void Draw(Texture tex, Rectangle? sourceRect, Rectangle destRect, Color4 color, SpriteEffects effects)
		{
			if (currentTexture != -1 && tex.Id != currentTexture) {
				Flush ();
			}
			currentTexture = (int)tex.Id;
			if (indexCount + 6 >= MAX_INDICES || vertexCount + 4 >= MAX_VERTICES)
				Flush ();
			Rectangle source;
			if (sourceRect.HasValue) {
				source = sourceRect.Value;
			} else {
				source.X = 0;
				source.Y = 0;
				source.Width = tex.Width;
				source.Height = tex.Height;
			}
			float x = (float)destRect.X;
			float y = (float)destRect.Y;
			float w = (float)destRect.Width;
			float h = (float)destRect.Height;
			float srcX = (float)source.X;
			float srcY = (float)source.Y;
			float srcW = (float)source.Width;
			float srcH = (float)source.Height;

			Vector2 topLeftCoord = new Vector2 (srcX / (float)tex.Width,
				srcY / (float)tex.Height);
			Vector2 topRightCoord = new Vector2 ((srcX + srcW) / (float)tex.Width,
				srcY / (float)tex.Height);
			Vector2 bottomLeftCoord = new Vector2 (srcX / (float)tex.Width,
				(srcY + srcH) / (float)tex.Height);
			Vector2 bottomRightCoord = new Vector2 ((srcX + srcW) / (float)tex.Width,
				(srcY + srcH) / (float)tex.Height);
			if ((effects & SpriteEffects.FlipHorizontal) == SpriteEffects.FlipHorizontal) {
				SwapVec (ref topLeftCoord, ref topRightCoord);
				SwapVec (ref bottomLeftCoord, ref bottomRightCoord);
			}
			if ((effects & SpriteEffects.FlipVertical) == SpriteEffects.FlipVertical) {
				SwapVec (ref topLeftCoord, ref bottomLeftCoord);
				SwapVec (ref topRightCoord, ref bottomRightCoord);
			}
			/* Top Left */
			vertices[vertexCount++] = new Vertex2D(
				new Vector2(x, y),
				topLeftCoord,
				color
			);
			/* Top Right */
			vertices[vertexCount++] = new Vertex2D(
				new Vector2(x + w,y),
				topRightCoord,
				color
			);
			/* Bottom Left */
			vertices[vertexCount++] = new Vertex2D(
				new Vector2(x, y + h),
				bottomLeftCoord,
				color
			);
			/* Bottom Right */
			vertices[vertexCount++] = new Vertex2D(
				new Vector2(x + w, y + h),
				bottomRightCoord,
				color
			);

			indexCount += 6;
		}

		void DrawInternal(Texture texture, Rectangle? sourceRect, Rectangle destRect, Color4 color, float dx, float dy, float sin, float cos)
		{
			if (currentTexture != -1 && texture.Id != currentTexture) {
				Flush ();
			}
			currentTexture = (int)texture.Id;
			if (indexCount + 6 >= MAX_INDICES || vertexCount + 4 >= MAX_VERTICES)
				Flush ();
			Rectangle source;
			if (sourceRect != null)
				source = sourceRect.Value;
			else
				source = new Rectangle (0, 0, texture.Width, texture.Height);
			float x = (float)destRect.X;
			float y = (float)destRect.Y;
			float w = (float)destRect.Width;
			float h = (float)destRect.Height;
			//Top Left
			vertices[vertexCount++] = 
				new Vertex2D (
					new Vector2 (x + dx * cos - dy * sin, 
						y + dx * sin + dy * cos),
					new Vector2 (source.X / (float)texture.Width,
						source.Y / (float)texture.Height),
					color);
			//Top Right
			vertices[vertexCount++] = 
				new Vertex2D (
					new Vector2 (x + (dx + w) * cos - dy * sin, 
						y + (dx + w) * sin + dy * cos),
					new Vector2 ((source.X + source.Width) / (float)texture.Width,
						source.Y / (float)texture.Height),
					color);
			//Bottom Left
			vertices[vertexCount++] =
				new Vertex2D (
					new Vector2 (x + dx * cos - (dy + h) * sin, 
						y + dx * sin + (dy + h) * cos),
					new Vector2 (source.X / (float)texture.Width,
						(source.Y + source.Height) / (float)texture.Height),
					color);
			//Bottom Right
			vertices[vertexCount++] = 
				new Vertex2D (
					new Vector2 (x + (dx + w) * cos - (dy + h) * sin, 
						y + (dx + w) * sin + (dy + h) * cos),
					new Vector2 ((source.X + source.Width) / (float)texture.Width,
						(source.Y + source.Height) / (float)texture.Height),
					color);
			
			indexCount += 6;
		}

		unsafe void Flush()
		{
			if (indexCount == 0)
				return;
			shader.UseProgram ();
			var mat = Matrix4.CreateOrthographicOffCenter (0, window.Width, window.Height, 0, 0, 1);
			shader.SetMatrix ("modelviewproj", ref mat);
			GL.glViewport (0, 0, window.Width, window.Height);
			GL.glBindVertexArray (vaoID);
			GL.glBindTexture (GL.GL_TEXTURE_2D, (uint)currentTexture);
			GL.glBindBuffer (GL.GL_ARRAY_BUFFER, vboID);
			GL.glBindBuffer (GL.GL_ELEMENT_ARRAY_BUFFER, iboID);
			fixed(Vertex2D *ptr = vertices) {
				GL.glBufferSubData (GL.GL_ARRAY_BUFFER, IntPtr.Zero, (IntPtr)(vertexCount * Vertex2D.Size), (IntPtr)ptr);
			}
			GL.glDrawElements (GL.GL_TRIANGLES, indexCount, GL.GL_UNSIGNED_SHORT, IntPtr.Zero);
			GL.glBindVertexArray (0);
			vertexCount = 0;
			indexCount = 0;
		}

	}
}

