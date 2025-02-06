local file = io.open("day2/input.txt", "r")
if file == nil then return 1 end
local line = file:read("l")

local numbers = {}
for a in string.gmatch(line, "([0-9]+)") do
  table.insert(numbers, tonumber(a))
end

local function add(x, y)
  return x + y
end

local function mul(x, y)
  return x * y
end

local function bla(numbers, action, position)
  local index1 = numbers[position + 1] + 1
  local index2 = numbers[position + 2] + 1
  local result = action(numbers[index1], numbers[index2])
  local index_to_change = numbers[position + 3] + 1
  numbers[index_to_change] = result
end

local function exec(numbers, noun, verb)
  local current_position = 1
  numbers[2] = noun
  numbers[3] = verb

  while true do
    local op = numbers[current_position]
    if op == 1 then
      bla(numbers, add, current_position)
    elseif op == 2 then
      bla(numbers, mul, current_position)
    elseif op == 99 then
      return numbers[1]
    else
      return -1
    end
    current_position = current_position + 4
  end
end

for n = 0, 100 do
  for v = 0, 100 do
    local new_table = {}
    for i, a in ipairs(numbers) do
      table.insert(new_table, a)
    end
    local result = exec(new_table, n, v)
    if result == 19690720 then
      print(100 * n + v)
      print('sdfsdf')
      return
    end
  end
end
print('aaaa')
