local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local result = 0

local get_largest_joltage = function(line)
  local line_table = lib.to_table(line)
  local first_digit = 0
  local first_digit_index = 0
  local second_digit = 0

  for index = 1, #line_table - 1, 1 do
    local number = tonumber(line_table[index])
    if (number > first_digit) then
      first_digit_index = index
      first_digit = number
    end
  end
  for index = 2, #line_table, 1 do
    if index > first_digit_index then
      local number = tonumber(line_table[index])
      if (number > second_digit) then second_digit = number end
    end
  end

  local largest_joltage = tonumber(first_digit .. second_digit)
  return largest_joltage
end

local get_largest_joltage_part2 = function(line, batteries_count)
  local line_table = lib.to_table(line)

  local digits = ''
  local last_digit_index = 0

  for battery_nr = 1, batteries_count, 1 do
    -- print('battery nr:', battery_nr)
    local digit = 0
    for index = last_digit_index + 1, #line_table - batteries_count + battery_nr, 1 do
      -- print('index:', index)
      local number = tonumber(line_table[index])
      if (number > digit) then
        last_digit_index = index
        digit = number
      end
      -- print(last_digit_index, digit, digits)
    end

    digits = digits .. digit
  end

  return tonumber(digits)
end

-- print(get_largest_joltage_part2('12345', 3))

for _, line in ipairs(input) do
  -- local largest_joltage = get_largest_joltage(line)
  local largest_joltage = get_largest_joltage_part2(line, 12)
  result = result + largest_joltage
end

print('result:', result)
vim.fn.setreg('+', result)
