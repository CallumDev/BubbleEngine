#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;

namespace BubbleEngine
{
	//Drawing primitives with SpriteBatch
	public partial class SpriteBatch
	{
		public void DrawLine (Vector2 start, Vector2 end, Color4 color) {
			int distance = (int)Vector2.Distance(start, end);
			float alpha = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
			Draw (dot, null, new Rectangle ((int)start.X, (int)start.Y, distance, 1), color, new Vector2 (0, 0), alpha);
		}
		public void DrawRectangle (Rectangle rect, Color4 color, int width)
		{
			FillRectangle (new Rectangle (rect.X, rect.Y, width, rect.Height), color);
			FillRectangle (new Rectangle (rect.X, rect.Y, rect.Width, width), color);
			FillRectangle (new Rectangle (rect.X + rect.Width - width, rect.Y, width, rect.Height), color);
			FillRectangle (new Rectangle (rect.X, rect.Y + rect.Height - width, rect.Width, width), color);
		}
		public void FillRectangle(Rectangle rect, Color4 color)
		{
			Draw (dot, null, rect, color);
		}
	}
}

