local function isValid(number)
  local stringValue = string.format("%d", number)
  local previousDigit = -1
  local counter = 0
  local double = false
  for i = 1, #stringValue do
    local digit = tonumber(string.sub(stringValue, i, i))
    if previousDigit == -1 then
    else
      if digit < previousDigit then
        return false
      end
    end

    if previousDigit == digit then
      if counter == 0 then
        counter = 2
      else
        counter = counter + 1
      end
    else
      if counter == 2 then
        double = true
      end
      counter = 0
    end
    previousDigit = digit
  end

  if counter == 2 then
    return true
  else
    return double
  end
end

print(isValid(112233))
print(isValid(123444))
print(isValid(111122))

local sum = 0

for i = 134564, 585159 do
  if isValid(i) then sum = sum + 1 end
end

require('lib').print({ " " .. sum })
