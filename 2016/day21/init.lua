local lib = require('lib')
local input = lib.getLines()
local password = lib.to_table('decab')
local function find_pos(table, item)
  for i, _item in ipairs(table) do
    if item == _item then return i end
  end
  return nil
end

local function swap(password, x, y)
  local temp = password[x]
  password[x] = password[y]
  password[y] = temp
end

local function rotate_right(password, value)
  if value == 0 then return end
  local copy = lib.copy_table(password)
  value = value % #password
  for i = 1, #password do
    local index = i - value
    if index <= 0 then index = #password + index end
    password[i] = copy[index]
  end
end

local function rotate_left(password, value)
  if value == 0 then return end
  local copy = lib.copy_table(password)
  value = value % #password
  for i = 1, #password do
    local index = i + value
    if index > #password then index = index - #password end
    password[i] = copy[index]
  end
end

local function bla(password, line)
  local swap_position = string.match(line, "swap position")
  if swap_position then
    local positions = string.gmatch(line, "position (%d)")
    local x = tonumber(positions()) + 1
    local y = tonumber(positions()) + 1
    swap(password, x, y)
  end
  local swap_letters = string.match(line, "swap letter")
  if swap_letters then
    local letters = string.gmatch(line, "letter (%a)")
    local l1 = letters()
    local l2 = letters()
    local x = find_pos(password, l1)
    local y = find_pos(password, l2)
    swap(password, x, y)
  end
  local reverse = string.match(line, "reverse")
  if reverse then
    local positions = string.gmatch(line, "(%d)")
    local start = tonumber(positions()) + 1
    local _end = tonumber(positions()) + 1
    local diff = _end - start
    local count = math.ceil(diff / 2)
    for i = 1, count do
      local x = start - 1 + i
      local y = _end + 1 - i
      swap(password, x, y)
    end
  end
  local rotate_l = string.match(line, "rotate left")
  if rotate_l then
    local diff = tonumber(string.match(line, "%d"))
    if reverse then
      rotate_right(password, diff)
    else
      rotate_left(password, diff)
    end
  end
  local rotate_r = string.match(line, "rotate right")
  if rotate_r then
    local diff = tonumber(string.match(line, "%d"))
    rotate_right(password, diff)
  end
  local move = string.match(line, "move position")
  if move then
    local positions = string.gmatch(line, "(%d)")
    local x = tonumber(positions()) + 1
    local y = tonumber(positions()) + 1
    local value = table.remove(password, x)
    table.insert(password, y, value)
  end
  local rotate_based = string.match(line, "rotate based on position of letter")
  if rotate_based then
    local letter = string.match(line, "letter (%a)")
    local index = 1
    for i = 1, #password do
      if password[i] == letter then
        index = i - 1
        break
      end
    end
    local rotations = 1 + index
    if index >= 4 then rotations = rotations + 1 end
    rotate_right(password, rotations)
  end
end
local function scramble(password, input)
  for _, line in ipairs(input) do
    bla(password, line)
  end
  return password
end

local function generate_password(password, letters, result)
  if #letters == 0 then
    table.insert(result, password)
    return
  end
  for i, letter in ipairs(letters) do
    local copy = lib.copy_table(password)
    table.insert(copy, letter)
    local remaining = lib.copy_table(letters)
    table.remove(remaining, i)
    generate_password(copy, remaining, result)
  end
end
local function get_possible_passwords()
  local letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' }
  local possibilites = {}
  local password = {}
  generate_password(password, letters, possibilites)
  return possibilites
end

local function brute_force(hash)
  local passwords = get_possible_passwords()
  for _, password in ipairs(passwords) do
    local string_ = lib.to_string(password)
    local calculated_hash = scramble(password, input)
    if lib.to_string(calculated_hash) == hash then
      print(string_)
    end
  end
end
--scramble(password, input, true)
brute_force('fbgdceah')

print(lib.to_string(scramble(lib.to_table("abcdefgh"), input)))
