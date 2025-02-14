local lib = require 'lib'

local lines = lib.getLines()
local columns = 50
local rows = 6

local screen = {}
for y = 1, rows do
  local row = {}
  for x = 1, columns do
    table.insert(row, false)
  end
  table.insert(screen, row)
end

local function print_(screen)
  for y = 1, rows do
    local row = ""
    for x = 1, columns do
      local char = ' '
      if screen[y][x] then char = '#' end
      row = row .. char
    end
    print(row)
  end
end

local function rect(screen, size_x, size_y)
  for y = 1, size_y do
    for x = 1, size_x do
      if screen[y] == nil then print(y) end
      screen[y][x] = true
    end
  end
end

local function rotate_column(screen, column, offset)
  local offset = offset % rows
  local rows_on = {}
  for row = 1, rows do
    if screen[row][column] then table.insert(rows_on, row) end
  end
  local new_rows_on = {}
  for row = 1, #rows_on do
    local new_row = rows_on[row] + offset
    if new_row > rows then new_row = new_row - rows end
    table.insert(new_rows_on, new_row)
  end

  for i = 1, rows do
    screen[i][column] = false
  end
  for i, new_row in ipairs(new_rows_on) do
    screen[new_row][column] = true
  end
end

local function rotate_row(screen, row, offset)
  local offset = offset % columns
  local columns_on = {}
  for column = 1, columns do
    if screen[row][column] then table.insert(columns_on, column) end
  end
  local new_columns_on = {}
  for column = 1, #columns_on do
    local new_column = columns_on[column] + offset
    if new_column > columns then new_column = new_column - columns end
    table.insert(new_columns_on, new_column)
  end

  for i = 1, columns do
    screen[row][i] = false
  end
  for i, new_column in ipairs(new_columns_on) do
    screen[row][new_column] = true
  end
end

for i = 1, #lines do
  local rect_match = string.match(lines[i], "rect ")
  if rect_match then
    local match = string.gmatch(lines[i], "%d+")
    local x = match()
    local y = match()
    rect(screen, x, y)
  elseif string.match(lines[i], "rotate column") then
    local match = string.gmatch(lines[i], "%d+")
    local x = match()
    local offset = match()
    rotate_column(screen, x + 1, offset)
  elseif string.match(lines[i], "rotate row") then
    local match = string.gmatch(lines[i], "%d+")
    local y = match()
    local offset = match()
    rotate_row(screen, y + 1, offset)
  end
end

local sum = 0
for y = 1, rows do
  local row = {}
  for x = 1, columns do
    if screen[y][x] then sum = sum + 1 end
  end
end
print(sum)

print_(screen)
