local file = assert(io.open("./day1/input.txt", "r"))
local line = file:read("l")
local numbers = {}
while line do
  table.insert(numbers, tonumber(line))
  line = file:read("l")
end

local expectedSum = 2020
local function bla()
  for i, number in ipairs(numbers) do
    for j = i + 1, #numbers do
      for x = j + 1, #numbers do
        if number + numbers[j] + numbers[x] == expectedSum then
          return number * numbers[j] * numbers[x]
        end
      end
    end
  end
end

print(bla())
