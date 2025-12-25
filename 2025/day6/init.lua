local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local result = 0

local problems = {}


for _, line in ipairs(input) do
  local problem_nr = 1
  for nr in string.gmatch(line, "(%d+)") do
    if problems[problem_nr] == nil then table.insert(problems, {}) end
    table.insert(problems[problem_nr], tonumber(nr))
    problem_nr = problem_nr + 1
  end
  for sign in string.gmatch(line, "(%p)") do
    local sum = 0

    local func_plus = function(b)
      sum = sum + b
    end

    local func_multiply = function(b)
      if sum == 0 then sum = 1 end
      sum = sum * b
    end

    for _, nr in ipairs(problems[problem_nr]) do
      if sign == '*' then
        func_multiply(nr)
      else
        func_plus(nr)
      end
    end

    result = result + sum
    problem_nr = problem_nr + 1
  end
end

print('result:', result)
vim.fn.setreg('+', result)

local signs = {}
local signs_line_index = 0
local numbers_table = {}

for index, line in ipairs(input) do
  local line_table = lib.to_table(line)
  signs_line_index = index
  for i = #line, 1, -1 do
    if line_table[i] == '+' then table.insert(signs, i, { sign = "+", index = i }) end
    if line_table[i] == '*' then table.insert(signs, i, { sign = "*", index = i }) end
  end
end

for index, line in ipairs(input) do
  if index == signs_line_index then
  else
    local line_table = lib.to_table(line)
    table.insert(numbers_table, line_table)
  end
end

local last_index = #numbers_table[1]

local result = 0
local skip_column = false
local numbers = {}
for column = last_index, 1, -1 do
  if skip_column == false then
    local number_string = ''
    for row = 1, #numbers_table do
      number_string = number_string .. numbers_table[row][column]
    end

    local number = tonumber(number_string)
    table.insert(numbers, number)

    if signs[column] ~= nil then
      -- sign column
      skip_column = true

      -- if empty column
      local sum = 0

      local func_plus = function(b)
        sum = sum + b
      end

      local func_multiply = function(b)
        if sum == 0 then sum = 1 end
        sum = sum * b
      end

      local f = func_plus
      if signs[column].sign == "*" then f = func_multiply end

      -- vim.print(numbers)
      for _, nr in ipairs(numbers) do
        -- vim.print(nr)
        f(nr)
      end
      -- vim.print(sum)

      result = result + sum

      numbers = {}
    else
      skip_column = false
    end
  else
    skip_column = false
  end
end

vim.print(result)
vim.fn.setreg('+', result)
