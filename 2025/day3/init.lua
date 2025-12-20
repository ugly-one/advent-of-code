local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local result = 0

for _, line in ipairs(input) do
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
  result =result + largest_joltage
end

print('result:', result)
vim.fn.setreg('+', result)
