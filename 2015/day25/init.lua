local lib = require('lib')

local function calc(code)
  return (code * 252533) % 33554393
end
local target_x = 3075
local target_y = 2981

local code = 20151125
local x = 1
local y = 1
local last_y = 1
local counter = 0
while true do
  if y == 1 then
    y = last_y + 1
    x = 1
    last_y = last_y + 1
  else
    x = x + 1
    y = y - 1
  end
  code = calc(code)
  if x == target_x and y == target_y then
    print(code)
    break
  end
end
