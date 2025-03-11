local lib = require('lib')
local input = lib.getLines()

local virus_pos = math.ceil(#input / 2)
virus_pos = { x = virus_pos, y = virus_pos }

local infected_nodes = {}
local function hash(x, y)
  return x .. '.' .. y
end
for y = 1, #input do
  local line = lib.to_table(input[y])
  for x = 1, #line do
    if line[x] == '#' then infected_nodes[hash(x - virus_pos.x, y - virus_pos.y)] = "I" end
  end
end

virus_pos = { x = 0, y = 0 }
local direction = "up"

local function left(direction)
  if direction == "up" then
    return "left"
  elseif direction == "right" then
    return "up"
  elseif direction == "down" then
    return "right"
  elseif direction == "left" then
    return "down"
  end
end

local function right(direction)
  if direction == "up" then
    return "right"
  elseif direction == "right" then
    return "down"
  elseif direction == "down" then
    return "left"
  elseif direction == "left" then
    return "up"
  end
end

local function reverse(direction)
  if direction == "up" then
    return "down"
  elseif direction == "right" then
    return "left"
  elseif direction == "down" then
    return "up"
  elseif direction == "left" then
    return "right"
  end
end

local function move(pos, direction)
  if direction == "up" then
    return { x = pos.x, y = pos.y - 1 }
  elseif direction == "right" then
    return { x = pos.x + 1, y = pos.y }
  elseif direction == "down" then
    return { x = pos.x, y = pos.y + 1 }
  elseif direction == "left" then
    return { x = pos.x - 1, y = pos.y }
  end
end

local infections = 0
for i = 1, 10000000 do
  local hash_pos = hash(virus_pos.x, virus_pos.y)
  local state = infected_nodes[hash_pos]
  if state == "I" then
    direction = right(direction)
    infected_nodes[hash_pos] = "F"
  elseif state == "F" then
    direction = reverse(direction)
    infected_nodes[hash_pos] = nil
  elseif state == "W" then
    infected_nodes[hash_pos] = "I"
    infections = infections + 1
  elseif state == nil then
    direction = left(direction)
    infected_nodes[hash_pos] = "W"
  end
  virus_pos = move(virus_pos, direction)
end
print(infections)
