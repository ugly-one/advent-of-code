local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local result = 0

local lines = {}
for _, line in ipairs(input) do
  local indicator_lights = string.match(line, "%[(%S+)%]")
  indicator_lights = lib.to_table(indicator_lights)
  local lights_as_number = 0
  for index, light in ipairs(indicator_lights) do
    local light_bit = 0
    if light == '#' then light_bit = 1 end
    lights_as_number = lights_as_number + light_bit * math.pow(2, index - 1)
  end

  local buttons = {}
  for button_string in string.gmatch(line, "%((.-)%)") do
    local light_number = 0
    for light_index_string in string.gmatch(button_string, "%d+") do
      local light_index = tonumber(light_index_string)
      assert(light_index)
      light_number = light_number + math.pow(2, light_index)
    end
    table.insert(buttons, light_number)
  end
  local joltage_requirements = string.match(line, "%{(.-)%}")

  table.insert(lines, { target = lights_as_number, buttons = buttons })
end

local function generate_combinations(available, combination, all)
  if #available == 0 then return end

  local n = #available
  for i = 1, n do
    local value = available[i]
    local remaining = { unpack(available, i + 1, n) }
    local new_combination = lib.copy_table(combination)
    table.insert(new_combination, value)
    table.insert(all, new_combination)
    generate_combinations(remaining, new_combination, all)
  end
end

local function press(combination)
  local light = 0
  for i = 1, #combination do
    local button = combination[i]
    light = bit.bxor(light, button)
  end
  return light
end

for _, line in ipairs(lines) do
  local combinations = {}
  generate_combinations(line.buttons, {}, combinations)

  local steps = nil

  for i = 1, #combinations do
    local combination = combinations[i]
    local light = press(combination)
    if light == line.target then
      if steps == nil or #combination < steps then
        steps = #combination
      end
    end
  end

  result = result + steps
end

print('result:', result)
vim.fn.setreg('+', result)

