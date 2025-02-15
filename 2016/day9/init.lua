local lib = require 'lib'
local lines = lib.getLines()

local function decompress(line)
  local line_array = lib.to_table(line)
  local reading_instruction = false
  local reading_second = false
  local characters_count_string = ""
  local times_string = ""
  local counts = {}
  for i = 1, #line_array do
    local char = line_array[i]
    if char == '(' then
      counts[i] = 0
      reading_instruction = true
    elseif char == ')' and reading_instruction then
      counts[i] = 0
      local characters_count = tonumber(characters_count_string)
      local times = tonumber(times_string)
      for c = 1, characters_count do
        if counts[i + c] == nil then
          counts[i + c] = times
        else
          counts[i + c] = counts[i + c] * times
        end
      end
      reading_instruction = false
      reading_second = false
      characters_count_string = ""
      times_string = ""
    elseif reading_instruction == true then
      counts[i] = 0
      if char == 'x' then
        reading_second = true
      else
        if reading_second then
          times_string = times_string .. char
        else
          characters_count_string = characters_count_string ..
              char
        end
      end
    else
      if counts[i] == nil then
        counts[i] = 1
      end
    end
  end

  local sum = 0
  for k, v in pairs(counts) do
    sum = sum + v
  end
  return sum
end

for i = 1, #lines do
  print(decompress(lines[i]))
end
