local lib = require 'lib'
local input = lib.getLines()[1]
local row = lib.to_table(input)
local rows = 400000
local function get_next(row)
  local result = {}
  local count = 0
  for i = 1, #row do
    local left = ""
    local right = ""
    local middle = row[i]
    if i == 1 then left = "." else left = row[i - 1] end
    if i == #row then right = "." else right = row[i + 1] end
    if left == "^" and middle == "^" and right == "." then
      result[i] = "^"
    elseif right == "^" and middle == "^" and left == "." then
      result[i] = "^"
    elseif right == "^" and middle == "." and left == "." then
      result[i] = "^"
    elseif left == "^" and middle == "." and right == "." then
      result[i] = "^"
    else
      result[i] = "."
      count = count + 1
    end
  end
  return { row = result, count = count }
end
local sum = 0
for _, char in ipairs(row) do
  if char == '.' then sum = sum + 1 end
end

for i = 1, rows - 1 do
  local result = get_next(row)
  row = result.row
  sum = sum + result.count
end
print(sum)
