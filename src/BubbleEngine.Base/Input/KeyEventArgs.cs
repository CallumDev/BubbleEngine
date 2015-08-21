#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;

namespace BubbleEngine
{
	public class KeyEventArgs
	{
		public Keys Key { get; private set; }
		public KeyModifiers Modifiers { get; private set; }

		public KeyEventArgs (Keys key, KeyModifiers mod)
		{
			Key = key;
			Modifiers = mod;
		}
	}
}

