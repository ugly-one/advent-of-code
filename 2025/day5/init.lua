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
      table.insert(fresh_ranges, { start = tonumber(start), _end = tonumber(_end), deleted = false })
    else
      table.insert(available, tonumber(line))
    end
  end
end

-- vim.print(fresh_ranges)
-- vim.print(available)

local result_part1 = 0

for _, ingredient in ipairs(available) do
  for _, fresh_range in ipairs(fresh_ranges) do
    if ingredient >= fresh_range.start and ingredient <= fresh_range._end then
      result_part1 = result_part1 + 1
      break
    end
  end
end


print('result:', result_part1)
vim.fn.setreg('+', result_part1)

for current_range_index, current_range in ipairs(fresh_ranges) do
  for index, range in ipairs(fresh_ranges) do
    if current_range_index == index then goto continue end

    if range.deleted == true then goto continue end

    if current_range.start <= range.start and current_range._end >= range._end then
      -- range is within current_range
      range.start = current_range.start
      range._end = current_range._end
      current_range.deleted = true
    elseif current_range.start >= range.start and current_range._end <= range._end then
      -- range entirely encapsulates current_range
      current_range.deleted = true
    elseif current_range.start >= range.start and current_range.start <= range.start and current_range._end >= range._end then
      range._end = current_range._end
      current_range.deleted = true
    elseif current_range.start <= range.start and current_range._end >= range.start and current_range._end <= current_range._end then
      if current_range.start < range.start then
        range.start = current_range.start
      end
      current_range.deleted = true
    end
    ::continue::
  end
end

local result_part2 = 0
for _, range in ipairs(fresh_ranges) do
  if range.deleted then goto continue2 end

  result_part2 = result_part2 + range._end - range.start + 1
  ::continue2::
end

vim.print(result_part2)
vim.fn.setreg('+', result_part2)
