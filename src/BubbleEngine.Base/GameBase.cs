 #region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BubbleEngine
{
	public class GameBase
	{
		protected GraphicsSettings GraphicsSettings { get; private set; }
		protected FontContext FontContext { get; private set; }
		protected Window Window { get; private set; }
		protected Mouse Mouse { get; private set; }
		protected Keyboard Keyboard { get; private set; }

		bool running = false;
		public GameBase ()
		{
			GraphicsSettings = new GraphicsSettings ();
			FontContext = new FontContext ();
			Window = new Window ();
		}

		public void Run()
		{
			Threading.RegisterUIThread ();
			Init ();
			Load ();
			//Game loop
			running = true;
			#if DEBUGMAC
			ulong last = SDL2.SDL_GetPerformanceCounter();
			ulong startTick = last;
			#else
			var timer = new Stopwatch();
			timer.Start ();
			double last = 0.0;
			#endif
			double elapsed = 0.0;
			/*
			 * 	DEBUGMAC Define:
			 * 	Use SDL2_GetTicks (less accurate) instead of Stopwatch
			 *  Xamarin Studio Debugger crashes on Mac with Stopwatch
			 *  DEBUGMAC not required for other platforms
			 */
			while (running) {
				//SDL2 events
				ProcessEvents ();
				//Run code for UI thread
				Threading.Update();
				//Run game code
				#if DEBUGMAC
				ulong currentTicks = SDL2.SDL_GetPerformanceCounter();
				var current = (double)(currentTicks - startTick) / (double)SDL2.SDL_GetPerformanceFrequency();
				var t = new GameTime(TimeSpan.FromSeconds(elapsed), TimeSpan.FromSeconds(current));
				#else
				var t = new GameTime(TimeSpan.FromSeconds(elapsed), timer.Elapsed);
				#endif
				if (running) {
					Update(t);
				}
				if (running) {
					GL.glClearColor (0f, 0f, 0f, 1f);
					GL.glClear (GL.GL_COLOR_BUFFER_BIT);
					Draw(t);
				}
				//Time
				#if DEBUGMAC
				elapsed = (double)(currentTicks - last) / (double)SDL2.SDL_GetPerformanceFrequency();
				last = currentTicks;
				#else
				elapsed = timer.Elapsed.TotalSeconds - last;
				last = timer.Elapsed.TotalSeconds;
				#endif
				if (elapsed < 0) {
					elapsed = 0;
					//apparently this can happen?
					Console.WriteLine ("Stopwatch returned negative time");
				}
				SDL2.SDL_GL_SwapWindow (Window.Handle);
				System.Threading.Thread.Sleep (0);
			}
			#if !DEBUGMAC
			timer.Stop ();
			#endif
			SDL2.SDL_Quit ();
		}
		bool fullscreen;
		public void ApplyGraphicsMode()
		{
			if (Window.Handle == IntPtr.Zero) //Not running
				return;
			
			var w = GraphicsSettings.RequestedWidth;
			var h = GraphicsSettings.RequestedHeight;

			SDL2.SDL_SetWindowSize (Window.Handle, w, h);
			if (fullscreen != GraphicsSettings.Fullscreen) {
				SDL2.SDL_SetWindowFullscreen (Window.Handle, GraphicsSettings.Fullscreen ? SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP : 0);
				fullscreen = GraphicsSettings.Fullscreen;
			}
			Window.Width = w;
			Window.Height = h;
		}
		protected virtual void Load()
		{

		}
		protected virtual void Update(GameTime gameTime)
		{

		}
		protected virtual void Draw(GameTime gameTime)
		{

		}
		//Convert from SDL2 button to Bubble button
		MouseButtons GetMouseButton(byte b)
		{
			if (b == SDL2.SDL_BUTTON_LEFT)
				return MouseButtons.Left;
			if (b == SDL2.SDL_BUTTON_MIDDLE)
				return MouseButtons.Middle;
			if (b == SDL2.SDL_BUTTON_RIGHT)
				return MouseButtons.Right;
			if (b == SDL2.SDL_BUTTON_X1)
				return MouseButtons.X1;
			if (b == SDL2.SDL_BUTTON_X2)
				return MouseButtons.X2;
			throw new Exception ("SDL2 gave undefined mouse button"); //should never happen
		}

		unsafe string GetEventText(ref SDL2.SDL_Event e)
		{
			byte[] rawBytes = new byte[SDL2.SDL_TEXTINPUTEVENT_TEXT_SIZE];
			fixed (byte* txtPtr = e.text.text) {
				Marshal.Copy ((IntPtr)txtPtr, rawBytes, 0, SDL2.SDL_TEXTINPUTEVENT_TEXT_SIZE);
			}
			int nullIndex = Array.IndexOf (rawBytes, (byte)0);
			string text = Encoding.UTF8.GetString (rawBytes, 0, nullIndex);
			return text;
		}

		void ProcessEvents()
		{
			SDL2.SDL_Event e;
			while (SDL2.SDL_PollEvent (out e) != 0) {
				if (e.type == SDL2.SDL_EventType.SDL_QUIT) {
					//TODO: Allow cancel of quit event
					running = false;
				}
				//mouse events
				if (e.type == SDL2.SDL_EventType.SDL_MOUSEMOTION) {
					Mouse.X = e.motion.x;
					Mouse.Y = e.motion.y;
					Mouse.OnMouseMove ();
				}
				if (e.type == SDL2.SDL_EventType.SDL_MOUSEBUTTONDOWN) {
					Mouse.X = e.button.x;
					Mouse.Y = e.button.y;
					var btn = GetMouseButton (e.button.button);
					Mouse.Buttons |= btn;
					Mouse.OnMouseDown (btn);
				}
				if (e.type == SDL2.SDL_EventType.SDL_MOUSEBUTTONUP) {
					Mouse.X = e.button.x;
					Mouse.Y = e.button.y;
					var btn = GetMouseButton (e.button.button);
					Mouse.Buttons &= ~btn;
					Mouse.OnMouseUp (btn);
				}
				if (e.type == SDL2.SDL_EventType.SDL_MOUSEWHEEL) {
					Mouse.OnMouseWheel (e.wheel.y);
				}
				//keyboard events
				if (e.type == SDL2.SDL_EventType.SDL_TEXTINPUT) {
					Keyboard.OnTextInput (GetEventText (ref e));
				}
				if (e.type == SDL2.SDL_EventType.SDL_KEYDOWN) {
					Keyboard.OnKeyDown ((Keys)e.key.keysym.sym, (KeyModifiers)e.key.keysym.mod);
				}
				if (e.type == SDL2.SDL_EventType.SDL_KEYUP) {
					Keyboard.OnKeyUp ((Keys)e.key.keysym.sym, (KeyModifiers)e.key.keysym.mod);
				}
			}
		}
		void Init()
		{
			//open SDL2
			SDL2.Load ();
			//initialise SDL2
			if (SDL2.SDL_Init (SDL2.SDL_INIT_VIDEO) != 0) {
				SDL2.SDL_ShowSimpleMessageBox (
					SDL2.SDL_MESSAGEBOX_ERROR,
					"Error",
					"SDL_Init failed, exiting.",
					IntPtr.Zero
				);
				return;
			}
			//context attributes
			SDL2.SDL_GL_SetAttribute (SDL2.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
			SDL2.SDL_GL_SetAttribute (SDL2.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 2);
			SDL2.SDL_GL_SetAttribute (SDL2.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, 
				(int)SDL2.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
			//create window
			fullscreen = GraphicsSettings.Fullscreen;
			var flags = GraphicsSettings.Fullscreen ?
				SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP | SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL :
				SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL;
			var sdlWin = SDL2.SDL_CreateWindow (
				Window.Title,
				SDL2.SDL_WINDOWPOS_CENTERED,
				SDL2.SDL_WINDOWPOS_CENTERED,
				GraphicsSettings.RequestedWidth,
				GraphicsSettings.RequestedHeight,
				flags
			);
			Window.Width = GraphicsSettings.RequestedWidth;
			Window.Height = GraphicsSettings.RequestedHeight;
			Mouse = new Mouse ();
			Keyboard = new Keyboard ();
			if (sdlWin == IntPtr.Zero) {
				SDL2.SDL_ShowSimpleMessageBox (
					SDL2.SDL_MESSAGEBOX_ERROR,
					"Error",
					"SDL_CreateWindow failed, exiting.",
					IntPtr.Zero
				);
				return;
			}
			Window.Handle = sdlWin;
			//create gl context
			var glcontext = SDL2.SDL_GL_CreateContext(sdlWin);
			if (glcontext == IntPtr.Zero) {
				SDL2.SDL_ShowSimpleMessageBox (
					SDL2.SDL_MESSAGEBOX_ERROR,
					"Error",
					"Failed to get GL context, exiting.",
					IntPtr.Zero
				);
				SDL2.SDL_Quit ();
				return;
			}
			//Load libraries
			GL.Load();
			FT.Load ();
			//Base gl state
			GL.glEnable (GL.GL_BLEND);
			GL.glBlendFunc (GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
		}
	}
}

