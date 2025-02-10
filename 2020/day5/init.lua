local function get_seat(pass)
  local bottom = 1
  local top = 128
  for i = 1, 7 do
    local letter = string.sub(pass, i, i)
    if letter == 'F' then
      top = top - (top - bottom + 1) / 2
    else
      bottom = bottom + (top - bottom + 1) / 2
    end
  end
  local row = bottom - 1
  local bottom = 1
  local top = 8
  for i = 1, 3 do
    local letter = string.sub(pass, 7 + i, 7 + i)
    if letter == 'L' then
      top = top - (top - bottom + 1) / 2
    else
      bottom = bottom + (top - bottom + 1) / 2
    end
  end
  local column = bottom - 1
  return (row * 8 + column)
end

local lines = require 'lib'.getLines()
local plane = {}
for _, line in ipairs(lines) do
  local seat = get_seat(line)
  table.insert(plane, seat)
end
table.sort(plane)

local previous
for _, k in pairs(plane) do
  if not previous then
    previous = k
  else
    local diff = k - previous
    if diff > 1 then print(k - 1) end
  end
  previous = k
end
