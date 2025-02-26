local lib = require('lib')
local input = lib.getLines()[1]
local code = {}
for n in string.gmatch(input, "(%d+)") do
  table.insert(code, tonumber(n))
end
local sum = 0
local id = 0
local function getNode(code, i)
  if i > #code then return end
  local children_count = code[i]
  local meta_count = code[i + 1]
  local new_i = i + 2
  local children = {}
  for c = 1, children_count do
    local node = getNode(code, new_i)
    new_i = node.new_i
    table.insert(children, node)
  end

  local meta = {}
  for m = 1, meta_count do
    table.insert(meta, code[new_i])
    new_i = new_i + 1
  end
  id = id + 1
  return { new_i = new_i, id = id, children = children, meta = meta }
end
local node = getNode(code, 1)

local function get_value(node)
  if #node.children == 0 then
    local sum = 0
    for _, i in ipairs(node.meta) do
      sum = sum + i
    end
    return sum
  end

  local sum = 0
  for _, index in ipairs(node.meta) do
    if index == 0 then
    elseif index > #node.children then
    else
      local child = node.children[index]
      sum = sum + get_value(child)
    end
  end
  return sum
end

local value = get_value(node)
print(value)
