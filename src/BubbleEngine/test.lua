function game.load()
	font = fonts:load("../../TestAssets/OpenSans-Regular.ttf", 16)
	window:setTitle("Hello Lua")
end

function game.draw(t)
	graphics:drawString(font, "Hello World!", 10, 10)
end

function game.update(t)

end
