local lib = require 'lib'
local lines = lib.getLines()
local instructions = {}
for i = 1, #lines do
  local arg1 = string.match(lines[i], 'inc (%a)')
  if arg1 then
    table.insert(instructions, { type = 'inc', arg1 = arg1 })
  end
  local arg1 = string.match(lines[i], 'dec (%a)')
  if arg1 then
    table.insert(instructions, { type = 'dec', arg1 = arg1 })
  end
  local arg1, arg2 = string.match(lines[i], 'cpy (%S+) (%S+)')
  if arg1 then
    table.insert(instructions, { type = 'cpy', arg1 = arg1, arg2 = arg2 })
  end
  local arg1, arg2 = string.match(lines[i], 'jnz (%S+) (%S+)')
  if arg1 then
    table.insert(instructions, { type = 'jnz', arg1 = arg1, arg2 = arg2 })
  end
end

local reg = {}
reg['a'] = 0
reg['b'] = 0
reg['c'] = 1
reg['d'] = 0
local position = 1
local count = 30
while count > 0 do
  local ins = instructions[position]
  --print('entire code...')
  --for _, inst in ipairs(instructions) do
  --  print(inst.type, inst.arg1, inst.arg2)
  --end
  --print('-----')
  --print(ins.type, ins.arg1, ins.arg2)
  --print(position)
  --print('registers')
  --print(reg['a'])
  --print(reg['b'])
  --print(reg['c'])
  --print(reg['d'])

  if ins.type == 'inc' then
    if reg[ins.arg1] then
      reg[ins.arg1] = reg[ins.arg1] + 1
    end
    position = position + 1
  end
  if ins.type == 'dec' then
    if reg[ins.arg1] then
      reg[ins.arg1] = reg[ins.arg1] - 1
    end
    position = position + 1
  end
  if ins.type == 'cpy' then
    local destination = ins.arg2
    if reg[destination] then
      local n = tonumber(ins.arg1)
      if n then
        reg[destination] = n
      else
        if reg[ins.arg1] then reg[destination] = reg[ins.arg1] end
      end
    end
    position = position + 1
  end
  if ins.type == 'jnz' then
    local distance = nil
    if tonumber(ins.arg2) then
      distance = tonumber(ins.arg2)
    elseif reg[ins.arg2] then
      distance = reg[ins.arg2]
    end

    if distance == nil then
      position = position + 1
    else
      -- we have distance
      local check = tonumber(ins.arg1)
      if check and check ~= 0 then
        position = position + distance
      else
        if reg[ins.arg1] and reg[ins.arg1] ~= 0 then
          position = position + distance
        else
          position = position + 1
        end
      end
    end
  end

  if position > #instructions then break end
end

print(reg['a'])
