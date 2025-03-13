local lib = require('lib')
local input = lib.getLines()

local points = {}
local velocities = {}
for _, line in ipairs(input) do
  local numbers = string.gmatch(line, "([0-9-]+)")
  local pos = { x = tonumber(numbers()), y = tonumber(numbers()) }
  local velocity = { x = tonumber(numbers()), y = tonumber(numbers()) }
  table.insert(points, pos)
  table.insert(velocities, velocity)
end

local function text_appeared(points)
  local min_x = nil
  local max_x = nil
  local min_y = nil
  local max_y = nil
  for i, point in ipairs(points) do
    if min_x == nil then min_x = point.x end
    if max_x == nil then max_x = point.x end
    if min_y == nil then min_y = point.y end
    if max_y == nil then max_y = point.y end
    if point.x < min_x then min_x = point.x end
    if point.x > max_x then max_x = point.x end
    if point.y < min_y then min_y = point.y end
    if point.y > max_y then max_y = point.y end
  end

  if max_x - min_x < 100 and max_y - min_y < 10 then
    for y = 1, max_y - min_y + 1 do
      local line = ""
      for x = 1, max_x - min_x + 1 do
        local ok = false
        for _, point in ipairs(points) do
          if point.x == x + min_x - 1 and point.y == y + min_y - 1 then
            ok = true
            break
          end
        end
        if ok then line = line .. '#' else line = line .. ' ' end
      end

      print(line)
    end
    return true
  else
    return false
  end
end

local printed_text = false
local counter = 0
while true do
  local text = text_appeared(points)
  if text then
    print(counter)
    printed_text = true
  end
  -- if we stopped printing text stop the loop so we can investigate
  if printed_text and text == false then break end
  for i, point in ipairs(points) do
    point.x = point.x + velocities[i].x
    point.y = point.y + velocities[i].y
  end
  counter = counter + 1
end
