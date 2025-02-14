local lib = require 'lib'
local lines = lib.getLines()

local function decompress(line)
  local line_array = lib.to_table(line)
  local result = ""
  local reading_instruction = false
  local reading_second = false
  local skip_count = 0
  local characters_count_string = ""
  local times_string = ""
  for i = 1, #line_array do
    if skip_count > 0 then
      skip_count = skip_count - 1
    else
      local char = line_array[i]
      if char == '(' then
        reading_instruction = true
      elseif char == ')' and reading_instruction then
        local characters_count = tonumber(characters_count_string)
        local times = tonumber(times_string)
        local characters = ""
        for c = 1, characters_count do
          characters = characters .. line_array[i + c]
        end
        for t = 1, times do
          result = result .. characters
        end
        reading_instruction = false
        reading_second = false
        characters_count_string = ""
        times_string = ""
        skip_count = characters_count
      elseif reading_instruction == true then
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
        result = result .. char
      end
    end
  end
  return #result
end

for i = 1, #lines do
  print(decompress(lines[i]))
end
