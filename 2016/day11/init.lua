local lib = require('lib')
local lines = lib.getLines('test.txt')
local floors = {}
local types = {}
for i, line in ipairs(lines) do
  local floor = {}
  local chips_string = string.gmatch(line, "a (%a+)-compatible microchip")
  for c in chips_string do
    table.insert(types, c)
    table.insert(floor, { name = c, chip = true })
  end

  local generators_string = string.gmatch(line, "a (%a+) generator")
  for g in generators_string do
    table.insert(floor, { name = g, generator = true })
  end
  table.insert(floors, floor)
end

local types_to_id = {}
for i, type in ipairs(types) do
  types_to_id[type] = i
end
local function generate_all_moves(options, current, result)
  if #current == 2 then return end
  for i = 1, #options do
    local options_copy = {}
    for j = i + 1, #options do
      table.insert(options_copy, options[j])
    end
    local current_copy = lib.copy_table(current)
    table.insert(current_copy, options[i])
    table.insert(result, current_copy)
    generate_all_moves(options_copy, current_copy, result)
  end
end


local function remove_invalid_pairs(options)
  for i, option in ipairs(options) do
    if #option == 2 then
      if (option[1].chip and option[2].generator) or (option[2].chip and option[1].generator)
          and not (option[1].name == option[2].name) then
        table.remove(options, i)
      end
    end
  end
end

local function generate(options)
  local result = {}
  local current = {}
  generate_all_moves(options, current, result)
  remove_invalid_pairs(result)
  return result
end

local function is_valid_floor(floor)
  local map = {}
  for i = 1, #floor do
    local item = floor[i]
    if map[item.name] then
      map[item.name] = 'safe'
    else
      if item.generator then
        map[item.name] = 'dangerous'
      else
        map[item.name] = 'chip'
      end
    end
  end

  local has_unprotected_chip = false
  local has_dangerous_generator = false
  for k, v in pairs(map) do
    if v == 'dangerous' then has_dangerous_generator = true end
    if v == 'chip' then has_unprotected_chip = true end
  end
  if has_unprotected_chip and has_dangerous_generator then return false else return true end
end

local function copy_floors(floors)
  local copy = {}
  for i = 1, 4 do
    table.insert(copy, lib.copy_table(floors[i]))
  end
  return copy
end

local function simulate_option(floors, option, source_floor_id, target_floor_id)
  local copy_floors = copy_floors(floors)
  local source_floor = copy_floors[source_floor_id]
  local target_floor = copy_floors[target_floor_id]
  for _, item in ipairs(option) do
    local item_id_to_remove = 0
    for i, floor_item in ipairs(source_floor) do
      if floor_item.name == item.name and floor_item.chip == item.chip and floor_item.generator == item.generator then
        item_id_to_remove = i
      end
    end
    table.remove(source_floor, item_id_to_remove)

    table.insert(target_floor, item)
  end

  if is_valid_floor(source_floor) and is_valid_floor(target_floor) then
    return copy_floors
  else
    return nil
  end
end

local function print_floors(floors)
  for i = 4, 1, -1 do
    local floor = floors[i]
    local line = ""
    for _, item in ipairs(floor) do
      local type = 'M'
      if item.chip then type = 'M' else type = 'G' end
      line = line .. ' ' .. string.upper(string.sub(item.name, 1, 1)) .. type
    end
    print(line)
  end
end

local min = 30

local function equal_option(o1, o2)
  if #o1 == #o2 then
    for i = 1, #o1 do
      local match = false
      for j = 1, #o2 do
        if o1[i].name == o2[j].name and o1[i].chip == o2[j].chip then match = true end
      end
      if not match then return false end
    end
    return true
  end
  return false
end

local checked_maps = {}

local function try_move_all_to_last_floor(
    floors,
    current_floor_id,
    current_step,
    previous_move,
    previous_moves)
  if current_step > min then return end

  if #floors[1] == 0 and #floors[2] == 0 and #floors[3] == 0 and current_step < min then
    print(' new best !', current_step)
    min = current_step
    return
  end
  local floor = floors[current_floor_id]
  local all_options = generate(floor)
  local directions = {}
  if current_floor_id < 4 and current_floor_id > 1 then
    directions = { 1, -1 }
  elseif current_floor_id == 4 then
    directions = { -1 }
  else
    directions = { 1 }
  end

  for _, option in ipairs(all_options) do
    for _, direction in ipairs(directions) do
      local new_move = { option = option, direction = direction }
      if previous_move and new_move.direction == -previous_move.direction and equal_option(new_move.option, previous_move.option) then
      else
        local new_floor = simulate_option(floors, option, current_floor_id, current_floor_id + direction)
        if new_floor then
          --local new_previous_moves = {}
          --for i = 1, #previous_moves do
          --  table.insert(new_previous_moves,
          --    { option = lib.copy_table(previous_moves[i].option), direction = previous_moves[i].direction })
          --end
          --table.insert(new_previous_moves, new_move)
          try_move_all_to_last_floor(new_floor, current_floor_id + direction, current_step + 1, new_move,
            previous_moves)
        end
      end
    end
  end
end

try_move_all_to_last_floor(floors, 1, 0, nil, {})

print(min)
