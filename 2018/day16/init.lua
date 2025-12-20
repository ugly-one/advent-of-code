local lib = require('lib')
local input = lib.getLines('bla.txt')

local program = {}
for _, line in ipairs(input) do
  if string.sub(line, 1, 3) == '#ip' then
  else
    local op, arg1, arg2, arg3 = string.match(line, "(%d+) (%d) (%d) (%d)")
    table.insert(program, { op = op, a = tonumber(arg1), b = tonumber(arg2), c = tonumber(arg3) })
  end
end
local registers = {}
registers[0] = 0
registers[1] = 0
registers[2] = 0
registers[3] = 0
registers[4] = 0
registers[5] = 0
local ip = 0

ip = 1
while true do
  local line = program[ip]
  if line.op == '2' then
    registers[line.c] = line.a
  elseif line.op == '8' then
    registers[line.c] = registers[line.a]
  elseif line.op == '9' then
    registers[line.c] = registers[line.a] + registers[line.b]
  elseif line.op == '11' then
    registers[line.c] = registers[line.a] + line.b
  elseif line.op == '15' then
    registers[line.c] = registers[line.a] * registers[line.b]
  elseif line.op == '7' then
    registers[line.c] = registers[line.a] * line.b
  elseif line.op == '5' then
    registers[line.c] = bit.band(registers[line.a], registers[line.b])
  elseif line.op == '1' then
    registers[line.c] = bit.band(registers[line.a], line.b)
  elseif line.op == '6' then
    registers[line.c] = bit.bor(registers[line.a], registers[line.b])
  elseif line.op == '3' then
    registers[line.c] = bit.bor(registers[line.a], line.b)
  elseif line.op == '12' then
    if line.a > registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == '14' then
    if registers[line.a] > line.b then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == '13' then
    if registers[line.a] > registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == '4' then
    if line.a == registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == '0' then
    if registers[line.a] == line.b then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == '10' then
    if registers[line.a] == registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  end
  ip = ip + 1
  if ip > #program then break end
end
print(registers[0])
