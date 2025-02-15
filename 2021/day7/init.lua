local lib = require 'lib'

local line = lib.getLines()[1]

local numbers = {}

local match = string.gmatch(line, "%d+")
local min = 9999
local max = 0
for m in match do
  local n = tonumber(m)
  if n > max then max = n end
  if n < min then min = n end
  table.insert(numbers, n)
end

local min_fuel = nil
for i = min, max do
  local fuel = 0
  for n = 1, #numbers do
    local number = numbers[n]

    local diff = math.abs(number - i)
    for x = 1, diff do
      fuel = fuel + x
    end
  end
  if min_fuel == nil then min_fuel = fuel elseif fuel < min_fuel then min_fuel = fuel end
end
print(min_fuel)
