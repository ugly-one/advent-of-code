local lib = require('lib')
local lines = lib.getLines()
local rows = #lines / 3
local columns = 3

local function bla(line)
  local numbersMatch = string.gmatch(line, "(%d+)")
  local n1 = tonumber(numbersMatch())
  local n2 = tonumber(numbersMatch())
  local n3 = tonumber(numbersMatch())
  return { n1, n2, n3 }
end
local function isValid(n1, n2, n3)
  if n1 + n2 > n3 and n1 + n3 > n2 and n2 + n3 > n1 then return true else return false end
end

local sum = 0
for r = 1, #lines, 3 do
  local numbers = bla(lines[r])
  local numbers2 = bla(lines[r + 1])
  local numbers3 = bla(lines[r + 2])
  for c = 1, 3 do
    if isValid(numbers[c], numbers2[c], numbers3[c]) then sum = sum + 1 end
  end
end
print(sum)
