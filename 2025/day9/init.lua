local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local min_x
local min_y
local max_x = 0
local max_y = 0
local red_tiles = {}
local points = {}

for _, line in ipairs(input) do
  local x_string, y_string = string.match(line, '(%d+),(%d+)')
  local x = tonumber(x_string)
  local y = tonumber(y_string)
  table.insert(red_tiles, { x = x, y = y })
  if min_x == nil or x < min_x then min_x = x end
  if x > max_x then max_x = x end
  if min_y == nil or y < min_y then min_y = y end
  if y > max_y then max_y = y end
  points[x .. ',' .. y] = 1
end

local min_point = { x = min_x, y = min_y }
local max_point = { x = max_x, y = max_y }

local map = {}
for x = min_x - 1, max_x + 1 do
  local line = ''
  for y = min_y - 1, max_y + 1 do
    local char
    if points[x .. ',' .. y] ~= nil then char = 'x'
    else char = '.'
    end
    line = line .. char
  end
  table.insert(map, line)
end

lib.print_strings_array(map)

local part1 = function()
  local max_area = 0
  for i = 1, #red_tiles do
    local corner1 = red_tiles[i]
    for j = i + 1, #red_tiles - 1 do
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
end

