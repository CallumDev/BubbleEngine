using System;

namespace BubbleEngine.LuaAPI
{
	public delegate void LKeyEventHandler(LKeyEventArgs e);
	public class LKeyboard
	{
		Keyboard kbd;
		public LKeyboard (Keyboard keyboard)
		{
			kbd = keyboard;
			kbd.KeyDown += delegate(KeyEventArgs e) {
				if(keyDown != null)
					keyDown(new LKeyEventArgs(e));
			};
			kbd.KeyUp += delegate(KeyEventArgs e) {
				if(keyUp != null)
					keyUp(new LKeyEventArgs(e));
			};
		}

		public event LKeyEventHandler keyDown;
		public event LKeyEventHandler keyUp;

		public bool isKeyDown(int key) {
			return kbd.IsKeyDown((Keys)key);
		}
		public bool isKeyUp(int key) {
			return kbd.IsKeyUp ((Keys)key);
		}
	}
	public class LKeyEventArgs
	{
		public int key;
		public LKeyEventArgs(KeyEventArgs e)
		{
			key = (int)e.Key;
		}
	}
}

