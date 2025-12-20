local lib = require('lib')
local input = lib.getLines()
input = lib.getLines('test.txt')

local result = 0

local map = {}

local rows = #input
local columns
for row_index = 1, rows, 1 do
  local row = lib.to_table(input[row_index])
  columns = #row
  table.insert(map, row)
end

for row_index = 1, rows, 1 do
  for column_index = 1, columns, 1 do
    if map[row_index][column_index] == '@' then
      local rolls_count = 0

      local inrement = function(x, y)
        if x > 0 and y > 0 and x <= rows and y <= columns then
          if map[y][x] == '@' then rolls_count = rolls_count + 1 end
        end
      end
      local x = column_index
      local y = row_index

      inrement(x-1, y-1)
      inrement(x, y-1)
      inrement(x+1, y-1)

      inrement(x-1, y)
      inrement(x+1, y)

      inrement(x-1, y+1)
      inrement(x, y+1)
      inrement(x+1, y+1)

      if rolls_count < 4 then
        result = result + 1
      end
    end
  end
end

print(result)
vim.fn.setreg('+', result)
