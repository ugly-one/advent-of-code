local lib = require('lib')
local input = lib.getLines()
-- input = lib.getLines('test.txt')

local function bla(current_lights, remaining_buttons, current_step, target_lights, minimum)
  if current_lights == target_lights then
    if minimum == nil or current_step < minimum then
      return current_step
    end
  end

  if #remaining_buttons == 0 then return minimum end

  if minimum ~= nil and current_step > minimum then return minimum end

  local new_min = minimum

  for index, button in ipairs(remaining_buttons) do
    if not button.used then
      remaining_buttons[index].used = true

      local new_lights = bit.bxor(current_lights, button.value)

      local new_min_option = bla(new_lights, remaining_buttons, current_step + 1, target_lights, new_min)

      remaining_buttons[index].used = false

      if new_min_option ~= nil and (new_min == nil or new_min_option < new_min) then
        new_min = new_min_option
      end
    end
  end

  return new_min
end

local function find_minimal_presses(indicator_lights_goal, buttons)
  local result = bla(0, buttons, 0, indicator_lights_goal, nil)

  return result
end


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
    local button_number = 0
    for light in string.gmatch(button_string, "%d+") do
      local light_number = tonumber(light)
      assert(light_number)
      button_number = button_number + math.pow(2, light_number)
    end
    table.insert(buttons, { value = button_number, used = false })
  end
  local joltage_requirements = string.match(line, "%{(.-)%}")

  table.insert(lines, { target = lights_as_number, buttons = buttons })
end

for index, line in ipairs(lines) do
  local minPresses = find_minimal_presses(line.target, line.buttons)
  result = result + minPresses
end

print('result:', result)
vim.fn.setreg('+', result)
