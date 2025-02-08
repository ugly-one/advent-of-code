local lib = require('lib')
local lines = lib.getLines()
local result = ""
local pos = 5
for i, line in ipairs(lines) do
  for i = 1, #line do
    local move = string.sub(line, i, i)
    if pos == 1 and move == 'D' then
      pos = 3
    elseif pos == 13 and move == 'U' then
      pos = 11
    elseif (pos == 2 or pos == 3 or pos == 5 or pos == 6 or pos == 7 or pos == 8 or pos == 10 or pos == 11) and move == 'R' then
      pos = pos + 1
    elseif (pos == 3 or pos == 4 or pos == 6 or pos == 7 or pos == 8 or pos == 9 or pos == 11 or pos == 12) and move == 'L' then
      pos = pos - 1
    elseif (pos == 2 or pos == 4 or pos == 3 or pos == 6 or pos == 7 or pos == 8) and move == 'D' then
      pos = pos + 4
    elseif (pos == 3) and move == 'U' then
      pos = 1
    elseif (pos == 10 or pos == 11 or pos == 12 or pos == 6 or pos == 7 or pos == 8) and move == 'U' then
      pos = pos - 4
    elseif (pos == 11) and move == 'D' then
      pos = 13
    end
  end
  local toLog = pos
  if pos == 10 then
    toLog = 'A'
  elseif pos == 11 then
    toLog = 'B'
  elseif pos == 12 then
    toLog = 'C'
  elseif pos == 13 then
    toLog = 'D'
  end
  result = result .. toLog
end

print(result)
