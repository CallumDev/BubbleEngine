using System;
using System.IO;
using System.Reflection;
using NLua;
namespace BubbleEngine
{
	public class LuaGame : GameBase
	{
		SpriteBatch spriteBatch;
		BubbleLua state;
		LuaTable gameTable;
		LuaFunction drawFunction;
		LuaFunction updateFunction;
		LuaAPI.Graphics luaGraphics;
		public LuaGame (string entryPath)
		{
			Window.Title = "Bubble Engine";
			//create lua state
			state = new BubbleLua (new Lua());
			state.Lua.LoadCLRPackage ();
			state.Lua ["runtime"] = new LuaAPI.Runtime();
			state.Lua ["fonts"] = new LuaAPI.Fonts (FontContext);
			luaGraphics = new LuaAPI.Graphics (null, GraphicsSettings);
			state.Lua ["graphics"] = luaGraphics;
			state.Lua ["window"] = new LuaAPI.LWindow (Window);
			LuaAPI.Util.RegisterEnum (typeof(Keys), state);
			state.Lua ["embedres"] = new LuaAPI.EmbeddedLoader ();
			//run init scripts
			state.Lua.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.procure.lua"));
			state.Lua.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.init.lua"));
			//config
			state.Lua.DoFile(entryPath);
			gameTable = (LuaTable)state.Lua ["game"];
			var cfg = (LuaFunction)gameTable ["config"];
			cfg.Call ();
			updateFunction = (LuaFunction)gameTable ["update"];
			drawFunction = (LuaFunction)gameTable ["draw"];
		}
		protected override void Load ()
		{
			state.Lua ["keyboard"] = new LuaAPI.LKeyboard (Keyboard);
			spriteBatch = new SpriteBatch (Window);
			luaGraphics.Batch = spriteBatch;
			var ld = (LuaFunction)gameTable ["load"];
			ld.Call ();
		}
		protected override void Update (GameTime gameTime)
		{
			updateFunction.Call (new LuaAPI.LGameTime(gameTime));
		}
		protected override void Draw (GameTime gameTime)
		{
			spriteBatch.Begin ();
			drawFunction.Call (new LuaAPI.LGameTime(gameTime));
			spriteBatch.End ();
		}
	}
}

