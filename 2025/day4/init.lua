local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local map = lib.to_table_2d(input)
local rows = #map
local columns = #(map[1])

local to_print = function(input)
  local result = {}
  for x = 1, #input do
    local row = ''
    for y = 1, #(input[1]) do
      local char = '.'
      if input[x][y].roll == true then char = '@' end
      row = row .. char
    end
    table.insert(result, row)
  end
  return result
end

local scan = function()
  local count_map = {}
  for row_index = 1, rows, 1 do
    local count_map_row = {}
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

        inrement(x - 1, y - 1)
        inrement(x, y - 1)
        inrement(x + 1, y - 1)

        inrement(x - 1, y)
        inrement(x + 1, y)

        inrement(x - 1, y + 1)
        inrement(x, y + 1)
        inrement(x + 1, y + 1)

        table.insert(count_map_row, { roll = true, count = rolls_count })
      else
        table.insert(count_map_row, { roll = false })
      end
    end
    table.insert(count_map, count_map_row)
  end

  return count_map
end

local move = function(map)
  local rolls_moved = 0
  for row_index = 1, #map, 1 do
    for column_index = 1, #(map[1]), 1 do
      local item = map[row_index][column_index]
      if item.count ~= nil and item.count < 4 then
        item.roll = false
        item.count = nil
        rolls_moved = rolls_moved + 1
        local decrement = function(x, y)
          if x > 0 and y > 0 and x <= rows and y <= columns then
            if map[y][x].roll == true then map[y][x].count = map[y][x].count - 1 end
          end
        end
        local x = column_index
        local y = row_index

        decrement(x - 1, y - 1)
        decrement(x, y - 1)
        decrement(x + 1, y - 1)

        decrement(x - 1, y)
        decrement(x + 1, y)

        decrement(x - 1, y + 1)
        decrement(x, y + 1)
        decrement(x + 1, y + 1)
      end
    end
  end
  return rolls_moved
end

local map = scan()
local something_moved = true
local total_rolls_moved = 0
while something_moved do
  local rolls_moved = move(map)
  total_rolls_moved = total_rolls_moved + rolls_moved
  if rolls_moved == 0 then something_moved = false end
end

print(total_rolls_moved)
vim.fn.setreg('+', total_rolls_moved)

local part1 = function()
  local result = 0
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

        inrement(x - 1, y - 1)
        inrement(x, y - 1)
        inrement(x + 1, y - 1)

        inrement(x - 1, y)
        inrement(x + 1, y)

        inrement(x - 1, y + 1)
        inrement(x, y + 1)
        inrement(x + 1, y + 1)

        if rolls_count < 4 then
          result = result + 1
        end
      end
    end
  end
  return result
end

-- local result = part1()
-- print(result)
-- vim.fn.setreg('+', result)
