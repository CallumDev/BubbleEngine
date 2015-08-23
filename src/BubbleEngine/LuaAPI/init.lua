-- Enums
require 'bit32'
spriteEffects = {
	none = 0,
	flipHorizontal = 1,
	flipVertical = 2,
	band = bit32.band
}

-- TODO: Create Sandbox
println("Initialised");
-- Create empty game object
game = {}