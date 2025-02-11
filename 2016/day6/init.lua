local lines = require 'lib'.getLines()

local map = {}

for _, line in ipairs(lines) do
  for c = 1, #line do
    local char = string.sub(line, c, c)
    if map[c] then
      local counts = map[c]
      if counts[char] then counts[char] = counts[char] + 1 else counts[char] = 1 end
    else
      map[c] = { [char] = 1 }
    end
  end
end

local result = ""
for _, column in pairs(map) do
  local sorted = require 'lib'.sort(column)
  local least = ""
  for _, char in pairs(sorted) do
    least = char.key
  end

  result = result .. least
end
print(result)
