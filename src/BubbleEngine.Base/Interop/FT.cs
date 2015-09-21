using System;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	static class FT
	{
		public const int FT_RENDER_MODE_NORMAL = 0;
		public const int FT_LOAD_DEFAULT = 0;
		//define FT_LOAD_TARGET_( x ) ( (FT_Int32)( (x) & 15) << 16 )
		//define FT_LOAD_TARGET_NORMAL = FT_LOAD_TARGET_(FT_RENDER_MODE_NORMAL)
		public const int FT_LOAD_TARGET_NORMAL = (FT_RENDER_MODE_NORMAL & 15) << 16;

		public const long FT_FACE_FLAG_KERNING = ( 1L <<  6 );
		//The fields do get assigned to, just through reflection.
		#pragma warning disable 0649

		public static bool Loaded = false;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int Init_FreeType (out IntPtr alibrary);
		public static Init_FreeType FT_Init_FreeType;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int Done_FreeType (IntPtr alibrary);
		public static Done_FreeType FT_Done_FreeType;

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
		public delegate int Get_Kerning(IntPtr face, uint left_glyph, uint right_glyph, uint kern_mode, out FT_Vector akerning);
		public static Get_Kerning FT_Get_Kerning;

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
			string libPath = "";
			switch (Platform.CurrentPlatform) {
			case Platforms.Linux:
				libPath = "libfreetype.so.6";
				break;
			case Platforms.Windows:
				if (Environment.Is64BitProcess) {
					libPath = "freetype6.x64.dll";
				} else {
					libPath = "freetype6.x86.dll";
				}
				break;
			case Platforms.OSX:
				if (Environment.Is64BitProcess) {
					libPath = "libfreetype.6.x64.dylib";
				} else {
					libPath = "libfreetype.6.x86.dylib";
				}
				break;
			}
			library = loader.LoadLibrary (InteropHelper.ResolvePath(libPath));
			InteropHelper.LoadFunctions (typeof(FT), (x) => loader.GetProcAddress (library, x));
			Loaded = true;
		}

		/// <summary>
		/// Internally represents a Face.
		/// </summary>
		/// <remarks>
		/// Refer to <see cref="Face"/> for FreeType documentation.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		internal class FaceRec
		{
			internal IntPtr num_faces;
			internal IntPtr face_index;

			internal IntPtr face_flags;
			internal IntPtr style_flags;

			internal IntPtr num_glyphs;

			internal IntPtr family_name;
			internal IntPtr style_name;

			internal int num_fixed_sizes;
			internal IntPtr available_sizes;

			internal int num_charmaps;
			internal IntPtr charmaps;

			internal GenericRec generic;

			internal BBox bbox;

			internal ushort units_per_EM;
			internal short ascender;
			internal short descender;
			internal short height;

			internal short max_advance_width;
			internal short max_advance_height;

			internal short underline_position;
			internal short underline_thickness;

			internal IntPtr glyph;
			internal IntPtr size;
			internal IntPtr charmap;

			private IntPtr driver;
			private IntPtr memory;
			private IntPtr stream;

			private IntPtr sizes_list;
			private GenericRec autohint;
			private IntPtr extensions;

			private IntPtr @internal;

			internal static int SizeInBytes { get { return Marshal.SizeOf(typeof(FaceRec)); } }
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct GenericRec
		{
			internal IntPtr data;
			internal IntPtr finalizer;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct BBox
		{
			IntPtr xMin, yMin;
			IntPtr xMax, yMax;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class GlyphSlotRec
		{
			internal IntPtr library;
			internal IntPtr face;
			internal IntPtr next;
			internal uint reserved;
			internal GenericRec generic;

			internal GlyphMetricsRec metrics;
			internal IntPtr linearHoriAdvance;
			internal IntPtr linearVertAdvance;
			internal FT_Vector advance;

			internal uint format;

			internal BitmapRec bitmap;
			internal int bitmap_left;
			internal int bitmap_top;

			internal OutlineRec outline;

			internal uint num_subglyphs;
			internal IntPtr subglyphs;

			internal IntPtr control_data;
			internal IntPtr control_len;

			internal IntPtr lsb_delta;
			internal IntPtr rsb_delta;

			internal IntPtr other;

			private IntPtr @internal;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct GlyphMetricsRec
		{
			internal IntPtr width;
			internal IntPtr height;

			internal IntPtr horiBearingX;
			internal IntPtr horiBearingY;
			internal IntPtr horiAdvance;

			internal IntPtr vertBearingX;
			internal IntPtr vertBearingY;
			internal IntPtr vertAdvance;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct BitmapRec
		{
			internal int rows;
			internal int width;
			internal int pitch;
			internal IntPtr buffer;
			internal short num_grays;
			internal byte pixel_mode;
			internal byte palette_mode;
			internal IntPtr palette;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct OutlineRec
		{
			internal short n_contours;
			internal short n_points;

			internal IntPtr points;
			internal IntPtr tags;
			internal IntPtr contours;

			internal int flags;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class SizeRec
		{
			internal IntPtr face;
			internal GenericRec generic;
			internal SizeMetricsRec metrics;
			private IntPtr @internal;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct SizeMetricsRec
		{
			internal ushort x_ppem;
			internal ushort y_ppem;

			internal IntPtr x_scale;
			internal IntPtr y_scale;
			internal IntPtr ascender;
			internal IntPtr descender;
			internal IntPtr height;
			internal IntPtr max_advance;
		}
	}
}

