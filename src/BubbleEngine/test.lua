function handle_keydown(e)
	print(e.Key)
end

function game.load()
	font = fonts:load("../../TestAssets/OpenSans-Regular.ttf", 16)
	window:setTitle("Hello Lua")
	keyboard.keyDown:Add(handle_keydown)
end

function game.draw(t)
	graphics:fillRectangle(0, 0, 800, 600, color4.blue)
	graphics:drawString(font, "Hello World!", 10, 10)
end

function game.update(t)

end
