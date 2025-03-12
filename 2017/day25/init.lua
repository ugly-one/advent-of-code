local lib = require('lib')
local input = lib.getLines()

local state = 'A'
local pos = 0
local tape = {}
tape[pos] = nil

local counter = 0
for step = 1, 12656374 do
  if state == 'A' then
    if tape[pos] == nil then
      tape[pos] = 1
      counter = counter + 1
      pos = pos + 1
      state = 'B'
    else
      tape[pos] = nil
      counter = counter - 1
      pos = pos - 1
      state = 'C'
    end
  elseif state == 'B' then
    if tape[pos] == nil then
      tape[pos] = 1
      counter = counter + 1
      pos = pos - 1
      state = 'A'
    else
      pos = pos - 1
      state = 'D'
    end
  elseif state == 'C' then
    if tape[pos] == nil then
      tape[pos] = 1
      counter = counter + 1
      pos = pos + 1
      state = 'D'
    else
      tape[pos] = nil
      counter = counter - 1
      pos = pos + 1
      state = 'C'
    end
  elseif state == 'D' then
    if tape[pos] == nil then
      pos = pos - 1
      state = 'B'
    else
      tape[pos] = nil
      counter = counter - 1
      pos = pos + 1
      state = 'E'
    end
  elseif state == 'E' then
    if tape[pos] == nil then
      tape[pos] = 1
      counter = counter + 1
      pos = pos + 1
      state = 'C'
    else
      pos = pos - 1
      state = 'F'
    end
  elseif state == 'F' then
    if tape[pos] == nil then
      tape[pos] = 1
      counter = counter + 1
      pos = pos - 1
      state = 'E'
    else
      pos = pos + 1
      state = 'A'
    end
  end
end
print(counter)
