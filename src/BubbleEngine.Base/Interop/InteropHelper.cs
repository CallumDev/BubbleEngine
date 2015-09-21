using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BubbleEngine
{
	//Helper for loading function pointers
	static class InteropHelper
	{
		public delegate IntPtr LoadFunction(string name);
		public static void LoadFunctions(Type type, LoadFunction func)
		{
			foreach (var f in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)) {
				if (f.FieldType.BaseType == typeof(MulticastDelegate) ||
					f.FieldType.BaseType == typeof(Delegate)) {
					var ptr = func (f.Name);
					if (f.Name.Contains ("$") || f.Name.Contains ("<") || f.Name.Contains (">"))
						continue; //For some reason this reflection stuff catches compiler-internal static variables
					var del = Marshal.GetDelegateForFunctionPointer (ptr, f.FieldType);
					f.SetValue (null, del);
				}
			}
		}
		//Make OSX and Linux path searching behave like windows, check executable directory for libraries first.
		public static string ResolvePath(string path)
		{
			if (Path.IsPathRooted (path))
				return path;
			var myDir = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
			if (File.Exists (Path.Combine (myDir, path)))
				return Path.Combine (myDir, path);
			return path;
		}
	}
}

