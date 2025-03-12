local lib = require('lib')

local input = lib.getLines()
local components = {}
for _, line in ipairs(input) do
  local part1, part2 = string.match(line, "(%d+)/(%d+)")
  part1 = tonumber(part1)
  part2 = tonumber(part2)
  local init = false
  if part1 == 0 or part2 == 0 then
    init = true
  end
  table.insert(components, { init = init, port1 = part1, port2 = part2 })
end

local function print_component(components)
  local line = ""
  for _, a in ipairs(components) do
    line = line .. ' ' .. a.port1 .. '<=>' .. a.port2
  end
  print(line)
end

local function part1()
  local strongest = nil
  local function make_bridge(strenght, pins, components, bridge)
    --print('bridge')
    --print_component(bridge)
    --print('available components')
    --print_component(components)
    --print('pins', pins)

    if strongest == nil then
      strongest = strenght
    else
      if strenght > strongest then
        strongest = strenght
      else
        local possible_strength = 0
        for _, component in ipairs(components) do
          possible_strength = possible_strength + component.port1 + component.port2
        end
        if strenght + possible_strength <= strenght then return end
      end
    end
    for i, component in ipairs(components) do
      if pins == nil and component.init then
        local new_pins = component.port1
        if component.port1 == 0 then new_pins = component.port2 end
        local remaining_components = lib.copy_table(components)
        table.remove(remaining_components, i)
        make_bridge(component.port1 + component.port2, new_pins, remaining_components, bridge)
      elseif pins ~= nil then
        if component.port1 == pins or component.port2 == pins then
          local new_strength = strenght + component.port1 + component.port2
          local new_pins = component.port1
          if component.port1 == pins then new_pins = component.port2 end
          local remaining_components = lib.copy_table(components)
          table.remove(remaining_components, i)
          make_bridge(new_strength, new_pins, remaining_components, bridge)
        end
      end
    end
  end

  make_bridge(0, nil, components, {})
  print(strongest)
end

local function part2()
  local strongest = nil
  local longest = nil
  local function make_bridge(strenght, length, pins, components, bridge)
    --print('bridge')
    --print_component(bridge)
    --print('available components')
    --print_component(components)
    --print('pins', pins)

    if strongest == nil then
      strongest = strenght
      longest = length
    else
      if length >= longest and strenght > strongest then
        strongest = strenght
      elseif length > longest then
        longest = length
        strongest = strenght
      else
        local possible_strength = 0
        local possible_length = 0
        for _, component in ipairs(components) do
          possible_strength = possible_strength + component.port1 + component.port2
          possible_length = possible_length + 1
        end
        if possible_length + length < longest then return end
        if possible_length + length == longest then
          if strenght + possible_strength <= strenght then return end
        end
      end
    end
    for i, component in ipairs(components) do
      if pins == nil and component.init then
        local new_pins = component.port1
        if component.port1 == 0 then new_pins = component.port2 end
        local remaining_components = lib.copy_table(components)
        table.remove(remaining_components, i)
        make_bridge(component.port1 + component.port2, length + 1, new_pins, remaining_components, bridge)
      elseif pins ~= nil then
        if component.port1 == pins or component.port2 == pins then
          local new_strength = strenght + component.port1 + component.port2
          local new_pins = component.port1
          if component.port1 == pins then new_pins = component.port2 end
          local remaining_components = lib.copy_table(components)
          table.remove(remaining_components, i)
          make_bridge(new_strength, length + 1, new_pins, remaining_components, bridge)
        end
      end
    end
  end

  make_bridge(0, 0, nil, components, {})
  print(strongest)
end
part1()
part2()
