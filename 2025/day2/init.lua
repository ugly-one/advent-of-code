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

local is_invalid_2 = function(id)
  local id_string = tostring(id)
  local length = string.len(id_string)
  local string_table = lib.to_table(id_string) -- Im iterating over each character inside to_table - may as well do that here. Now I iterate twice

  local is_invalid = function(chunks_count)
    local chunks_size = length / chunks_count
    for index_within_chunk = 1, chunks_size, 1 do
      local char = string_table[index_within_chunk]
      -- start at chunk nr 2 as chunk nr is accessed above
      for chunk_id = 2, chunks_count, 1 do
        if char ~= string_table[chunks_size * (chunk_id - 1) + index_within_chunk] then
          return false
        end
      end
    end
    return true
  end

  for chunks_count = length, 2, -1 do
    -- if it is possible to divide entire string into equal chunks
    if length % chunks_count == 0 then
      if is_invalid(chunks_count) then return true end
    end
  end
  return false
end


print(is_invalid_2(12345678))
print(is_invalid_2(11))
print(is_invalid_2(1111))
print(is_invalid_2(121212))
print(is_invalid_2(1212122))
print(is_invalid_2(121213))

local sum = 0
for first_id_string, last_id_string in string.gmatch(input, "(%d+)-(%d+)") do
  local first_id = tonumber(first_id_string)
  local last_id = tonumber(last_id_string)

  for id = first_id, last_id, 1 do
    if is_invalid_2(id) then
      sum = sum + id
    end
  end
end

print(sum)

vim.fn.setreg('+', sum)
