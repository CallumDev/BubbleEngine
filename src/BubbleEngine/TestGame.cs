using System;
using System.IO;
using System.Reflection;
using NLua;
namespace BubbleEngine
{
	public class TestGame : GameBase
	{
		SpriteBatch spriteBatch;
		Lua state;
		LuaTable gameTable;
		LuaFunction drawFunction;
		LuaFunction updateFunction;
		public TestGame ()
		{
			Window.Title = "Bubble Engine";
		}
			
		protected override void Load ()
		{
			spriteBatch = new SpriteBatch (Window);
			//load fonts
			FontContext.LoadFallback("../../TestAssets/DroidSansFallback.ttf");
			state = new Lua ();
			state.LoadCLRPackage ();
			//create lua state
			state.RegisterFunction (
				"println", 
				typeof(Console).GetMethod (
					"WriteLine", 
					new Type[] { typeof(string) }
				)
			);
			state ["runtime"] = new LuaAPI.Runtime();
			state ["fonts"] = new LuaAPI.Fonts (FontContext);
			state ["graphics"] = new LuaAPI.Graphics (spriteBatch);
			state ["window"] = new LuaAPI.LWindow (Window);
			state ["keyboard"] = new LuaAPI.LKeyboard (Keyboard);
			LuaAPI.Util.RegisterEnum (typeof(Keys), state);
			state ["embedres"] = new LuaAPI.EmbeddedLoader ();
			//run init scripts
			state.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.procure.lua"));
			state.DoString(EmbeddedResources.GetString("BubbleEngine.LuaAPI.init.lua"));
			//run
			state.DoString (File.ReadAllText ("test.lua"));
			gameTable = (LuaTable)state ["game"];
			var ld = (LuaFunction)gameTable ["load"];
			ld.Call ();
			updateFunction = (LuaFunction)gameTable ["update"];
			drawFunction = (LuaFunction)gameTable ["draw"];
		}
		protected override void Update (GameTime gameTime)
		{
			updateFunction.Call (gameTime);
		}
		protected override void Draw (GameTime gameTime)
		{
			//Test: red, blue, green + white rectangles
			spriteBatch.Begin ();
			drawFunction.Call (gameTime);
			spriteBatch.End ();
		}
	}
}

