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
  local pairs = {}
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
