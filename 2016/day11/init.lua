local lib = require('lib')
local lines = lib.getLines()
local floors = { {}, {}, {}, {} }
local types = {}
local types_count = 0
local chip_indexes = {}
local generator_indexes = {}
local index = 1
for i, line in ipairs(lines) do
  local chips_string = string.gmatch(line, "a (%a+)-compatible microchip")
  for c in chips_string do
    types[c] = index
    floors[i][index] = true
    table.insert(chip_indexes, index)
    index = index + 2
    types_count = types_count + 1
  end
end

for i = 1, 4 do
  for j = 1, types_count * 2 do
    if not floors[i][j] then floors[i][j] = false end
  end
end

for i, line in ipairs(lines) do
  local generators_string = string.gmatch(line, "a (%a+) generator")
  for g in generators_string do
    local index = types[g] + 1
    table.insert(generator_indexes, index)
    floors[i][index] = true
  end
end

local floor_size = types_count * 2

local function to_string(floor)
  local line = ""
  for j = 1, floor_size do
    if floor[j] then line = line .. 'x' else line = line .. ' ' end
  end
  return line
end
local function to_string_map(floors)
  local result = ""
  for i = 4, 1, -1 do
    local line = to_string(floors[i])
    result = result .. line
  end
  return result
end


local function get_indexes(floor)
  local indexes = {}
  for i = 1, floor_size do
    if floor[i] then table.insert(indexes, i) end
  end
  return indexes
end

local function get_permutations_rec(options, current, result)
  if #current == 2 then
    if math.abs(current[1] - current[2]) == 1 then
      for _, r in ipairs(result) do
        if #r == 2 then
          if math.abs(r[1] - r[2]) == 1 then return end
        end
      end
    end
    table.insert(result, current)
    return
  end
  if #current == 1 then
    table.insert(result, current)
  end
  for i = 1, #options do
    local current_copy = lib.copy_table(current)
    table.insert(current_copy, options[i])
    local options_copy = {}
    for j = i + 1, #options do
      table.insert(options_copy, options[j])
    end
    get_permutations_rec(options_copy, current_copy, result)
  end
end

local function get_permutations(options)
  local result = {}
  get_permutations_rec(options, {}, result)
  return result
end


local function copy_floors(floors)
  local result = {}
  for i = 1, 4 do
    table.insert(result, lib.copy_table(floors[i]))
  end
  return result
end

local visited = {}

local function entire_floor_occupied(floor)
  for i = 1, #floor do
    if not floor[i] then return false end
  end
  return true
end

local function is_valid(floor)
  local non_paired_chips = false
  for i = 1, floor_size, 2 do
    if floor[i] then
      if floor[i + 1] == false then
        non_paired_chips = true
        break
      end
    end
  end
  local non_paired_generators = false
  for i = 1, floor_size, 2 do
    if floor[i + 1] then
      if floor[i] == false then
        non_paired_generators = true
        break
      end
    end
  end
  if non_paired_chips and non_paired_generators then return false else return true end
end

local min = 100
local function part1(floors, steps, floor, steps_taken, previous_direction)
  if steps > min then return end
  if entire_floor_occupied(floors[4]) then
    if steps < min then min = steps end
    local line = ""
    for _, step in ipairs(steps_taken) do
      line = line .. ' -> ' .. step
    end
    print("new min", steps, line)
    return
  end
  local options = get_permutations(get_indexes(floors[floor]))
  local directions = {}
  if floor == 1 then
    directions = { 1 }
  elseif floor == 4 then
    directions = { -1 }
  elseif floor == 2 or floor == 3 then
    if previous_direction == -1 then
      directions = { 1 }
    else
      directions = { 1, -1 }
    end
  end
  for _, option in ipairs(options) do
    for _, direction in ipairs(directions) do
      if #option == 1 and direction == 1 then
        -- do not try to bring one item up. going with one item is slow - let's try to make this assumption
      elseif #option == 2 and direction == -1 then
        -- do not bring 2 things down. I think it makes sense to only bring one down.
      else
        local floors_copy = copy_floors(floors)
        local step_taken = tostring(direction)
        for _, item in ipairs(option) do
          floors_copy[floor][item] = false
          floors_copy[floor + direction][item] = true
          step_taken = step_taken .. ' ' .. item
        end
        if is_valid(floors_copy[floor]) and is_valid(floors_copy[floor + direction]) then
          local hash = to_string_map(floors_copy) .. tostring(floor + direction)
          local maybe_visited = visited[hash]
          local steps_taked_copy = lib.copy_table(steps_taken)
          table.insert(steps_taked_copy, step_taken)
          if not maybe_visited then
            visited[hash] = steps + 1
            part1(floors_copy, steps + 1, floor + direction, steps_taked_copy, direction)
          else
            if steps + 1 < maybe_visited then
              visited[hash] = steps + 1
              part1(floors_copy, steps + 1, floor + direction, steps_taked_copy, direction)
            end
          end
        end
      end
    end
  end
end

part1(floors, 0, 1, {}, -2)
print(min)
