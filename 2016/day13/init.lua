local lib = require('lib')
local favorite = 10

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

local target = { x = 7, y = 4 }
target = { x = 31, y = 39 }
favorite = 1362
local current_location = { x = 1, y = 1 }

local function hash(location)
  return location.x .. ',' .. location.y
end

local function part1()
  local min_steps = 9999999
  local function find_shortest(location, steps, visited_locations)
    if location.x == target.x and location.y == target.y then
      if steps < min_steps then
        min_steps = steps
      end
      return
    end
    local locations_to_explore = {}
    local locations_to_consider = {
      { x = location.x,     y = location.y - 1 },
      { x = location.x - 1, y = location.y },
      { x = location.x + 1, y = location.y },
      { x = location.x,     y = location.y + 1 },
    }
    for _, location in ipairs(locations_to_consider) do
      if location.x >= 0 and location.y >= 0 then
        if visited_locations[hash(location)] then
        else
          if is_open_space(location.x, location.y) then
            table.insert(locations_to_explore, location)
          end
        end
      end
    end

    for _, location in ipairs(locations_to_explore) do
      local copy = {}
      for k, v in pairs(visited_locations) do
        copy[k] = v
      end
      copy[hash(location)] = true
      find_shortest(location, steps + 1, copy)
    end
  end
  local visited = {}
  visited[hash({ x = 1, y = 1 })] = true
  find_shortest({ x = 1, y = 1 }, 0, visited)
  print(min_steps)
end

local function part2()
  local max_steps = 50
  local locations_to_distance = {}
  locations_to_distance[hash({ x = 1, y = 1 })] = 0

  local function mark_distance(location, steps)
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
              mark_distance(location, steps + 1)
            end
          else
            locations_to_distance[hash] = steps
            mark_distance(location, steps + 1)
          end
        end
      end
    end
  end
  mark_distance({ x = 1, y = 1 }, 1)
  local counter = 0
  for k, v in pairs(locations_to_distance) do
    counter = counter + 1
  end
  print(counter)
end
part1()
part2()
