local lib = require('lib')
local input = lib.getLines()

local function parse(line)
  local name = string.gmatch(line, "(%d+)")
  local a = name()
  local b = name()
  local x = tonumber(a)
  local y = tonumber(b)
  local spaces = string.gmatch(line, "(%d+)T")
  return { x = x, y = y, size = tonumber(spaces()), used = tonumber(spaces()), avail = tonumber(spaces()) }
end
local flat_nodes = {}
local nodes = {}
for i = 1, #input do
  if string.sub(input[i], 1, 1) == '/' then
    local node = parse(input[i])
    if nodes[node.y] then
      nodes[node.y][node.x] = node
    else
      nodes[node.y] = {}
      nodes[node.y][node.x] = node
    end
    table.insert(flat_nodes, node)
  end
end

local function count_pairs(nodes)
  local pairs_count = 0
  for i = 1, #nodes - 1 do
    for j = i + 1, #nodes do
      local node = nodes[i]
      local another_node = nodes[j]
      if (node.used > 0 and another_node.avail >= node.used) or (another_node.used > 0 and node.avail >= another_node.used) then
        pairs_count = pairs_count + 1
      end
    end
  end

  print(pairs_count)
end

count_pairs(flat_nodes)

local rows = #nodes
local columns = #nodes[1]

local char_nodes = {}
for i = 1, #input do
  if string.sub(input[i], 1, 1) == '/' then
    local node = parse(input[i])
    if not char_nodes[node.y] then
      char_nodes[node.y] = {}
    end
    local char = '.'
    if node.used == 0 then
      char = '_'
    elseif node.used > 300 then
      char = '#'
    end
    char_nodes[node.y][node.x] = char
  end
end

char_nodes[1][columns] = 'G'

local function print_nodes(nodes)
  for y = 1, rows do
    local line = ""
    for x = 1, columns do
      line = line .. nodes[y][x]
    end
    print(line)
  end
end

print_nodes(char_nodes)

local distance_to_door = 17 - 6 + 24 - 13
local distance_from_door_to_first_front = 37 - 6 + 13 - 1
local distance = distance_from_door_to_first_front + distance_to_door

print(distance + 36 * 5 + 1)
