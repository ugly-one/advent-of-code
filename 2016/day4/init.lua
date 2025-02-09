local lines = require('lib').getLines()
local function sort(map)
  local sorted = {}
  for key, value in pairs(map) do
    table.insert(sorted, { key = key, value = value })
  end

  table.sort(sorted, function(a, b)
    if a.value == b.value then
      if a.key < b.key then
        return true
      else
        return false
      end
    elseif a.value > b.value then
      return true
    else
      return false
    end
  end)

  return sorted
end

local function parse(line)
  local match = string.gmatch(line, "[%a-]+")
  local sectorMatch = string.gmatch(line, "%d+")
  local name = match()
  name = string.sub(name, 1, #name - 1)
  local checksum = match()
  local sector = sectorMatch()
  return { name = name, checksum = checksum, sectorId = tonumber(sector) }
end

function check(name, checksum)
  local map = {}
  for i = 1, #name do
    local char = string.sub(name, i, i)
    if not (char == '-') then
      if map[char] then
        map[char] = map[char] + 1
      else
        map[char] = 1
      end
    end
  end

  local sorted = sort(map)
  for i = 1, #checksum do
    local char = string.sub(checksum, i, i)
    if sorted[i].key == char then
    else
      return false
    end
  end
  return true
end

local sum = 0
for i, line in ipairs(lines) do
  local parsedLine = parse(line)
  if (check(parsedLine.name, parsedLine.checksum)) then sum = sum + parsedLine.sectorId end
end
print(sum)

local function decrypt(name, forward)
  local result = ""
  for i = 1, #name do
    local char = string.sub(name, i, i)
    if char == '-' then
      result = result .. " "
    else
      local byte = string.byte(char)
      local clampedShift = forward % (122 - 97 + 1)
      local new_value = byte + clampedShift
      if new_value > 122 then
        new_value = 96 + (new_value - 122)
      end
      local new_char = string.char(new_value)
      result = result .. new_char
    end
  end
  return result
end

local decrypted_names_with_ids = {}
for i, line in ipairs(lines) do
  local parsedLine = parse(line)
  local decrypted_name = decrypt(parsedLine.name, parsedLine.sectorId)
  table.insert(decrypted_names_with_ids, decrypted_name .. ' ' .. parsedLine.sectorId)
end

require('lib').print(decrypted_names_with_ids)
