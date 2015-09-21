function game.config()
	window:setTitle("Hello Lua")
	graphics:setResolution(640,480)
end

function game.load()
	font = fonts:load("../../TestAssets/OpenSans-Regular.ttf", 16)
	keyboard.keyDown:Add(handle_keydown)
end

function handle_keydown(e)
	print(keys.getString(e.key))
end

function game.draw(t)
	if keyboard:isDown(keys.space) then
		graphics:fillRectangle(0, 0, 800, 600, color4.cornflowerBlue)
	else
		graphics:fillRectangle(0, 0, 800, 600, color4.crimson)
	end
	graphics:drawString(font, "Hello World!", 12, 12, color4.black)
	graphics:drawString(font, "Hello World!", 10, 10, color4.wheat)
end

function game.update(t)
	--print("elapsed: " .. t.elapsed .. ", total: " .. t.total)
end
