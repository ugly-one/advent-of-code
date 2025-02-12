local lines = require 'lib'.getLines('testInput.txt')
local lines = require 'lib'.getLines()

local seeds = {}
local maps = {}
maps["seed"] = {}
maps["soil"] = {}
maps["fertilizer"] = {}
maps["water"] = {}
maps["light"] = {}
maps["temperature"] = {}
maps["humidity"] = {}
local current_map = {}

for _, line in ipairs(lines) do
  if string.sub(line, 1, 5) == 'seeds' then
    local seeds_string = string.sub(line, 8)
    local seeds_match = string.gmatch(seeds_string, "(%d+)")

    while true do
      local first = seeds_match()
      if (first == nil) then break end
      local seed_start = tonumber(first)
      local seed_range = tonumber(seeds_match())
      table.insert(seeds, { start = seed_start, range = seed_range, end_ = seed_start + seed_range })
    end
  else
    local type = string.match(line, "(%a+)[-]")
    if type then
      current_map = maps[type]
    elseif not (line == "") then
      local numbers_match = string.gmatch(line, "%d+")
      local destination = tonumber(numbers_match())
      local source = tonumber(numbers_match())
      local range = tonumber(numbers_match())
      local transformation = {
        source_start = source,
        source_end = source + range,
        destination_start = destination,
        destination_end = destination + range,
        diff = destination - source
      }
      table.insert(current_map, transformation)
    end
  end
end

local function get_previous(number, map)
  for _, transformation in ipairs(map) do
    if number >= transformation.destination_start and number <= transformation.destination_end then
      return number - transformation.diff
    end
  end
  return number
end

for number = 0, 9999999999 do
  local location = number
  number = get_previous(number, maps["humidity"])
  number = get_previous(number, maps["temperature"])
  number = get_previous(number, maps["light"])
  number = get_previous(number, maps["water"])
  number = get_previous(number, maps["fertilizer"])
  number = get_previous(number, maps["soil"])
  number = get_previous(number, maps["seed"])
  for _, seed_range in ipairs(seeds) do
    if number >= seed_range.start and number <= seed_range.end_ then
      print(location)
      return
    end
  end
end
