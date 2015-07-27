using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace BubbleEngine
{
	//Internal shader abstraction.
	class Shader
	{
		uint programID = 0;
		Dictionary<string, int> progLocations = new Dictionary<string, int> ();
		public Shader (string vertex_source, string fragment_source)
		{
			var vertexHandle = GL.glCreateShader (GL.GL_VERTEX_SHADER);
			var fragmentHandle = GL.glCreateShader (GL.GL_FRAGMENT_SHADER);
			GL.ShaderSource (vertexHandle, vertex_source);
			GL.ShaderSource (fragmentHandle, fragment_source);
			GL.glCompileShader (vertexHandle);
			Console.WriteLine (GL.GetShaderInfoLog (vertexHandle));
			GL.glCompileShader (fragmentHandle);
			Console.WriteLine (GL.GetShaderInfoLog (fragmentHandle));
			programID = GL.glCreateProgram ();
			GL.glAttachShader (programID, vertexHandle);
			GL.glAttachShader (programID, fragmentHandle);
			GL.glBindAttribLocation (programID, 0, "position");
			GL.glBindAttribLocation (programID, 1, "texcoord");
			GL.glBindAttribLocation (programID, 2, "color");
			GL.glLinkProgram (programID);
			Console.WriteLine (GL.GetProgramInfoLog (programID));
		}

		int GetLocation(string name)
		{
			if (!progLocations.ContainsKey (name))
				progLocations [name] = GL.glGetUniformLocation (programID, name);
			return progLocations [name];
		}

		public void SetMatrix(string name, ref Matrix4 mat)
		{
			GL.glUseProgram (programID);
			var handle = GCHandle.Alloc (mat, GCHandleType.Pinned);
			GL.glUniformMatrix4fv (GetLocation (name), 1, false, handle.AddrOfPinnedObject ());
			handle.Free ();
		}

		public void SetInteger(string name, int value)
		{
			GL.glUseProgram (programID);
			GL.glUniform1i (GetLocation (name), value);
		}

		public void UseProgram()
		{
			GL.glUseProgram (programID);
		}
	}
}

