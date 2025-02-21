local test = {
  "Disc #1 has 5 positions; at time=0, it is at position 4.",
  "Disc #2 has 2 positions; at time=0, it is at position 1."
}
local test = require('lib').getLines()

local disks_count = 0
local disks = {}
for i, line in ipairs(test) do
  local disk = { id = i }
  local positions = string.match(line, "has (%d+) position")
  disk.positions = tonumber(positions)
  local start_position = string.match(line, "at position (%d+)")
  disk.current_position = tonumber(start_position)
  table.insert(disks, disk)
  disks_count = disks_count + 1
end


local function get_position(disk, time)
  return (time + disk.current_position) % disk.positions
end

local function part1()
  local start_time = 1
  while true do
    local success = true
    for i, disk in ipairs(disks) do
      local position = get_position(disk, i + start_time)
      if position ~= 0 then
        success = false
        break
      end
    end
    if success then
      print(start_time)
      break
    end
    start_time = start_time + 1
  end
end

part1()
table.insert(disks, { id = disks_count + 1, positions = 11, current_position = 0 })
part1()
