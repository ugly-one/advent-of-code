local lines = require 'lib'.getLines()

local points = {}

local function run(line, action)
  local point = { x = 0, y = 0 }
  local time = 1
  local paths = string.gmatch(line, "(%a%d+)")
  for path in paths do
    local direction = string.sub(path, 1, 1)
    local distance = string.sub(path, 2)
    if direction == 'R' then
      for i = 1, distance do
        point = { x = point.x + 1, y = point.y }
        action(point, time)
        time = time + 1
      end
    elseif direction == 'L' then
      for i = 1, distance do
        point = { x = point.x - 1, y = point.y }
        action(point, time)
        time = time + 1
      end
    elseif direction == 'D' then
      for i = 1, distance do
        point = { x = point.x, y = point.y + 1 }
        action(point, time)
        time = time + 1
      end
    elseif direction == 'U' then
      for i = 1, distance do
        point = { x = point.x, y = point.y - 1 }
        action(point, time)
        time = time + 1
      end
    end
  end
end

local crossPoints = {}
run(lines[1], function(point, time)
  local hash = point.x .. '.' .. point.y
  if not points[hash] then points[hash] = time end
end)

run(lines[2], function(point, time)
  local hash = point.x .. '.' .. point.y
  if points[hash] then
    crossPoints[point] = points[hash] + time
  end
end)
local min = 999999999
for point, a in pairs(crossPoints) do
  if a < min then min = a end
end
print(min)
