local line = require('lib').getLines()[1]

local programlib = require('2019.day5.program')
local memory = {}
for a in string.gmatch(line, "([-0-9]+)") do
  table.insert(memory, tonumber(a))
end

local function generate(available, result, settings)
  if #available == 0 then
    table.insert(settings, result)
  else
    for i = 1, #available do
      local result_copy = require 'lib'.copy_table(result)
      table.insert(result_copy, available[i])
      local available_copy = require 'lib'.copy_table(available)
      table.remove(available_copy, i)
      generate(available_copy, result_copy, settings)
    end
  end
end

local function execute(settings)
  local last_output = 0
  local use_last_output = false

  local current_setting = 1
  local function input()
    if use_last_output then
      use_last_output = not use_last_output
      return last_output
    else
      current_setting = current_setting + 1
      use_last_output = not use_last_output
      return settings[current_setting - 1]
    end
  end

  for i = 1, 5 do
    local program = programlib.init(memory)
    last_output = programlib.exec(program, input())
  end

  return last_output
end

local function run1()
  local settings = {}

  generate({ 0, 1, 2, 3, 4 }, {}, settings)

  local max = 0
  for _, setting in ipairs(settings) do
    local result = execute(setting)
    if result > max then max = result end
  end

  print(max)
end

local function run_with_feedback(setting)
  local output = {
  }
  local programs = {
    programlib.init(memory),
    programlib.init(memory),
    programlib.init(memory),
    programlib.init(memory),
    programlib.init(memory)
  }

  for i = 1, 5 do
    programlib.exec(programs[i], setting[i])
  end

  _ = programlib.exec(programs[1], 0)
  output[1] = programlib.exec(programs[1])
  local current_program = 2
  local previous_program = 1
  local last_output = 0

  while true do
    _ = programlib.exec(programs[current_program], output[previous_program])
    output[current_program] = programlib.exec(programs[current_program])
    if current_program == 5 and not (output[current_program] == nil) then last_output = output[current_program] end
    if current_program == 5 and output[5] == nil then break end
    previous_program = current_program
    current_program = current_program + 1
    if current_program > 5 then
      current_program = 1
    end
  end
  return last_output
end


local function run2()
  local settings = {}
  generate({ 5, 6, 7, 8, 9 }, {}, settings)

  local max = 0
  for _, setting in ipairs(settings) do
    local last_output = run_with_feedback(setting)
    if last_output > max then max = last_output end
  end

  print(max)
end

run2()
