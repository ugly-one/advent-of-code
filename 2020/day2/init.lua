local file = io.open("day2/input.txt", "r")
local function check(line)
  local a, b, start, end_, letter, input = string.find(line, "(%d+)%-(%d+)%s(%a)[:]%s(%a+)")

  if not letter then print(line) end
  local letter1 = string.sub(input, tonumber(start), tonumber(start))
  local letter2 = string.sub(input, tonumber(end_), tonumber(end_))
  local match = 0
  if letter1 == letter then
    match = 1
  end
  if letter2 == letter then
    match = match + 1
  end

  if match == 1 then return true else return false end
end

local counter = 0
local line = file:read("l")
while line do
  if check(line) then
    counter = counter + 1
  end
  line = file:read("l")
end

print(counter)
