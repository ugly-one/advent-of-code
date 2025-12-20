local lib = require('lib')
local input = lib.getLines('test.txt')

local function to_hash(point)
  return point.x .. '.' .. point.y
end

local clay = {}
local water = {}
local still_water = {}
local source = { x = 500, y = 0 }
local first_water = { x = 500, y = 1 }
water[to_hash(first_water)] = first_water
local min_x = 500
local max_x = 500
local min_y = nil
local max_y = 0
for _, line in ipairs(input) do
  local x_min, x_max = string.match(line, "x=(%d+)%.%.(%d+)")
  if x_min == nil then
    x_min = tonumber(string.match(line, "x=(%d+)"))
    x_max = x_min
  else
    x_min = tonumber(x_min)
    x_max = tonumber(x_max)
  end
  local y_min, y_max = string.match(line, "y=(%d+)%.%.(%d+)")
  if y_min == nil then
    y_min = tonumber(string.match(line, "y=(%d+)"))
    y_max = y_min
  else
    y_min = tonumber(y_min)
    y_max = tonumber(y_max)
  end
  for y = y_min, y_max do
    for x = x_min, x_max do
      clay[x .. '.' .. y] = true
      if x < min_x then min_x = x end
      if x > max_x then max_x = x end
      if min_y == nil or y < min_y then min_y = y end
      if y > max_y then max_y = y end
    end
  end
end


local function print_map()
  local limit
  if max_y > 100 then
    limit = 100
  else
    limit = max_y
  end
  for y = min_y - 10, limit do
    local line = ""
    for x = min_x - 1, max_x + 1 do
      local hash = to_hash({ x = x, y = y })
      if x == source.x and y == source.y then
        line = line .. '+'
      elseif water[hash] then
        line = line .. '|'
      elseif clay[hash] then
        line = line .. "#"
      else
        line = line .. ' '
      end
    end
    print(line)
  end
end



local function down(point)
  return { x = point.x, y = point.y + 1 }
end

local function left(point)
  return { x = point.x - 1, y = point.y }
end
local function right(point)
  return { x = point.x + 1, y = point.y }
end

local function count(map)
  local counter = 0
  for k, v in pairs(map) do
    counter = counter + 1
  end
  return counter
end

local function fill(point, point_hash)
  local points = {}
  return true, points
end

local counter = 0
while true do
  print_map()
  local answer = io.read()
  os.execute("clear")

  local new_water = {}
  for water_hash, water in pairs(water) do
    local below = down(water)
    local below_hash = to_hash(below)
    if clay[below_hash] or still_water[below_hash] then
      -- fill out entire line
      -- I should be able to detect if we overflown an edge
      local is_overflow, points = fill(water, water_hash)
      if is_overflow then

      else
        -- not overflown
        -- mark the entire line as still_water
        for _, point in ipairs(points) do
          still_water[to_hash(point)] = point
        end
        new_water[water_hash] = water
      end
    elseif water[below_hash] then
      -- do nothing, there is anothor water below
      new_water[water_hash] = water
    else
      -- fall
      new_water[water_hash] = water
      new_water[below_hash] = below
    end
  end
  water = new_water
end

print(counter)
