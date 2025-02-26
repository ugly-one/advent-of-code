local lib = require('lib')
local input = lib.getLines()
local map = {}
local stops = {}
for i, line in ipairs(input) do
  map[i] = {}
  for j, char in ipairs(lib.to_table(line)) do
    map[i][j] = char
    if tonumber(char) then
      stops[char] = { x = j, y = i }
    end
  end
end
vim.print(stops)
local function print_map(map)
  for i, line in ipairs(map) do
    local line = ""
    for j, char in ipairs(map[i]) do
      line = line .. char
    end
    print(line)
  end
end

local function is_open_space(x, y)
  if map[y][x] ~= '#' then return true else return false end
end

local function hash(location)
  return location.x .. ',' .. location.y
end

local function mark_distance(location, target, steps, locations_to_distance)
  if locations_to_distance[target] and steps > locations_to_distance[target] then return end

  local locations_to_consider = {
    { x = location.x,     y = location.y - 1 },
    { x = location.x - 1, y = location.y },
    { x = location.x + 1, y = location.y },
    { x = location.x,     y = location.y + 1 },
  }
  for _, location in ipairs(locations_to_consider) do
    if (location.x >= 0 and location.y >= 0)
        and is_open_space(location.x, location.y) then
      local location_hash = hash(location)
      if locations_to_distance[location_hash] then
        if locations_to_distance[location_hash] > steps then
          locations_to_distance[location_hash] = steps
          mark_distance(location, target, steps + 1, locations_to_distance)
        end
      else
        locations_to_distance[location_hash] = steps
        mark_distance(location, target, steps + 1, locations_to_distance)
      end
    end
  end
end

local function part1()
  local locations_to_distance = {}
  local start = stops['0']
  local target = stops['1']
  locations_to_distance[hash(start)] = 0
  mark_distance(start, hash(target), 1, locations_to_distance)
  print(locations_to_distance[hash(target)])
end

part1()
