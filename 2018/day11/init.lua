local lib = require('lib')
local input = tonumber(lib.getLines()[1])
local function calculate(x, y)
  local rackId = x + 10
  local power = rackId * y + input
  power = power * rackId

  if power > 99 then
    local a = tostring(power)
    local index = #a - 2
    power = tonumber(string.sub(a, index, index)) - 5
  else
    power = -5
  end
  return power
end

local grid_size = 300
local grid = {}
for y = 1, grid_size do
  local row = {}
  local line = ""
  for x = 1, grid_size do
    row[x] = calculate(x, y)
    line = line .. '  ' .. row[x]
  end
  table.insert(grid, row)
end

-- [SLOW] not horribly slow, finished within 2 minutes.
local cache = {}
local max = nil
local max_point = nil
local result_size = nil
for size = 1, grid_size do
  print(size)
  for y = 1, grid_size - size + 1 do
    for x = 1, grid_size - size + 1 do
      -- check if it's possible to beat the max
      local sum = 0
      local hash = x .. '.' .. y .. '.' .. (size - 1)
      if cache[hash] then
        -- we have a size of a square that starts at the same
        -- position but one row/column smaller
        sum = cache[hash]
        -- add the last row
        for i = 1, size do
          sum = sum + grid[y + size - 1][x + i - 1]
        end
        -- add the last column
        -- we don't need the last item as it was already calculated when taking the last row
        for i = 1, size - 1 do
          sum = sum + grid[y + i - 1][x + size - 1]
        end
      else
        for i_y = 0, size - 1 do
          for i_x = 0, size - 1 do
            sum = sum + grid[y + i_y][x + i_x]
          end
        end
      end

      hash = x .. '.' .. y .. '.' .. size
      cache[hash] = sum
      if max == nil or sum > max then
        print('new max', sum)
        max = sum
        max_point = { x = x, y = y }
        result_size = size
      end
    end
  end
end

print(max_point.x, max_point.y, result_size, 'total', max)
