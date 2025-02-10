local lines = require 'lib'.getLines()

local visitedPoints = {}

local function visit(hash)
  if visitedPoints[hash] then visitedPoints[hash] = visitedPoints[hash] + 1 else visitedPoints[hash] = 1 end
end
for _, line in ipairs(lines) do
  local match = string.gmatch(line, "(%d+)")
  local x1 = tonumber(match())
  local y1 = tonumber(match())
  local x2 = tonumber(match())
  local y2 = tonumber(match())
  -- vertical
  if x1 == x2 or y1 == y2 then
    if x1 == x2 then
      if y1 < y2 then
        for i = y1, y2 do
          visit(x1 .. '.' .. i)
        end
      else
        for i = y2, y1 do
          visit(x1 .. '.' .. i)
        end
      end
    end
    if y1 == y2 then
      if x1 < x2 then
        for i = x1, x2 do
          visit(i .. '.' .. y1)
        end
      else
        for i = x2, x1 do
          visit(i .. '.' .. y1)
        end
      end
    end
  end

  -- diagonal
  local distanceX = math.abs(x2 - x1)
  local distanceY = math.abs(y2 - y1)
  if distanceX == distanceY then
    local diffX = (x2 - x1 > 0 and 1 or -1)
    local diffY = (y2 - y1 > 0 and 1 or -1)
    for i = 0, distanceX do
      visit(x1 + i * diffX .. '.' .. y1 + i * diffY)
    end
  else
  end
end

local sum = 0
for k, v in pairs(visitedPoints) do
  if v > 1 then sum = sum + 1 end
end
print(sum)
