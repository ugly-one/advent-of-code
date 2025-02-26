local lib = require('lib')

local target = { x = 7, y = 4 }
local favorite = 10

target = { x = 31, y = 39 }
favorite = 1362

local function is_open_space(x, y)
  local a = x * x + 3 * x + 2 * x * y + y + y * y
  a = a + favorite
  local binary = lib.to_bits(a)
  local count = 0
  for i = 1, #binary do
    if binary[i] == 1 then count = count + 1 end
  end
  if count % 2 == 0 then return true else return false end
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
  local start = { x = 1, y = 1 }
  locations_to_distance[hash(start)] = 0
  mark_distance(start, hash(target), 1, locations_to_distance)
  print(locations_to_distance[hash(target)])
end

part1()

local function mark_distance_ver2(location, steps, max_steps, locations_to_distance)
  if steps > max_steps then return end
  local locations_to_consider = {
    { x = location.x,     y = location.y - 1 },
    { x = location.x - 1, y = location.y },
    { x = location.x + 1, y = location.y },
    { x = location.x,     y = location.y + 1 },
  }
  for _, location in ipairs(locations_to_consider) do
    if location.x >= 0 and location.y >= 0 then
      if is_open_space(location.x, location.y) then
        local hash = hash(location)
        if locations_to_distance[hash] then
          if locations_to_distance[hash] > steps then
            locations_to_distance[hash] = steps
            mark_distance_ver2(location, steps + 1, max_steps, locations_to_distance)
          end
        else
          locations_to_distance[hash] = steps
          mark_distance_ver2(location, steps + 1, max_steps, locations_to_distance)
        end
      end
    end
  end
end

local function part2()
  local max_steps = 50
  local locations_to_distance = {}
  local start = { x = 1, y = 1 }
  locations_to_distance[hash(start)] = 0
  mark_distance_ver2(start, 1, max_steps, locations_to_distance)
  local counter = 0
  for k, v in pairs(locations_to_distance) do
    counter = counter + 1
  end
  print(counter)
end
part2()
