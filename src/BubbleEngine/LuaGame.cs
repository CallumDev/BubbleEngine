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
		string entry;
		public LuaGame (string entryPath)
		{
			Window.Title = "Bubble Engine";
			entry = entryPath;
		}
		protected override void Load ()
		{
			spriteBatch = new SpriteBatch (Window);
			//load fonts
			//FontContext.LoadFallback("../../TestAssets/DroidSansFallback.ttf");
			state = new BubbleLua (new Lua());
			state.Lua.LoadCLRPackage ();
			//create lua state
			state.Lua.RegisterFunction (
				"println", 
				typeof(Console).GetMethod (
					"WriteLine", 
					new Type[] { typeof(string) }
				)
			);
			state.Lua ["runtime"] = new LuaAPI.Runtime();
			state.Lua ["fonts"] = new LuaAPI.Fonts (FontContext);
			state.Lua ["graphics"] = new LuaAPI.Graphics (spriteBatch);
			state.Lua ["window"] = new LuaAPI.LWindow (Window);
			state.Lua ["keyboard"] = new LuaAPI.LKeyboard (Keyboard);
			LuaAPI.Util.RegisterEnum (typeof(Keys), state);
			state.Lua ["embedres"] = new LuaAPI.EmbeddedLoader ();
			//run init scripts
			state.Lua.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.procure.lua"));
			state.Lua.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.init.lua"));
			//run
			state.Lua.DoString (File.ReadAllText (entry), entry);
			gameTable = (LuaTable)state.Lua ["game"];
			var ld = (LuaFunction)gameTable ["load"];
			ld.Call ();
			updateFunction = (LuaFunction)gameTable ["update"];
			drawFunction = (LuaFunction)gameTable ["draw"];
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

