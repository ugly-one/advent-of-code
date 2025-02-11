local lines = require 'lib'.getLines()

local function get_size(table)
  local sum = 0
  for k, v in pairs(table) do
    sum = sum + 1
  end
  return sum
end

local function filter(table_, value)
  local filtered = {}
  for k, v in pairs(table_) do
    if v == value then
      table.insert(filtered, k)
    end
  end
  return filtered
end
local count = 0
local group_size = 0
local group_answers = {}
for _, line in ipairs(lines) do
  if line == "" then
    count = count + #filter(group_answers, group_size)
    group_answers = {}
    group_size = 0
  else
    for i = 1, #line do
      local char = string.sub(line, i, i)
      if group_answers[char] == nil then group_answers[char] = 1 else group_answers[char] = group_answers[char] + 1 end
    end
    group_size = group_size + 1
  end
end

count = count + #filter(group_answers, group_size)
print(count)
