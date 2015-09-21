bubbleinternal = {}

-- Functions for making a table read-only (good for enumerations)

local err = error --No overriding error!

function bubbleinternal.readonlytable(t)
	local proxy = {}
	local mt = {
		-- hide the actual table being accessed
		__metatable = "read only table",
		__len = #t,
		__index = function(tab, k) return t[k] end,
		__pairs = function() return pairs(t) end,
		__newindex = function (t,k,v)
			err("attempt to update a read-only table", 2)
		end
	}
	setmetatable(proxy, mt)
	return proxy
end

-- Add a pairs operator to the metatable
local oldpairs = pairs
function pairs(t)
	local mt = getmetatable(t)
	if mt==nil then
		return oldpairs(t)
	elseif type(mt.__pairs) ~= "function" then
		return oldpairs(t)
	end
	
	return mt.__pairs()
end

-- Block rawset on read-only tables
local oldrawset = rawset
function rawset(t, k, v)
	local mt = getmetatable(t)
	if mt=="read only table" then
		err("attempt to update a read-only table (rawset)")
	end
	return oldrawset(t,k,v)
end

