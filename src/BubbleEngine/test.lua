function game.config()
	bubble.window:setTitle("Hello Lua")
	bubble.graphics:setResolution(640,480)
end

function game.load()
	font = bubble.fonts:load("../../TestAssets/OpenSans-Regular.ttf", 16)
	bubble.keyboard.keyDown:Add(handle_keydown)
end

function handle_keydown(e)
	print(bubble.keys.getString(e.key))
end

function game.draw(t)
	if bubble.keyboard:isDown(bubble.keys.space) then
		bubble.graphics:fillRectangle(0, 0, 800, 600, bubble.color4.cornflowerBlue)
	else
		bubble.graphics:fillRectangle(0, 0, 800, 600, bubble.color4.crimson)
	end
	bubble.graphics:drawString(font, "Hello World!", 12, 12, bubble.color4.black)
	bubble.graphics:drawString(font, "Hello World!", 10, 10, bubble.color4.wheat)
end

function game.update(t)
	--print("elapsed: " .. t.elapsed .. ", total: " .. t.total)
end
