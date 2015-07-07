#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;

namespace BubbleEngine
{
	public partial class SpriteBatch
	{
		const string vertex_source = @"
#version 140

in vec2 position;
in vec2 texcoord;
in vec4 color;
out vec2 out_texcoord;
out vec4 blendColor;
uniform mat4x4 modelviewproj;

void main()
{
    gl_Position = modelviewproj * vec4(position, 0.0, 1.0);
    blendColor = color;
    out_texcoord = texcoord;
}
";
		const string fragment_source = @"
#version 140

in vec2 out_texcoord;
in vec4 blendColor;

out vec4 out_color;

uniform sampler2D tex;

void main()
{
	out_color = texture(tex, out_texcoord) * blendColor;
}
";
	}
}