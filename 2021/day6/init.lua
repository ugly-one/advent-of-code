local input = require 'lib'.getLines()[1]
--input = '3,4,3,1,2'

local state = {}
local match = string.gmatch(input, "%d")
for m in match do
  m = tonumber(m)
  if state[m] then state[m] = state[m] + 1 else state[m] = 1 end
end

for i = 1, 256 do
  local new_state = {}
  for days, count in pairs(state) do
    if days > 0 then
      if new_state[days - 1] then new_state[days - 1] = new_state[days - 1] + count else new_state[days - 1] = count end
    else
      new_state[8] = count
      if new_state[6] then new_state[6] = new_state[6] + count else new_state[6] = count end
    end
  end
  state = new_state
end
local sum = 0
for days, count in pairs(state) do
  sum = sum + count
end
vim.print(sum)
