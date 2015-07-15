using System;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	static class FT
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int Init_FreeType (out IntPtr alibrary);
		public static Init_FreeType FT_Init_FreeType;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int New_Face (IntPtr library, string filepathname, int face_index, out IntPtr aface);
		public static New_Face FT_New_Face;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int Set_Char_Size (IntPtr face, IntPtr char_width, IntPtr char_height, uint horz_resolution, uint vert_resolution);
		public static Set_Char_Size FT_Set_Char_Size;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate uint Get_Char_Index(IntPtr face, uint charcode);
		public static Get_Char_Index FT_Get_Char_Index;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int Load_Glyph (IntPtr face, uint glyph_index, int load_flags);
		public static Load_Glyph FT_Load_Glyph;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int Render_Glyph(IntPtr slot, int render_mode);
		public static Render_Glyph FT_Render_Glyph;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int FT_Get_Kerning(IntPtr face, uint left_glyph, uint right_glyph, uint kern_mode, out FT_Vector akerning);

		[StructLayout(LayoutKind.Sequential)]
		public struct FT_Vector
		{
			//signed long FT_Pos -> IntPtr
			public IntPtr x; 
			public IntPtr y;
		}

		public static void Load()
		{
			var loader = Platform.GetDllLoader ();
			IntPtr library = IntPtr.Zero;
			switch (Platform.CurrentPlatform) {
			case Platforms.Linux:
				library = loader.LoadLibrary ("libfreetype.so.6");
				break;
			case Platforms.Windows:
				if (Environment.Is64BitProcess) {
					library = loader.LoadLibrary ("freetype6.x64.dll");
				} else {
					library = loader.LoadLibrary ("freetype6.x86.dll");
				}
				break;
			case Platforms.OSX:
				if (Environment.Is64BitProcess) {
					library = loader.LoadLibrary ("libfreetype.6.x64.dylib");
				} else {
					library = loader.LoadLibrary ("libfreetype.6.x86.dylib");
				}
				break;
			}
			InteropHelper.LoadFunctions (typeof(FT), (x) => loader.GetProcAddress (library, x));
		}
	}
}

