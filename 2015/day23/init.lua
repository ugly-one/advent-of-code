local lib = require('lib')
local lines = lib.getLines()

local instructions = {}
for i = 1, #lines do
  local arg1 = string.match(lines[i], 'inc (%a)')
  if arg1 then
    table.insert(instructions, { type = 'inc', arg1 = arg1 })
  end
  local arg1 = string.match(lines[i], 'tpl (%a)')
  if arg1 then
    table.insert(instructions, { type = 'tpl', arg1 = arg1 })
  end

  local arg1 = string.match(lines[i], 'hlf (%a)')
  if arg1 then
    table.insert(instructions, { type = 'hlf', arg1 = arg1 })
  end
  local arg1, arg2 = string.match(lines[i], 'jmp (%S+)')
  if arg1 then
    table.insert(instructions, { type = 'jmp', arg1 = tonumber(arg1) })
  end
  local arg1, arg2 = string.match(lines[i], 'jie (%a), (%S+)')
  if arg1 then
    table.insert(instructions, { type = 'jie', arg1 = arg1, arg2 = tonumber(arg2) })
  end
  local arg1, arg2 = string.match(lines[i], 'jio (%a), (%S+)')
  if arg1 then
    table.insert(instructions, { type = 'jio', arg1 = arg1, arg2 = tonumber(arg2) })
  end
end

local function execute()
  local reg = {}
  reg['a'] = 1
  reg['b'] = 0
  local position = 1
  while true do
    local ins = instructions[position]
    print(ins.type, ins.arg1, ins.arg2)
    if ins.type == 'inc' then
      reg[ins.arg1] = reg[ins.arg1] + 1
      position = position + 1
    end
    if ins.type == 'hlf' then
      reg[ins.arg1] = reg[ins.arg1] / 2
      position = position + 1
    end
    if ins.type == 'tpl' then
      reg[ins.arg1] = reg[ins.arg1] * 3
      position = position + 1
    end
    if ins.type == 'jmp' then
      position = position + ins.arg1
    end
    if ins.type == 'jie' then
      if reg[ins.arg1] % 2 == 0 then position = position + ins.arg2 else position = position + 1 end
    end
    if ins.type == 'jio' then
      if reg[ins.arg1] == 1 then position = position + ins.arg2 else position = position + 1 end
    end

    if position > #instructions then break end
  end
  return reg['b']
end
print(execute())
