local lib = require 'lib'
local lines = lib.getLines()
local instructions = {}
for i = 1, #lines do
  local inc = string.match(lines[i], 'inc (%a)')
  if inc then
    table.insert(instructions, { type = 'inc', dest = inc })
  end
  local dec = string.match(lines[i], 'dec (%a)')
  if dec then
    table.insert(instructions, { type = 'dec', dest = dec })
  end
  local value, dest = string.match(lines[i], 'cpy (%-?%d+) (%a)')
  if value then
    table.insert(instructions, { type = 'cpy', dest = dest, value = tonumber(value) })
  end
  local source, dest = string.match(lines[i], 'cpy (%a) (%a)')
  if source then
    table.insert(instructions, { type = 'cpy', dest = dest, reg = source })
  end
  local value, distance = string.match(lines[i], 'jnz (%a) (%-?%d+)')
  if value then
    print(lines[i], value, distance)
    table.insert(instructions, { type = 'jnz', distance = tonumber(distance), reg = value })
  end

  local value, distance = string.match(lines[i], 'jnz ([%-?%d+]) ([%-?%d+])')
  if value then
    table.insert(instructions, { type = 'jnz', distance = tonumber(distance), value = tonumber(value) })
  end
end

local reg = {}
reg['a'] = 0
reg['b'] = 0
reg['c'] = 1
reg['d'] = 0
local position = 1
while true do
  local ins = instructions[position]
  if ins.type == 'inc' then
    reg[ins.dest] = reg[ins.dest] + 1
    position = position + 1
  end
  if ins.type == 'dec' then
    reg[ins.dest] = reg[ins.dest] - 1
    position = position + 1
  end
  if ins.type == 'cpy' then
    if ins.value then
      reg[ins.dest] = ins.value
    end
    if ins.reg then
      reg[ins.dest] = reg[ins.reg]
    end
    position = position + 1
  end
  if ins.type == 'jnz' then
    if ins.value then
      print('jnz by value')
      if ins.value ~= 0 then
        position = position + ins.distance
      else
        position = position + 1
      end
    end
    if ins.reg then
      if reg[ins.reg] ~= 0 then
        position = position + ins.distance
      else
        position = position + 1
      end
    end
  end
  if position > #instructions then break end
end
print(reg['a'])
