using System;

namespace BubbleEngine.LuaAPI
{
	public class LKeyboard
	{
		Keyboard kbd;
		public LKeyboard (Keyboard keyboard)
		{
			kbd = keyboard;
		}
		public event KeyEventHandler keyDown {
			add { kbd.KeyDown += value; }
			remove { kbd.KeyDown -= value; }
		}
	}
}

