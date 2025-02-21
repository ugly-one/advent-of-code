local lib = require 'lib'
local lines = lib.getLines()

local function get_state(line)
  local initial_state = lib.to_table(line)
  for i = 1, #initial_state do
    initial_state[i] = tonumber(initial_state[i])
  end
  return initial_state
end
local function modify_dragon(input, desired_length)
  local size = #input
  if size == desired_length then
    return input
  elseif size > desired_length then
    local result = {}
    for i = 1, desired_length do
      result[i] = input[i]
    end
    return result
  end
  local copy = {}
  for i = 1, size do
    copy[i] = input[size + 1 - i]
  end
  for i = 1, size do
    if copy[i] == 1 then copy[i] = 0 else copy[i] = 1 end
  end
  local result = {}
  for i = 1, size do
    result[i] = input[i]
  end
  result[size + 1] = 0
  for i = 1, size do
    result[size + 1 + i] = copy[i]
  end

  return modify_dragon(result, desired_length)
end

local function to_hash(input)
  local size = #input
  if size % 2 == 1 then
    return input
  end

  local result = {}
  local counter = 1
  for i = 1, size - 1, 2 do
    if input[i] == input[i + 1] then result[counter] = 1 else result[counter] = 0 end
    counter = counter + 1
  end
  return to_hash(result)
end


local disk = get_state(lines[1])
local randomized = modify_dragon(disk, 35651584)
local result = to_hash(randomized)
vim.print(result)
