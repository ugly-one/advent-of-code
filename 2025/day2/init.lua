local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')
input = lib.to_string(input)

local is_invalid = function(id)
  local id_string = tostring(id)
  local length = string.len(id_string)

  if length < 2 then return false end
  if (length % 2 ~= 0) then return false end

  local last_index_left_part = math.floor(length / 2)
  local string_table = lib.to_table(id_string) -- Im iterating over each character inside to_table - may as well do that here. Now I iterate twice
  for i = 1, last_index_left_part, 1 do
    if string_table[i] ~= string_table[last_index_left_part + i] then return false end
  end

  return true
end

local sum = 0
for first_id_string, last_id_string in string.gmatch(input, "(%d+)-(%d+)") do
  local first_id = tonumber(first_id_string)
  local last_id = tonumber(last_id_string)

  for id = first_id, last_id, 1 do
    if is_invalid(id) then
      sum = sum + id
    end
  end
end

print(sum)

vim.fn.setreg('+', sum)

