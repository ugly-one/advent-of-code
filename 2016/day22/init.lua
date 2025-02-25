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

local nodes = {}
for i = 1, #input do
  if string.sub(input[i], 1, 1) == '/' then
    table.insert(nodes, parse(input[i]))
  end
end

local function count_pairs(nodes)
  local pairs_count = 0
  local pairs = {}
  for i, node in ipairs(nodes) do
    local to_compare = lib.copy_table(nodes)
    table.remove(to_compare, i)
    for j, another_node in ipairs(to_compare) do
      if node.used > 0 and another_node.avail >= node.used then
        local hash = node.x .. '.' .. node.y .. '.' .. another_node.x .. '.' .. another_node.y
        local another_hash = another_node.x .. '.' .. another_node.y .. '.' .. node.x .. '.' .. node.y
        if pairs[hash] or pairs[another_hash] then
        else
          pairs[hash] = true
          pairs_count = pairs_count + 1
        end
      end
    end
  end
  print(pairs_count)
end

count_pairs(nodes)
