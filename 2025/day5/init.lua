local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

-- vim.print(input)

local fresh_ranges = {}
local available = {}

local processing_fresh = true

for _, line in ipairs(input) do
  if line == '' then
    processing_fresh = false
  else
    if processing_fresh then
      local start, _end = string.match(line, '(%d+)-(%d+)')
      table.insert(fresh_ranges, { start = tonumber(start), _end = tonumber(_end) })
    else
      table.insert(available, tonumber(line))
    end
  end
end

-- vim.print(fresh_ranges)
-- vim.print(available)

local result = 0

for _, ingredient in ipairs(available) do
  local is_fresh = false
  for _, fresh_range in ipairs(fresh_ranges) do
    if ingredient >= fresh_range.start and ingredient <=fresh_range._end then
      is_fresh = true
      result = result + 1
      break
    end
  end
  -- print(ingredient, ' fresh?:', is_fresh)
end


print('result:', result)
vim.fn.setreg('+', result)
