local lib = require('lib')
local md5 = lib.md5.sumhexa
local is_open = {}
is_open['b'] = true
is_open['c'] = true
is_open['d'] = true
is_open['e'] = true
is_open['f'] = true

local min = 99999999
local min_path = {}
local count = 0
local function find_exit(current_pos, previous_moves)
  --print('pos ', current_pos.x, current_pos.y, #previous_moves)
  if current_pos.x == 4 and current_pos.y == 4 then
    if #previous_moves < min then
      min = #previous_moves
      min_path = previous_moves
    end
    return
  end
  if #previous_moves >= min then
    return
  end
  local passcode = 'pxxbnzuo'
  for _, move in ipairs(previous_moves) do
    passcode = passcode .. move
  end

  local hash = md5(passcode)
  local up = string.sub(hash, 1, 1)
  local down = string.sub(hash, 2, 2)
  local left = string.sub(hash, 3, 3)
  local right = string.sub(hash, 4, 4)
  local open_doors = {}

  if current_pos.x == 1 and current_pos.y == 1 then
    -- left top
    if is_open[right] then table.insert(open_doors, { x = current_pos.x + 1, y = current_pos.y, letter = 'R' }) end
    if is_open[down] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y + 1, letter = 'D' }) end
  elseif current_pos.x == 4 and current_pos.y == 4 then
    -- right bottm
    if is_open[left] then table.insert(open_doors, { x = current_pos.x - 1, y = current_pos.y, letter = 'L' }) end
    if is_open[up] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y - 1, letter = 'U' }) end
  elseif current_pos.x == 1 and current_pos.y == 4 then
    -- left bottom
    if is_open[right] then table.insert(open_doors, { x = current_pos.x + 1, y = current_pos.y, letter = 'R' }) end
    if is_open[up] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y - 1, letter = 'U' }) end
  elseif current_pos.x == 4 and current_pos.y == 1 then
    -- right top
    if is_open[left] then table.insert(open_doors, { x = current_pos.x - 1, y = current_pos.y, letter = 'L' }) end
    if is_open[down] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y + 1, letter = 'D' }) end
  elseif current_pos.x == 1 then
    -- left edge
    if is_open[down] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y + 1, letter = 'D' }) end
    if is_open[up] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y - 1, letter = 'U' }) end
    if is_open[right] then table.insert(open_doors, { x = current_pos.x + 1, y = current_pos.y, letter = 'R' }) end
  elseif current_pos.x == 4 then
    -- right edge
    if is_open[down] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y + 1, letter = 'D' }) end
    if is_open[up] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y - 1, letter = 'U' }) end
    if is_open[left] then table.insert(open_doors, { x = current_pos.x - 1, y = current_pos.y, letter = 'L' }) end
  elseif current_pos.y == 1 then
    -- top edge
    if is_open[down] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y + 1, letter = 'D' }) end
    if is_open[left] then table.insert(open_doors, { x = current_pos.x - 1, y = current_pos.y, letter = 'L' }) end
    if is_open[right] then table.insert(open_doors, { x = current_pos.x + 1, y = current_pos.y, letter = 'R' }) end
  elseif current_pos.y == 4 then
    -- bottom edge
    if is_open[up] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y - 1, letter = 'U' }) end
    if is_open[right] then table.insert(open_doors, { x = current_pos.x + 1, y = current_pos.y, letter = 'R' }) end
    if is_open[left] then table.insert(open_doors, { x = current_pos.x - 1, y = current_pos.y, letter = 'L' }) end
  else
    -- middle
    if is_open[left] then table.insert(open_doors, { x = current_pos.x - 1, y = current_pos.y, letter = 'L' }) end
    if is_open[up] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y - 1, letter = 'U' }) end
    if is_open[right] then table.insert(open_doors, { x = current_pos.x + 1, y = current_pos.y, letter = 'R' }) end
    if is_open[down] then table.insert(open_doors, { x = current_pos.x, y = current_pos.y + 1, letter = 'D' }) end
  end

  for _, open_door in ipairs(open_doors) do
    local copy = lib.copy_table(previous_moves)
    table.insert(copy, open_door.letter)
    find_exit({ x = open_door.x, y = open_door.y }, copy)
  end
end

local current_pos = { x = 1, y = 1 }
find_exit(current_pos, {})
print(min)
local result = ""
for _, move in ipairs(min_path) do
  result = result .. move
end
print(result)
