using System;
using NLua;
namespace BubbleEngine
{
	//Extended Lua State
	class BubbleLua
	{
		public Lua Lua;

		public BubbleLua (Lua lua, bool init = true)
		{
			Lua = lua;
			if (!init)
				return;
			lua.DoString (EmbeddedResources.GetString ("BubbleEngine.LuaAPI.bubbleinternal.lua"));
		}
	}
}

