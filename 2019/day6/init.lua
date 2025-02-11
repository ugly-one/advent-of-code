local lines = require 'lib'.getLines()

local space = {}
for _, line in ipairs(lines) do
  local match = string.gmatch(line, "([%a%d]+)")
  local parentName = match()
  local childName = match()
  local child = nil
  if space[childName] == nil then
    child = { name = childName, children = {}, parent = nil }
    space[childName] = child
  else
    child = space[childName]
  end
  local parent = nil
  if space[parentName] == nil then
    parent = { name = parentName, children = { child } }
    child.parent = parent
    space[parentName] = parent
  else
    parent = space[parentName]
    child.parent = parent
    table.insert(parent.children, child)
  end
end

local sum = 0
local function mark(object, distance, pathToRoot)
  object.distance = distance
  object.pathToRoot = pathToRoot
  sum = sum + distance
  for _, child in ipairs(object.children) do
    local copyPath = {}
    for _, item in ipairs(pathToRoot) do
      table.insert(copyPath, item)
    end
    table.insert(copyPath, object.name)
    mark(child, distance + 1, copyPath)
  end
end

local function markDistance()
  local root = space["COM"]
  local children = root.children

  for _, child in ipairs(children) do
    mark(child, 1, { "COM" })
  end
end

markDistance()
print(sum)

local me = space["YOU"]
local santa = space["SAN"]

local distanceFromRootToCommon = 0
for i, object in ipairs(me.pathToRoot) do
  if santa.pathToRoot[i] == object then
  else
    distanceFromRootToCommon = i - 2
    break
  end
end

print()
print(me.distance - distanceFromRootToCommon - 1 + santa.distance - distanceFromRootToCommon - 1)
