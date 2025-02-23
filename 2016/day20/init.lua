local lib = require('lib')
local input = lib.getLines()
local min = 0
local max = 4294967295
local allowed_ranges = {
  { start = min, _end = max },
}
for i = 1, #input do
  local line = input[i]
  local range = string.gmatch(line, "%d+")
  local start = tonumber(range())
  local _end = tonumber(range())
  local new_allowed_ranges = {}
  for _, allowed_range in ipairs(allowed_ranges) do
    if allowed_range._end < start or allowed_range.start > _end then
      -- do nothing
      -- the new blocked range does not affect us
      table.insert(new_allowed_ranges, allowed_range)
    elseif allowed_range.start < start and allowed_range._end > _end then
      -- blocked range is completely within current range
      local new_end = start - 1
      table.insert(new_allowed_ranges, { start = allowed_range.start, _end = new_end })
      local new_start = _end + 1
      table.insert(new_allowed_ranges, { start = new_start, _end = allowed_range._end })
    elseif allowed_range.start == start and allowed_range._end > _end then
      -- bloked range starts at the start but finished somewhere in the middle
      table.insert(new_allowed_ranges, { start = _end + 1, _end = allowed_range._end })
    elseif allowed_range.start < start and allowed_range._end == _end then
      -- bloked range starts somewhere in the middle and finished at the end
      table.insert(new_allowed_ranges, { start = allowed_range.start, _end = start - 1 })
    elseif start <= allowed_range.start and _end >= allowed_range._end then
      -- remove entire range. blocked range encapsulates it entirely
    elseif start < allowed_range.start and _end < allowed_range._end then
      -- start is earlier but end is within
      table.insert(new_allowed_ranges, { start = _end + 1, _end = allowed_range._end })
    elseif start > allowed_range.start and _end > allowed_range._end then
      -- start is within, end is after
      table.insert(new_allowed_ranges, { start = allowed_range.start, _end = start - 1 })
    end
  end
  allowed_ranges = new_allowed_ranges
  --vim.print(allowed_ranges)
end

local min = 99999999999
for _, range in ipairs(allowed_ranges) do
  if range.start < min then min = range.start end
end
print(min)
local counter = 0
for _, range in ipairs(allowed_ranges) do
  counter = counter + range._end - range.start + 1
end
print(counter)
