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
local function print_map(map)
  for i, line in ipairs(map) do
    local line = ""
    for j, char in ipairs(map[i]) do
      line = line .. char
    end
    print(line)
  end
end

local function get_pairs(stops)
  local result = {}

  for i = 1, #stops - 1 do
    local l1 = stops[i]
    for j = i + 1, #stops do
      local l2 = stops[j]
      table.insert(result, { l1, l2 })
    end
  end
  return result
end

local function get_permutations(stops)
  local result = {}

  local function get_permutations_rec(current, remaining, result)
    if #remaining == 0 then
      table.insert(result, current)
      return
    end
    for i = 1, #remaining do
      local new_remaining = lib.copy_table(remaining)
      table.remove(new_remaining, i)
      local new_current = lib.copy_table(current)
      table.insert(new_current, remaining[i])
      get_permutations_rec(new_current, new_remaining, result)
    end
  end
  get_permutations_rec({}, stops, result)
  return result
end

local stop_names = {}
for k, v in pairs(stops) do
  table.insert(stop_names, k)
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
  local pairs = get_pairs(stop_names)
  local distances = {}
  for _, pair in ipairs(pairs) do
    local locations_to_distance = {}
    local target = stops[pair[1]]
    local start = stops[pair[2]]
    locations_to_distance[hash(start)] = 0
    mark_distance(start, hash(target), 1, locations_to_distance)
    local distance = locations_to_distance[hash(target)]
    distances[pair[1] .. pair[2]] = distance
  end

  print('done calculating all distances')

  local stop_names_without_start = {}
  for _, name in ipairs(stop_names) do
    if name ~= '0' then table.insert(stop_names_without_start, name) end
  end

  local min = nil
  local permutations = get_permutations(stop_names_without_start)
  for _, permutation in ipairs(permutations) do
    table.insert(permutation, '0')
    local total_distance = 0
    local stop1 = '0'
    for _, stop2 in ipairs(permutation) do
      if distances[stop1 .. stop2] then
        total_distance = total_distance + distances[stop1 .. stop2]
      elseif distances[stop2 .. stop1] then
        total_distance = total_distance + distances[stop2 .. stop1]
      else
        print('?????')
      end
      stop1 = stop2
    end

    if min == nil or total_distance < min then min = total_distance end
  end
  print(min)
end

part1()
