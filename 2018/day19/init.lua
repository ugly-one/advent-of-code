local lib = require('lib')
local input = lib.getLines('input.txt')

local program = {}
local register_for_ip
for _, line in ipairs(input) do
  if string.sub(line, 1, 3) == '#ip' then
    local nr = string.match(line, "(%d+)")
    register_for_ip = tonumber(nr)
  else
    local op, arg1, arg2, arg3 = string.match(line, "(%a+) (%d+) (%d+) (%d+)")
    table.insert(program, { op = op, a = tonumber(arg1), b = tonumber(arg2), c = tonumber(arg3) })
  end
end
local registers = {}
registers[0] = 1
registers[1] = 0
registers[2] = 0
registers[3] = 0
registers[4] = 0
registers[5] = 0

local ip = 0

local function log(registers, ip)
  local log = ""
  for key, value in pairs(registers) do
    log = log .. ' [ ' .. key .. ' ] ' .. value
  end
  print(ip .. ': ' .. log)
end
while true do
  local line = program[ip + 1]
  registers[register_for_ip] = ip
  if line.op == 'seti' then
    registers[line.c] = line.a
  elseif line.op == 'setr' then
    registers[line.c] = registers[line.a]
  elseif line.op == 'addr' then
    registers[line.c] = registers[line.a] + registers[line.b]
  elseif line.op == 'addi' then
    registers[line.c] = registers[line.a] + line.b
  elseif line.op == 'mulr' then
    registers[line.c] = registers[line.a] * registers[line.b]
  elseif line.op == 'muli' then
    registers[line.c] = registers[line.a] * line.b
  elseif line.op == 'banr' then
    registers[line.c] = bit.band(registers[line.a], registers[line.b])
  elseif line.op == 'bani' then
    registers[line.c] = bit.band(registers[line.a], line.b)
  elseif line.op == 'borr' then
    registers[line.c] = bit.bor(registers[line.a], registers[line.b])
  elseif line.op == 'bori' then
    registers[line.c] = bit.bor(registers[line.a], line.b)
  elseif line.op == 'grir' then
    if line.a > registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == 'gtri' then
    if registers[line.a] > line.b then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == 'gtrr' then
    if registers[line.a] > registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == 'eqir' then
    if line.a == registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == 'eqri' then
    if registers[line.a] == line.b then registers[line.c] = 1 else registers[line.c] = 0 end
  elseif line.op == 'eqrr' then
    if registers[line.a] == registers[line.b] then registers[line.c] = 1 else registers[line.c] = 0 end
  end
  ip = registers[register_for_ip]
  ip = ip + 1
  if ip >= #program then break end
  -- log(registers, ip)
  --
end
print(registers[0])
