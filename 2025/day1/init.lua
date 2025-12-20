local lib = require('lib')
local lines = lib.getLines('test.txt')
lines = lib.getLines()

local dial = 50
local counter = 0

for _, a in ipairs(lines) do
  local direction, steps_string = string.match(a, '(%a)(%d+)')

  local steps = tonumber(steps_string)

  local circles = steps / 100
  circles = math.floor(circles)
  counter = circles + counter

  steps = steps % 100

  local dial_before = dial
  if direction == 'L' then
    dial = dial - steps
    if dial == 0 then
      counter = counter + 1
    elseif dial < 0 then
      dial = 100 + dial
      if dial_before ~= 0 then counter = counter + 1 end
    end
  end

  if direction == 'R' then
    dial = dial + steps
    if dial == 0 then
      counter = counter + 1
    elseif dial > 99 then
      dial = dial - 100
      if dial_before ~= 0 then counter = counter + 1 end
    end
  end
end

print(counter)
