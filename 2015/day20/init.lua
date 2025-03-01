local lib = require('lib')
local input = lib.getLines()

local target = 33100000

local houses = {}
for elf = 1, target do
  local visits = 0
  for house = elf, target, elf do
    if visits > 50 then break end
    if houses[house] == nil then houses[house] = 0 end
    houses[house] = houses[house] + elf * 11
    visits = visits + 1
  end
end

for i, presents in pairs(houses) do
  if presents > target then
    print(i)
    return
  end
end
