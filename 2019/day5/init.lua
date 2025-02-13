local line = require('lib').getLines()[1]
local numbers = {}
for a in string.gmatch(line, "([-0-9]+)") do
  table.insert(numbers, tonumber(a))
end
local programlib = require('2019.day5.program')
local program = programlib.init(numbers)
programlib.exec(program, 5)
local result = programlib.exec(program)
print(result)
