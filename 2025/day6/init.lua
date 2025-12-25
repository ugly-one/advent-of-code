local lib = require('lib')
local input = lib.getLines()
input = lib.getLines('test.txt')

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
