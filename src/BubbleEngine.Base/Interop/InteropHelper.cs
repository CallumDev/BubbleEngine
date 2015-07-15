﻿using System;
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
					var del = Marshal.GetDelegateForFunctionPointer (ptr, f.FieldType);
					f.SetValue (null, del);
				}
			}
		}
	}
}

