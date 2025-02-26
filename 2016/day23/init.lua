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
  local arg1 = string.match(lines[i], 'tgl (%a)')
  if arg1 then
    table.insert(instructions, { type = 'tgl', arg1 = arg1 })
  end
end

local function try_multiply(position, registers)
  if position + 4 > #instructions then return false end
  local acc = instructions[position]
  local dec1 = instructions[position + 1]
  local jnz1 = instructions[position + 2]
  local dec2 = instructions[position + 3]
  local jnz2 = instructions[position + 4]

  if acc.type == 'inc' and dec1.type == 'dec' and jnz1.type == 'jnz' and dec1.arg1 == jnz1.arg1 and dec2.type == 'dec' and jnz2.type == 'jnz' and dec2.arg1 == jnz2.arg1 then
    local result = registers[jnz1.arg1] * registers[jnz2.arg1]
    registers[jnz1.arg1] = 0
    registers[jnz2.arg1] = 0
    registers[acc.arg1] = registers[acc.arg1] + result
    return true
  end
  return false
end

local function execute(a)
  local reg = {}
  reg['a'] = a
  reg['b'] = 0
  reg['c'] = 0
  reg['d'] = 0
  local position = 1
  local count = 500
  while count > 0 do
    local multiply = try_multiply(position, reg)
    if multiply then
      position = position + 4
    else
      local ins = instructions[position]
      print('---------------------------------------------------------------')
      print(reg['a'], reg['b'], reg['c'], reg['d'])
      print('instruction nr: ', position, ins.type, ins.arg1, ins.arg2)

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
      if ins.type == 'tgl' then
        if reg[ins.arg1] then
          local distance = reg[ins.arg1]
          local index = position + distance
          if index < 1 or index > #instructions then
          else
            local ins_to_modify = instructions[index]
            if ins_to_modify.type == 'inc' then
              ins_to_modify.type = 'dec'
            elseif ins_to_modify.type == 'dec' then
              ins_to_modify.type = 'inc'
            elseif ins_to_modify.type == 'jnz' then
              ins_to_modify.type = 'cpy'
            elseif ins_to_modify.type == 'cpy' then
              ins_to_modify.type = 'jnz'
            elseif ins_to_modify.type == 'tgl' then
              ins_to_modify.type = 'inc'
            else
              print('????', ins_to_modify.type)
            end
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
    end

    print(reg['a'], reg['b'], reg['c'], reg['d'])
    print('---------------------------------------------------------------')
    if position > #instructions then break end
  end

  print(reg['a'])
end

execute(12)
