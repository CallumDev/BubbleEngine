#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;
using System.Diagnostics;
namespace BubbleEngine
{
	public class GameBase
	{
		protected GraphicsSettings GraphicsSettings { get; private set; }
		protected FontContext FontContext { get; private set; }
		protected Window Window { get; private set; }
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
			var timer = new Stopwatch();
			timer.Start ();
			double last = 0.0;
			double elapsed = 0.0;
			while (running) {
				//SDL2 events
				ProcessEvents ();
				//Run code for UI thread
				Threading.Update();
				//Run game code
				var t = new GameTime(TimeSpan.FromSeconds(elapsed), timer.Elapsed);

				if (running) {
					//TODO: Update
					Update(t);
				}
				if (running) {
					GL.glClearColor (0f, 0f, 0f, 1f);
					GL.glClear (GL.GL_COLOR_BUFFER_BIT);
					//TODO: Draw
					Draw(t);
				}
				//Time
				elapsed = timer.Elapsed.TotalSeconds - last;
				last = timer.Elapsed.TotalSeconds;
				if (elapsed < 0) {
					elapsed = 0;
					//apparently this can happen?
					Console.WriteLine ("Stopwatch returned negative time");
				}
				SDL2.SDL_GL_SwapWindow (Window.Handle);
				System.Threading.Thread.Sleep (0);
			}
			timer.Stop ();
			SDL2.SDL_Quit ();
		}
		bool fullscreen;
		public void ApplyGraphicsMode()
		{
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
		void ProcessEvents()
		{
			SDL2.SDL_Event e;
			while (SDL2.SDL_PollEvent (out e) != 0) {
				if (e.type == SDL2.SDL_EventType.SDL_QUIT) {
					//TODO: Allow cancel of quit event
					running = false;
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

