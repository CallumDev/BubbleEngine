﻿#region License
/* Original Code: SDL2#
 * Adapted for Bubble Engine
 * 
 * SDL2# - C# Wrapper for SDL2
 *
 * Copyright (c) 2013-2015 Ethan Lee.
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */
#endregion
using System;
using System.Reflection;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	static partial class SDL2
	{
		public static bool Loaded = false;

		//The fields do get assigned to, just through reflection.
		#pragma warning disable 0649

		#region Init
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int Init(uint flags);
		public static Init SDL_Init;
		public const uint SDL_INIT_TIMER 		=	0x00000001;
		public const uint SDL_INIT_AUDIO 		=	0x00000010;
		public const uint SDL_INIT_VIDEO 		=	0x00000020;
		public const uint SDL_INIT_CDROM 		=	0x00000100;
		public const uint SDL_INIT_JOYSTICK 	=	0x00000200;
		public const uint SDL_INIT_NOPARACHUTE 	=	0x00100000;	/**< Don't catch fatal signals */
		public const uint SDL_INIT_EVENTTHREAD 	=	0x01000000; /**< Not supported on all OS's */
		public const uint SDL_INIT_EVERYTHING 	=	0x0000FFFF;
		#endregion

		#region Quit
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void Quit();
		public static Quit SDL_Quit;
		#endregion

		#region Message Box
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int ShowSimpleMessageBox(
			uint flags,
			string title,
			string message, 
			IntPtr window
		);
		public static ShowSimpleMessageBox SDL_ShowSimpleMessageBox;

		public const uint SDL_MESSAGEBOX_ERROR =  0x00000010;
		#endregion

		#region Window Creation
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr CreateWindow(
			string title,
			int x,
			int y,
			int w,
			int h,
			SDL_WindowFlags flags
		);
		public static CreateWindow SDL_CreateWindow;

		[Flags]
		public enum SDL_WindowFlags
		{
			SDL_WINDOW_FULLSCREEN =		0x00000001,
			SDL_WINDOW_OPENGL =		0x00000002,
			SDL_WINDOW_SHOWN =		0x00000004,
			SDL_WINDOW_HIDDEN =		0x00000008,
			SDL_WINDOW_BORDERLESS =		0x00000010,
			SDL_WINDOW_RESIZABLE =		0x00000020,
			SDL_WINDOW_MINIMIZED =		0x00000040,
			SDL_WINDOW_MAXIMIZED =		0x00000080,
			SDL_WINDOW_INPUT_GRABBED =	0x00000100,
			SDL_WINDOW_INPUT_FOCUS =	0x00000200,
			SDL_WINDOW_MOUSE_FOCUS =	0x00000400,
			SDL_WINDOW_FULLSCREEN_DESKTOP =
				(SDL_WINDOW_FULLSCREEN | 0x00001000),
			SDL_WINDOW_FOREIGN =		0x00000800,
			SDL_WINDOW_ALLOW_HIGHDPI =	0x00002000	/* Only available in 2.0.1 */
		}

		public const int SDL_WINDOWPOS_UNDEFINED_MASK =	0x1FFF0000;
		public const int SDL_WINDOWPOS_CENTERED_MASK =	0x2FFF0000;
		public const int SDL_WINDOWPOS_UNDEFINED =		0x1FFF0000;
		public const int SDL_WINDOWPOS_CENTERED =		0x2FFF0000;

		public static int SDL_WINDOWPOS_UNDEFINED_DISPLAY(int X)
		{
			return (SDL_WINDOWPOS_UNDEFINED_MASK | X);
		}

		public static bool SDL_WINDOWPOS_ISUNDEFINED(int X)
		{
			return (X & 0xFFFF0000) == SDL_WINDOWPOS_UNDEFINED_MASK;
		}

		public static int SDL_WINDOWPOS_CENTERED_DISPLAY(int X)
		{
			return (SDL_WINDOWPOS_CENTERED_MASK | X);
		}

		public static bool SDL_WINDOWPOS_ISCENTERED(int X)
		{
			return (X & 0xFFFF0000) == SDL_WINDOWPOS_CENTERED_MASK;
		}
		#endregion

		#region Window Size
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GetWindowSize(IntPtr window, out int w, out int h);
		public static GetWindowSize SDL_GetWindowSize;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void SetWindowSize(IntPtr window, int w, int h);
		public static SetWindowSize SDL_SetWindowSize;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int SetWindowFullscreen(IntPtr window, SDL_WindowFlags flags);
		public static SetWindowFullscreen SDL_SetWindowFullscreen;
		#endregion

		#region Window Title
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void SetWindowTitle(IntPtr window, string title);
		public static SetWindowTitle SDL_SetWindowTitle;
		#endregion

		#region GL Attributes
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int GL_SetAttribute(
			SDL_GLattr attr,
			int value
		);

		public static GL_SetAttribute SDL_GL_SetAttribute;

		public enum SDL_GLattr
		{
			SDL_GL_RED_SIZE,
			SDL_GL_GREEN_SIZE,
			SDL_GL_BLUE_SIZE,
			SDL_GL_ALPHA_SIZE,
			SDL_GL_BUFFER_SIZE,
			SDL_GL_DOUBLEBUFFER,
			SDL_GL_DEPTH_SIZE,
			SDL_GL_STENCIL_SIZE,
			SDL_GL_ACCUM_RED_SIZE,
			SDL_GL_ACCUM_GREEN_SIZE,
			SDL_GL_ACCUM_BLUE_SIZE,
			SDL_GL_ACCUM_ALPHA_SIZE,
			SDL_GL_STEREO,
			SDL_GL_MULTISAMPLEBUFFERS,
			SDL_GL_MULTISAMPLESAMPLES,
			SDL_GL_ACCELERATED_VISUAL,
			SDL_GL_RETAINED_BACKING,
			SDL_GL_CONTEXT_MAJOR_VERSION,
			SDL_GL_CONTEXT_MINOR_VERSION,
			SDL_GL_CONTEXT_EGL,
			SDL_GL_CONTEXT_FLAGS,
			SDL_GL_CONTEXT_PROFILE_MASK,
			SDL_GL_SHARE_WITH_CURRENT_CONTEXT,
			SDL_GL_FRAMEBUFFER_SRGB_CAPABLE
		}

		[Flags]
		public enum SDL_GLprofile
		{
			SDL_GL_CONTEXT_PROFILE_CORE				= 0x0001,
			SDL_GL_CONTEXT_PROFILE_COMPATIBILITY	= 0x0002,
			SDL_GL_CONTEXT_PROFILE_ES				= 0x0004
		}

		#endregion

		#region GL Context
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr GL_CreateContext(IntPtr window);
		public static GL_CreateContext SDL_GL_CreateContext;
		#endregion

		#region GetProcAddress
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr GL_GetProcAddress(
			string proc
		);
		public static GL_GetProcAddress SDL_GL_GetProcAddress; 
		#endregion

		#region Flip Window
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GL_SwapWindow(IntPtr window);
		public static GL_SwapWindow SDL_GL_SwapWindow;
		#endregion

		#region Get Error
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr GetError();
		public static GetError SDL_GetError;
		public static string GetErrorString()
		{
			var ptr = SDL_GetError ();
			return Marshal.PtrToStringAnsi (ptr);
		}
		#endregion

		#region Performance Counter (Mac Debug)
		#if DEBUGMAC
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate ulong GetPerformanceCounter();
		public static GetPerformanceCounter SDL_GetPerformanceCounter;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate ulong GetPerformanceFrequency();
		public static GetPerformanceFrequency SDL_GetPerformanceFrequency;
		#endif
		#endregion
		/// <summary>
		/// Load all delegate functions
		/// </summary>
		public static void Load()
		{
			var loader = Platform.GetDllLoader ();
			string lib;
			//mac libraries
			if (!Environment.Is64BitProcess) {
				lib = "libSDL2.x86.dylib";
			} else {
				lib = "libSDL2.x64.dylib";
			}
			//linux + windows libraries (don't need a specific path)
			if (Platform.CurrentPlatform == Platforms.Linux)
				lib = "libSDL2.so";
			else if (Platform.CurrentPlatform == Platforms.Windows) {
				if (Environment.Is64BitProcess) {
					lib = "SDL2.x64.dll";
				} else {
					lib = "SDL2.x86.dll";
				}
			}
			var sdl2_ptr = loader.LoadLibrary (InteropHelper.ResolvePath(lib));
			InteropHelper.LoadFunctions (typeof(SDL2), (x) => loader.GetProcAddress (sdl2_ptr, x));
			Loaded = true;
		}
	}
}

