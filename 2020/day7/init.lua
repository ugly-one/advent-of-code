local lib = require('lib')
local lines = lib.getLines()

local function parse(line)
  local model = { color = "", content = {} }
  local m = string.gmatch(line, "((%a+) (%a+)) bags contain")
  model.color = m()

  local m = string.gmatch(line, "(%d+ %a+ %a+) bag")
  local content = {}
  for i in m do
    local n = tonumber(string.sub(i, 1, 1))
    local color = string.sub(i, 3)
    table.insert(content, { amount = n, color = color })
  end
  model.content = content
  return model
end

local target_color = "shiny gold"


local all_bags = {}
for _, line in ipairs(lines) do
  local bag = parse(line)
  all_bags[bag.color] = bag
end

local function check(bag, color)
  for i, inner_bag in ipairs(bag.content) do
    if inner_bag.color == color then return true end
    if check(all_bags[inner_bag.color], color) then return true end
  end
  return false
end

local function part1()
  local sum = 0
  for k, v in pairs(all_bags) do
    if check(v, target_color) then sum = sum + 1 end
  end

  print(sum)
end

local function count(bag)
  local sum = 1
  if bag.content == nil then return sum end
  for i, inner_bag in ipairs(bag.content) do
    sum = sum + inner_bag.amount * count(all_bags[inner_bag.color])
  end
  return sum
end
local function part2()
  local gold = all_bags["shiny gold"]
  print(count(gold) - 1)
end

part1()
part2()
