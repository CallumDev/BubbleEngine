#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	static class GL
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ClearColor(float r, float g, float b, float a);
		public static ClearColor glClearColor;

		public const int GL_COLOR_BUFFER_BIT = 0x00004000;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void Clear(int flags);
		public static Clear glClear;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void Viewport(int x, int y, int width, int height);
		public static Viewport glViewport;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate uint CreateShader(int shaderType);
		public static CreateShader glCreateShader;
		public const int GL_FRAGMENT_SHADER = 0x8B30;
		public const int GL_VERTEX_SHADER = 0x8B31;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GenTextures(int n, out uint textures);
		public static GenTextures glGenTextures;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void TexParameteri(int target, int pname, int param);
		public static TexParameteri glTexParameteri;
		public const int GL_TEXTURE_2D = 0x0DE1;
		public const int GL_TEXTURE_MIN_FILTER = 0x2801;
		public const int GL_TEXTURE_MAG_FILTER = 0x2800;
		public const int GL_LINEAR = 0x2601;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void BindTexture(int target, uint id);
		public static BindTexture glBindTexture;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void TexImage2D(int target, int level, int internalFormat, int width, int height, int border, int format, int type, IntPtr data);
		public static TexImage2D glTexImage2D;
		public const int GL_RGBA = 0x1908;
		public const int GL_UNSIGNED_BYTE = 0x1401;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void TexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, IntPtr data);
		public static TexSubImage2D glTexSubImage2D;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate void gShaderSource(uint shader, int count, ref IntPtr str, IntPtr length);
		static gShaderSource glShaderSource;
		public static unsafe void ShaderSource(uint shader, string s)
		{
			var bytes = new byte[s.Length + 1];
			Encoding.ASCII.GetBytes (s, 0, s.Length, bytes, 0);
			bytes [s.Length] = 0;
			int len = s.Length;
			fixed(byte* ptr = bytes) {
				var intptr = (IntPtr)ptr;
				glShaderSource (shader, 1, ref intptr, IntPtr.Zero);
			}
		}
		public delegate void CompileShader(uint shader);
		public static CompileShader glCompileShader;

		public delegate uint CreateProgram();
		public static CreateProgram glCreateProgram;

		public delegate void AttachShader(uint program, uint shader);
		public static AttachShader glAttachShader;

		public delegate void BindAttribLocation(uint program, uint index, string name);
		public static BindAttribLocation glBindAttribLocation;

		public delegate void LinkProgram(uint program);
		public static LinkProgram glLinkProgram;

		public delegate void UseProgram(uint program);
		public static UseProgram glUseProgram;

		public delegate int GetUniformLocation(uint program, string name);
		public static GetUniformLocation glGetUniformLocation;

		public delegate void Uniform1i (int location, int v0);
		public static Uniform1i glUniform1i;

		public delegate void UniformMatrix4fv (int location, int count, bool transpose, IntPtr value);
		public static UniformMatrix4fv glUniformMatrix4fv;

		public delegate void GenBuffers(int n, out uint buffers);
		public static GenBuffers glGenBuffers;

		public delegate void BindBuffer(int target, uint id);
		public static BindBuffer glBindBuffer;
		public const int GL_ARRAY_BUFFER = 0x8892;
		public const int GL_ELEMENT_ARRAY_BUFFER = 0x8893;

		public delegate void BufferData(int target, IntPtr size, IntPtr data, int usage);
		public static BufferData glBufferData;
		public const int GL_DYNAMIC_DRAW = 0x88E8;

		public delegate void BufferSubData(int target, IntPtr offset, IntPtr size, IntPtr data);
		public static BufferSubData glBufferSubData;

		public delegate void GenVertexArrays(int n, out uint arrays);
		public static GenVertexArrays glGenVertexArrays;

		public delegate void BindVertexArray(uint array);
		public static BindVertexArray glBindVertexArray;

		public delegate void EnableVertexAttribArray(int index);
		public static EnableVertexAttribArray glEnableVertexAttribArray;

		public delegate void VertexAttribPointer(uint index, int size, int type, bool normalized, int stride, IntPtr data);
		public static VertexAttribPointer glVertexAttribPointer;
		public const int GL_FLOAT = 0x1406;

		public delegate void DrawElements(int mode, int count, int type, IntPtr indices);
		public static DrawElements glDrawElements;
		public const int GL_TRIANGLES = 0x0004;
		public const int GL_UNSIGNED_SHORT = 0x1403;

		public delegate int GetError ();
		public static GetError glGetError;

		public delegate void Enable(int flags);
		public static Enable glEnable;
		public const int GL_BLEND = 0x0BE2;

		public delegate void BlendFunc(int sfactor, int dfactor);
		public static BlendFunc glBlendFunc;
		public const int GL_SRC_ALPHA = 0x0302;
		public const int GL_ONE_MINUS_SRC_ALPHA = 0x0303;

		delegate void ShaderInfoLog(uint shader, int maxLength, out int length, IntPtr infoLog);
		static ShaderInfoLog glGetShaderInfoLog;

		public static string GetShaderInfoLog(uint shader)
		{
			int len;
			var ptr = Marshal.AllocHGlobal (4096);
			glGetShaderInfoLog (shader, 4096, out len, ptr);
			var str = Marshal.PtrToStringAnsi (ptr, len);
			Marshal.FreeHGlobal (ptr);
			return str;
		}

		delegate void ProgramInfoLog(uint shader, int maxLength, out int length, IntPtr infoLog);
		static ProgramInfoLog glGetProgramInfoLog;

		public static string GetProgramInfoLog(uint program)
		{
			int len;
			var ptr = Marshal.AllocHGlobal (4096);
			glGetProgramInfoLog (program, 4096, out len, ptr);
			var str = Marshal.PtrToStringAnsi (ptr, len);
			Marshal.FreeHGlobal (ptr);
			return str;
		}
		#if DEBUG
		public static void Load(bool checkErrors = true)
		#else
		public static void Load(bool checkErrors = false)
		#endif
		{
			var t = typeof (GL);
			foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)) {

				if (f.FieldType.BaseType == typeof(MulticastDelegate) ||
					f.FieldType.BaseType == typeof(Delegate)) {
					var ptr = SDL2.SDL_GL_GetProcAddress (f.Name);
					var del = Marshal.GetDelegateForFunctionPointer (ptr, f.FieldType);
					Delegate finalDel = del;
					if (f.Name != "glGetError" && f.Name != "glGetShaderInfoLog" && checkErrors) {
						MethodInfo checkMethod = 
							typeof(GL).GetMethod("CheckError", 
								BindingFlags.Public | BindingFlags.Static);
						var parms = GetDelegateParameterTypes (f.FieldType);
						var ret = GetDelegateReturnType (f.FieldType);
						DynamicMethod handler = new DynamicMethod (
							"",
							ret,
							parms
						);
						var ilgen = handler.GetILGenerator ();
						for (int i = 0; i < parms.Length; i++) {
							ilgen.Emit (OpCodes.Ldarg, i);
						}
						ilgen.Emit (OpCodes.Call, del.Method);
						ilgen.Emit (OpCodes.Ldstr, f.Name);
						ilgen.Emit (OpCodes.Call, checkMethod);
						ilgen.Emit (OpCodes.Ret);
						finalDel = handler.CreateDelegate (f.FieldType);
					}
					f.SetValue (null, finalDel);
				}
			}
		}
		private static Type[] GetDelegateParameterTypes(Type d)
		{
			if (d.BaseType != typeof(MulticastDelegate))
				throw new ApplicationException("Not a delegate.");

			MethodInfo invoke = d.GetMethod("Invoke");
			if (invoke == null)
				throw new ApplicationException("Not a delegate.");

			ParameterInfo[] parameters = invoke.GetParameters();
			Type[] typeParameters = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				typeParameters[i] = parameters[i].ParameterType;
			}
			return typeParameters;
		}

		private static Type GetDelegateReturnType(Type d)
		{
			if (d.BaseType != typeof(MulticastDelegate))
				throw new ApplicationException("Not a delegate.");

			MethodInfo invoke = d.GetMethod("Invoke");
			if (invoke == null)
				throw new ApplicationException("Not a delegate.");

			return invoke.ReturnType;
		}
		enum GLError {
			GL_NO_ERROR = 0,
			GL_INVALID_ENUM = 0x500,
			GL_INVALID_VALUE = 0x501,
			GL_INVALID_OPERATION = 0x502,
			GL_OUT_OF_MEMORY = 0x505
		}
		public static void CheckError(string method)
		{
			GLError error = GLError.GL_NO_ERROR;
			while ((error = (GLError)glGetError ()) != GLError.GL_NO_ERROR) {
				throw new Exception (string.Format ("GL Error: {0} - {1}", method, error));
			}
		}
	}
}

