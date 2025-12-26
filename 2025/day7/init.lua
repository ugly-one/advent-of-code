local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local result = 0
local map = {}
local max_y = 0
for _, line in ipairs(input) do
  table.insert(map, lib.to_table(line))
  max_y = max_y + 1
end

local first_line = map[1]
local beam_x = 0
for x = 1, #first_line do
  local char = first_line[x]
  if char == 'S' then beam_x = x end
end

map[2][beam_x] = '|'

local function simulate(step)
  local row = map[step]
  for x = 1, #row do
    if row[x] == '|' and map[step + 1][x] == '^' then
      map[step + 1][x - 1] = '|'
      map[step + 1][x + 1] = '|'
      result = result + 1
    elseif row[x] == '|' then
      map[step + 1][x] = '|'
    end
  end
end

local function print_map()
  for row = 1, #map do
    local line = ''
    for column = 1, #map[row] do
      line = line .. map[row][column]
    end
    vim.print(line)
  end
end

local step = 2
while step < max_y - 1 do
  simulate(step)
  step = step + 1
end

print('result:', result)
vim.fn.setreg('+', result)
