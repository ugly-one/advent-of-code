local file = io.open("day7/input.txt", "r")
if file == nil then return 1 end


local function parseLine(line)
  for result, numbers in string.gmatch(line, "(.*):(.*)") do
    local numbersArray = {}
    local count = 0
    for number in string.gmatch(numbers, "[0-9]+") do
      table.insert(numbersArray, tonumber(number))
      count = count + 1
    end
    local expectedResult = tonumber(result)
    return { expectedResult = expectedResult, numbers = numbersArray, count = count }
  end
end

local possibleOperands = { '+', '*', '|' }

local function generateOperands(size)
  local result = {}

  local function generate(current, length)
    if length == size then
      table.insert(result, current)
      return
    end

    for i, operand in pairs(possibleOperands) do
      generate(current .. operand, length + 1)
    end
  end

  generate("", 0)
  return result
end

local function evaluate(numbers, operands)
  local sum = numbers[1]
  for i = 1, string.len(operands) do
    local operand = string.sub(operands, i, i)
    local nextNumber = numbers[i + 1]
    if operand == '+' then
      sum = sum + nextNumber
    elseif operand == '*' then
      sum = sum * nextNumber
    elseif operand == '|' then
      sum = tonumber(sum .. nextNumber)
    end
  end
  return sum
end


local function isValid(line)
  local operatorCount = line.count - 1
  local combinations = generateOperands(operatorCount)

  for i, operands in ipairs(combinations) do
    local evaluation = evaluate(line.numbers, operands)
    if evaluation == line.expectedResult then return true end
  end
  return false
end

local line = file:read("l")

local sum = 0
while line do
  local parsedLine = parseLine(line)
  if isValid(parsedLine) then
    sum = sum + parsedLine.expectedResult
  end
  line = file:read("l")
end

print(string.format("%.0f", sum))
