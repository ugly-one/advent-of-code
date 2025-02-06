local function bla(line)
  return math.floor(line / 3) - 2
end

local function bla2(line)
  local sum = 0
  local part = bla(line)
  while part > 0 do
    sum = sum + part
    part = bla(part)
  end
  return sum
end

local file = assert(io.open("/home/tgn/Code/advent-of-code/2019/day1/input.txt", "r"))
local line = file:read("l")

local sum = 0
while line do
  sum = sum + bla2(line)
  line = file:read("l")
end

print(sum)
