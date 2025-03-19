local lib = require('lib')
local input = lib.getLines('input.txt')
local size = #input
local map = {}
for _, line in ipairs(input) do
  table.insert(map, lib.to_table(line))
end

local function print_map(map)
  for y = 1, size do
    local line = ""
    for x = 1, size do
      line = line .. map[y][x]
    end
    print(line)
  end
end

local function convert(x, y, map)
  local trees = 0
  local opens = 0
  local lumberyards = 0
  local function update_stats(x, y)
    if map[y][x] == '.' then opens = opens + 1 end
    if map[y][x] == '|' then trees = trees + 1 end
    if map[y][x] == '#' then lumberyards = lumberyards + 1 end
  end
  -- row above
  if y - 1 >= 1 then
    if x - 1 >= 1 then
      update_stats(x - 1, y - 1)
    end
    update_stats(x, y - 1)
    if x + 1 <= size then
      update_stats(x + 1, y - 1)
    end
  end
  -- row in the center
  if x - 1 >= 1 then
    update_stats(x - 1, y)
  end
  if x + 1 <= size then
    update_stats(x + 1, y)
  end
  -- row below
  if y + 1 <= size then
    if x - 1 >= 1 then
      update_stats(x - 1, y + 1)
    end
    update_stats(x, y + 1)
    if x + 1 <= size then
      update_stats(x + 1, y + 1)
    end
  end
  if map[y][x] == '.' and trees >= 3 then
    return '|'
  elseif map[y][x] == '.' then
    return '.'
  elseif map[y][x] == '|' and lumberyards >= 3 then
    return '#'
  elseif map[y][x] == '|' then
    return '|'
  elseif map[y][x] == '#' and lumberyards >= 1 and trees >= 1 then
    return '#'
  else
    return '.'
  end
end

local function get_stats(map)
  local trees = 0
  local lumberyards = 0
  for y = 1, size do
    for x = 1, size do
      if map[y][x] == '|' then
        trees = trees + 1
      elseif map[y][x] == '#' then
        lumberyards = lumberyards + 1
      end
    end
  end
  return trees, lumberyards
end

local stats = {}
local time = 1000000000
for minute = 1, time do
  local new_map = {}
  for y = 1, size do
    local row = {}
    for x = 1, size do
      table.insert(row, '.')
    end
    table.insert(new_map, row)
  end
  for y = 1, size do
    for x = 1, size do
      new_map[y][x] = convert(x, y, map)
    end
  end
  map = new_map

  local trees, lumberyards = get_stats(map)
  local hash = trees .. '.' .. lumberyards
  if stats[hash] then
    table.insert(stats[hash], minute)
  else
    stats[hash] = { minute }
  end
  for k, v in pairs(stats) do
    local line = ""
    for _, m in ipairs(v) do
      line = line .. ' ' .. m
    end
    print(k, line)
  end

  print('.......................')
end

local trees, lumberyards = get_stats(map)
print(trees * lumberyards)

-- part 2 was solved by observing that after minute 600 there was a loop
-- a loop that repeated each 28 minutes.
-- by accident I found out that 1000000000 - 608 is divisible by 2 and that means that what I see on minute 608 is what we will see on minute 1000000000.
