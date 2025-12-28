local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local red_tiles = {}
for _,line in ipairs(input) do
  local x,y = string.match(line, '(%d+),(%d+)')
  table.insert(red_tiles, { x = tonumber(x), y = tonumber(y) })
end
-- lib.print(red_tiles)

local max_area = 0
for i=1, #red_tiles do
  local corner1 = red_tiles[i]
  for j=i+1, #red_tiles - 1 do
    local corner2 = red_tiles[j]
    local area = math.abs(corner1.x - corner2.x + 1) * math.abs(corner1.y - corner2.y + 1)

    if max_area == nil or area > max_area then
      max_area = area
    end
  end
end
local result = max_area

print('result:', result)
vim.fn.setreg('+', result)

