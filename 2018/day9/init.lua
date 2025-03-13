local lib = require('lib')
local input = lib.getLines()


local function print_circle(marble)
  local line = ""
  local start_value = marble.value
  while true do
    line = line .. ' ' .. marble.value
    marble = marble.next
    if marble.value == start_value then break end
  end
  print(line)
end

local function add_marble(marble, new_value)
  local new_marble = { value = new_value, previous = marble, next = marble.next }
  marble.next.previous = new_marble
  marble.next = new_marble
end

local function play(players, last_marble)
  local current_marble = { value = 0, next = nil, previous = nil }
  current_marble.next = current_marble
  current_marble.previous = current_marble
  local player = 1
  local scores = {}
  for p = 1, players do
    scores[p] = 0
  end
  for marble_value = 1, last_marble do
    if marble_value % 23 == 0 then
      local marble_to_remove = current_marble
      for _ = 1, 7 do
        marble_to_remove = marble_to_remove.previous
      end
      scores[player] = scores[player] + marble_value + marble_to_remove.value
      local previous_marble = marble_to_remove.previous
      local next_marble = marble_to_remove.next
      previous_marble.next = next_marble
      next_marble.previous = previous_marble
      current_marble = next_marble
    else
      local previous_marble = current_marble.next
      local next_marble = previous_marble.next
      local new_marble = { value = marble_value, previous = previous_marble, next = next_marble }
      previous_marble.next = new_marble
      next_marble.previous = new_marble
      current_marble = new_marble
    end
    -- print_circle(current_marble)
    player = player + 1
    if player > players then player = 1 end
  end

  local score = 0
  for p = 1, players do
    if scores[p] > score then score = scores[p] end
  end
  print(score)
end
for _, line in ipairs(input) do
  local players, last_marble = string.match(line, "(%d+).+worth (%d+)")
  players = tonumber(players)
  last_marble = tonumber(last_marble)
  play(players, last_marble * 100)
end
