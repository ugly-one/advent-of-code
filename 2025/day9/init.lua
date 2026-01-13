local lib = require('lib')
local input = lib.getLines()
input = lib.getLines('test.txt')

local BUCKET_SIZE = 6710886
-- local BUCKET_SIZE = 2

local get_direction = function(point1, point2)
  local direction = nil
  if point1.x == point2.x then
    if point1.y - point2.y > 0 then
      direction = "D"
    else
      direction = "U"
    end
  else
    if point1.x - point2.x > 0 then
      direction = "R"
    else
      direction = "L"
    end
  end
  return direction
end

local get_turn = function(direction, last_direction)
  local turn
  if direction == "R" and last_direction == 'U' then
    turn = 'R'
  elseif direction == 'R' and last_direction == 'D' then
    turn = 'L'
  elseif direction == 'L' and last_direction == 'U' then
    turn = 'L'
  elseif direction == 'L' and last_direction == 'D' then
    turn = 'R'
  elseif direction == "D" and last_direction == 'L' then
    turn = 'L'
  elseif direction == 'D' and last_direction == 'R' then
    turn = 'R'
  elseif direction == 'U' and last_direction == 'L' then
    turn = 'R'
  elseif direction == 'U' and last_direction == 'R' then
    turn = 'L'
  else
    error('?????')
  end

  return turn
end

-- local test = {}
-- local counter = 1
-- while counter <= BUCKET_SIZE do
--   test[counter] = "."
--   counter = counter + 1
-- end

-- TODO, split the points hashtable to have max 167108865 items
local min_x
local min_y
local max_x = 0
local max_y = 0

local corners_array = {}
local last_point = nil
local lines = {}
local directions = {}
local last_direction = nil
local turns = {}
local left_turns = 0
local right_turns = 0
local corners_map = {}

local count_turn = function(turn)
  if turn == 'L' then
    left_turns = left_turns + 1
  else
    right_turns = right_turns + 1
  end
end
for _, line in ipairs(input) do
  local x_string, y_string = string.match(line, '(%d+),(%d+)')
  local x = assert(tonumber(x_string))
  local y = assert(tonumber(y_string))
  local point = { x = x, y = y }
  table.insert(corners_array, point)
  if min_x == nil or point.x < min_x then min_x = point.x end
  if point.x > max_x then max_x = point.x end
  if min_y == nil or point.y < min_y then min_y = point.y end
  if point.y > max_y then max_y = point.y end
  corners_map[point.x .. ',' .. point.y] = 'R'

  if last_point ~= nil then
    local direction = get_direction(point, last_point)
    table.insert(directions, direction)

    local _line = { _start = last_point, _end = point, direction = direction }
    table.insert(lines, _line)

    if last_direction ~= nil then
      local turn = get_turn(direction, last_direction)
      count_turn(turn)
      table.insert(turns, turn)
    end
    last_direction = direction
  end
  last_point = { x = x, y = y }
end


local direction = get_direction(corners_array[1], last_point)
table.insert(directions, direction)

local _line = { _start = last_point, _end = lines[1]._start, direction = direction }
table.insert(lines, _line)

local turn = get_turn(direction, last_direction)
count_turn(turn)
table.insert(turns, turn)

local last_turn = get_turn(directions[1], direction)
count_turn(last_turn)
table.insert(turns, last_turn)

local side = nil
if left_turns + 4 == right_turns then
  side = 'right'
end

if right_turns + 4 == left_turns then
  side = 'left'
end

local current_bucket = {}
local buckets = {}
table.insert(buckets, current_bucket)
-- this structure may not be necessary... I thought this table of points gets too big, but I think Im wrong... :D
local points = { current_bucket_size = 0, buckets = buckets, current_bucket = current_bucket }

local increase_bucket_count = function()
  points.current_bucket_size = points.current_bucket_size + 1
  if points.current_bucket_size == BUCKET_SIZE then
    points.current_bucket_size = 0
    local new_bucket = {}
    points.current_bucket = new_bucket
    table.insert(points.buckets, new_bucket)
  end
end

local try_get_value = function(x, y)
  local point = nil
  for _, bucket in ipairs(points.buckets) do
    point = bucket[x .. ',' .. y]
    if point ~= nil then
      break
    end
  end
  return point
end

local mark_lines = function()
  for _, line in ipairs(lines) do
    if line.direction == 'R' then
      for x = line._start.x, line._end.x do
        points.current_bucket[x .. ',' .. line._start.y] = 'L'
        increase_bucket_count()
      end
    end
    if line.direction == 'L' then
      for x = line._end.x, line._start.x do
        points.current_bucket[x .. ',' .. line._start.y] = 'L'
        increase_bucket_count()
      end
    end

    if line.direction == 'U' then
      for y = line._end.y, line._start.y do
        points.current_bucket[line._start.x .. ',' .. y] = 'L'
        increase_bucket_count()
      end
    end
    if line.direction == 'D' then
      for y = line._start.y, line._end.y do
        points.current_bucket[line._start.x .. ',' .. y] = 'L'
        increase_bucket_count()
      end
    end
  end
end

local function log_to_file(msg)
  local f = io.open("debug_log.txt", "a")
  f:write(msg .. "\n")
  f:close()
end

local mark_inside = function()
  for _, line in ipairs(lines) do
    if line.direction == 'L' then
      local x = line._end.x + 1
      log_to_file("x: " .. x .. " end: " .. line._start.x - 1)
      while x <= line._start.x - 1 do
        -- for x = line._end.x + 1, line._start.x - 1 do
        local dy = nil
        if side == 'right' then
          dy = -1
        else
          dy = 1
        end
        local y = line._end.y + dy
        local point = try_get_value(x, y)
        while point == nil do
          points.current_bucket[x .. ',' .. y] = 'G'
          increase_bucket_count()
          y = y + dy
          point = try_get_value(x, y)
        end
        x = x + 1
      end
    end
    if line.direction == 'R' then
      local x = line._start.x + 1
      log_to_file("x: " .. x .. " end: " .. line._start.x - 1)
      while x <= line._end.x - 1 do
        -- for x = line._start.x + 1, line._end.x - 1 do
        local dy = nil
        if side == 'right' then
          dy = 1
        else
          dy = -1
        end
        local y = line._end.y + dy
        local point = try_get_value(x, y)
        while point == nil do
          points.current_bucket[x .. ',' .. y] = 'G'
          increase_bucket_count()
          y = y + dy
          point = try_get_value(x, y)
        end
        x = x + 1
      end
    end
  end
end

local to_map = function()
  local map = {}
  for y = min_y - 1, max_y + 1 do
    local line = ''
    for x = min_x - 1, max_x + 1 do
      local char = '.'
      local point = try_get_value(x, y)

      if point ~= nil then
        char = point
      end

      line = line .. char
    end
    table.insert(map, line)
  end
  return map
end

-- mark_lines()
-- mark_inside()
-- lib.print()

local function bla(x, y)
  -- print(x, y)
  for _, line in ipairs(lines) do
    if line.direction == 'R' or line.direction == 'L' then
      if y == line._start.y then
        local min_x = line._start.x
        local max_x = line._end.x
        if direction == 'L' then
          min_x = line._end.x
          max_x = line._start.x
        end
        if x >= min_x and x <= max_x then return true end
      end
    end

    if line.direction == 'U' or line.direction == 'D' then
      if x == line._start.x then
        local min_y = line._start.y
        local max_y = line._end.y
        if direction == 'U' then
          min_y = line._end.y
          max_y = line._start.y
        end
        if y >= min_y and y <= max_y then return true end
      end
    end
  end

  return false
end


local function is_bad(top_left_x, top_left_y, bottom_right_x, bottom_right_y)
  for x = top_left_x, bottom_right_x do
    for y = top_left_y, bottom_right_y do
      -- check if the point is one a line - then it is good
      -- check if the point is inside - then it is good
    end
  end
  return false
end


local part2 = function()
  local max_area = 0
  for i = 1, #corners_array do
    local corner1 = corners_array[i]
    for j = i + 1, #corners_array - 1 do
      local corner2 = corners_array[j]

      local min_x
      local max_x
      local min_y
      local max_y
      if corner1.x < corner2.x then
        min_x = corner1.x
        max_x = corner2.x
      else
        min_x = corner2.x
        max_x = corner1.x
      end
      if corner1.y < corner2.y then
        min_y = corner1.y
        max_y = corner2.y
      else
        min_y = corner2.y
        max_y = corner1.y
      end

      local area = math.abs(corner1.x - corner2.x + 1) * math.abs(corner1.y - corner2.y + 1)

      if max_area == nil or area > max_area then
        local bad = is_bad(min_x, min_y, max_x, max_y)

        if not bad then
          max_area = area
        end
      end
      -- print(min_x, max_x, min_y, max_y)
    end
  end
  local result = max_area

  print('result:', result)
  vim.fn.setreg('+', result)
end

part2()

local part1 = function()
  local max_area = 0
  for i = 1, #corners_array do
    local corner1 = corners_array[i]
    for j = i + 1, #corners_array - 1 do
      local corner2 = corners_array[j]
      local area = math.abs(corner1.x - corner2.x + 1) * math.abs(corner1.y - corner2.y + 1)

      if max_area == nil or area > max_area then
        max_area = area
      end
    end
  end
  local result = max_area

  print('result:', result)
  vim.fn.setreg('+', result)
end
