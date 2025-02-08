local lines = require('lib').getLines('input.txt')
local steps = #lines - 1
local columns = #lines[1]

local function count(right, down)
  local position = { x = 1, y = 1 }
  local trees = 0
  for i = 1, steps, down do
    local x = position.x + right
    if x > columns then x = (x - columns) end
    position = { x = x, y = position.y + down }
    if string.sub(lines[position.y], position.x, position.x) == '#' then
      trees = trees + 1
    else
    end
  end
  return trees
end

print(count(1, 1) * count(3, 1) * count(5, 1) * count(7, 1) * count(1, 2))
