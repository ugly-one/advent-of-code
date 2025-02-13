local function add(x, y)
  return x + y
end

local function mul(x, y)
  return x * y
end

local function get_value(mode, numbers, position)
  if mode == 0 then
    return numbers[numbers[position] + 1]
  else
    return numbers[position]
  end
end

local function get_operation_code(instruction_code)
  local instruction_code = tostring(instruction_code)
  local length = #instruction_code
  local operation_code = string.sub(instruction_code, length - 1, length)
  local parameter_modes = { 0, 0, 0 }
  for i = 1, length - 2 do
    parameter_modes[i] = tonumber(string.sub(instruction_code, length - 2 - i + 1, length - 2 - i + 1))
  end
  return { op = tonumber(operation_code), parameters_modes = parameter_modes }
end

local function init(memory)
  local program = {}
  program.current_position = 1
  program.state = 'running'
  program.memory = require 'lib'.copy_table(memory)
  return program
end

local function exec(program, input)
  local numbers = program.memory
  while true do
    local instruction_code = numbers[program.current_position]
    local instruction = get_operation_code(instruction_code)
    local operation_code = instruction.op
    if operation_code == 1 or operation_code == 2 then
      local arg1 = get_value(instruction.parameters_modes[1], numbers, program.current_position + 1)
      local arg2 = get_value(instruction.parameters_modes[2], numbers, program.current_position + 2)
      local action = add
      if operation_code == 1 then
        action = add
      else
        action = mul
      end
      local result = action(arg1, arg2)
      local destination = numbers[program.current_position + 3] + 1
      numbers[destination] = result
      program.current_position = program.current_position + 4
    elseif operation_code == 3 then
      local destination = numbers[program.current_position + 1] + 1
      numbers[destination] = input
      program.current_position = program.current_position + 2
      return
    elseif operation_code == 4 then
      local source = numbers[program.current_position + 1] + 1
      local value = numbers[source]
      program.current_position = program.current_position + 2
      return value
    elseif operation_code == 5 then
      local parameter = get_value(instruction.parameters_modes[1], numbers, program.current_position + 1)
      if not (parameter == 0) then
        program.current_position = get_value(instruction.parameters_modes[2], numbers, program.current_position + 2) + 1
      else
        program.current_position = program.current_position + 3
      end
    elseif operation_code == 6 then
      local parameter = get_value(instruction.parameters_modes[1], numbers, program.current_position + 1)
      if parameter == 0 then
        program.current_position = get_value(instruction.parameters_modes[2], numbers, program.current_position + 2) + 1
      else
        program.current_position = program.current_position + 3
      end
    elseif operation_code == 7 then
      local parameter1 = get_value(instruction.parameters_modes[1], numbers, program.current_position + 1)
      local parameter2 = get_value(instruction.parameters_modes[2], numbers, program.current_position + 2)
      local destination = get_value(1, numbers, program.current_position + 3) + 1
      if parameter1 < parameter2 then
        numbers[destination] = 1
      else
        numbers[destination] = 0
      end
      program.current_position = program.current_position + 4
    elseif operation_code == 8 then
      local parameter1 = get_value(instruction.parameters_modes[1], numbers, program.current_position + 1)
      local parameter2 = get_value(instruction.parameters_modes[2], numbers, program.current_position + 2)
      local destination = get_value(1, numbers, program.current_position + 3) + 1
      if parameter1 == parameter2 then
        numbers[destination] = 1
      else
        numbers[destination] = 0
      end
      program.current_position = program.current_position + 4
    elseif operation_code == 99 then
      program.state = 'stopped'
      return nil
    else
      program.state = 'error'
      return nil
    end
  end
end
local M = {}
M.exec = exec
M.init = init
return M
