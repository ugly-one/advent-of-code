local lib = require('lib')
local md5 = lib.md5.sumhexa
local is_open = {}
is_open['b'] = true
is_open['c'] = true
is_open['d'] = true
is_open['e'] = true
is_open['f'] = true

local threshold = 0
local hashes = {}

-- part2 was super slow
local function find_exit(pos, counter, passcode)
  if pos.x > 4 or pos.x < 1 or pos.y > 4 or pos.y < 1 then return end
  if pos.x == 4 and pos.y == 4 then
    if counter > threshold then
      threshold = counter
      print(threshold)
    end
    return
  end
  local hash = ""

  -- cache doesnt work...
  if hashes[passcode] then
    hash = hashes[passcode]
    print('hit cache')
  else
    hash = md5(passcode)
    hashes[passcode] = hash
  end
  local up = string.sub(hash, 1, 1)
  local down = string.sub(hash, 2, 2)
  local left = string.sub(hash, 3, 3)
  local right = string.sub(hash, 4, 4)

  local counter = counter + 1
  if is_open[left] then find_exit({ x = pos.x - 1, y = pos.y }, counter, passcode .. 'L') end
  if is_open[up] then find_exit({ x = pos.x, y = pos.y - 1 }, counter, passcode .. 'U') end
  if is_open[right] then find_exit({ x = pos.x + 1, y = pos.y }, counter, passcode .. 'R') end
  if is_open[down] then find_exit({ x = pos.x, y = pos.y + 1 }, counter, passcode .. 'D') end
end

local current_pos = { x = 1, y = 1 }
local passcode = lib.getLines()[1]
find_exit(current_pos, 0, passcode)
print(threshold)
