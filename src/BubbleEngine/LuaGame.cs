using System;
using System.IO;
using System.Reflection;
using NLua;
namespace BubbleEngine
{
	public class LuaGame : GameBase
	{
		public GraphicsSettings Graphics {
			get {
				return GraphicsSettings;
			}
		}
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
			state.Bubble ["runtime"] = new LuaAPI.Runtime();
			state.Bubble ["fonts"] = new LuaAPI.Fonts (FontContext);
			luaGraphics = new LuaAPI.Graphics (null, this);
			state.Bubble ["graphics"] = luaGraphics;
			state.Bubble ["window"] = new LuaAPI.LWindow (Window);
			LuaAPI.Util.RegisterEnum (typeof(Keys), state);
			state.Lua ["embedres"] = new LuaAPI.EmbeddedLoader ();
			//run init scripts
			state.Lua.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.procure.lua"));
			state.Lua.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.init.lua"));
			//config
			state.Lua.DoFile(entryPath);
			gameTable = (LuaTable)state.Lua ["game"];
			var cfg = (LuaFunction)gameTable ["config"];
			if (cfg == null) {
				throw new NLua.Exceptions.LuaScriptException ("Script must have a game.config() function", entryPath);
			}
			cfg.Call ();
			updateFunction = (LuaFunction)gameTable ["update"];
			drawFunction = (LuaFunction)gameTable ["draw"];
		}
		protected override void Load ()
		{
			state.Bubble ["keyboard"] = new LuaAPI.LKeyboard (Keyboard);
			spriteBatch = new SpriteBatch (Window);
			luaGraphics.Batch = spriteBatch;
			var ld = (LuaFunction)gameTable ["load"];
			if(ld != null)
				ld.Call ();
		}
		protected override void Update (GameTime gameTime)
		{
			if(updateFunction != null)
				updateFunction.Call (new LuaAPI.LGameTime(gameTime));
		}
		protected override void Draw (GameTime gameTime)
		{
			spriteBatch.Begin ();
			if(drawFunction != null)
				drawFunction.Call (new LuaAPI.LGameTime(gameTime));
			spriteBatch.End ();
		}
	}
}

