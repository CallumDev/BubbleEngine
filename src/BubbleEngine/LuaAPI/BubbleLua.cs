using System;
using NLua;
namespace BubbleEngine
{
	//Extended Lua State
	class BubbleLua
	{
		public Lua Lua;
		public LuaTable Bubble;
		public BubbleLua (Lua lua, bool init = true)
		{
			Lua = lua;
			if (!init)
				return;
			Lua.NewTable ("bubble");
			Bubble = (LuaTable)Lua ["bubble"];
			lua.DoString (EmbeddedResources.GetString ("BubbleEngine.LuaAPI.bubbleinternal.lua"));
		}
	}
}

