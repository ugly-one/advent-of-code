local lib = require('lib')
local input = lib.getLines()
local map = {}
for _, line in ipairs(input) do
  local row = lib.to_table(line)
  table.insert(map, row)
end
local carts = {}

for y = 1, #map do
  for x = 1, #map[1] do
    local point = map[y][x]
    if point == '>' or point == '<' or point == '^' or point == 'v' then
      table.insert(carts, { x = x, y = y, direction = point, intersection_count = 0, crashed = false })
      if point == '>' or point == '<' then
        map[y][x] = '-'
      end
      if point == 'v' or point == '<' then
        map[y][x] = '|'
      end
    end
  end
end

local function print_map(map, carts)
  for y = 1, #map do
    local line = ""
    for x = 1, #map[1] do
      local has_cart = false
      for _, cart in ipairs(carts) do
        if cart.x == x and cart.y == y then
          has_cart = cart.direction
        end
      end
      if has_cart then line = line .. has_cart else line = line .. map[y][x] end
    end
    print(line)
  end
end

local positions = {}
for _, cart in ipairs(carts) do
  local hash = cart.x .. '.' .. cart.y
  positions[hash] = true
end

print_map(map, carts)
local function drive()
  for _, cart in ipairs(carts) do
    if cart.crashed then
    else
      local hash = cart.x .. '.' .. cart.y
      positions[hash] = nil

      local next_position = nil
      if cart.direction == '>' then next_position = { x = cart.x + 1, y = cart.y } end
      if cart.direction == '<' then next_position = { x = cart.x - 1, y = cart.y } end
      if cart.direction == 'v' then next_position = { x = cart.x, y = cart.y + 1 } end
      if cart.direction == '^' then next_position = { x = cart.x, y = cart.y - 1 } end
      cart.x = next_position.x
      cart.y = next_position.y

      local hash = cart.x .. '.' .. cart.y
      if positions[hash] then
        -- mark crashed carts

        for _, c in ipairs(carts) do
          if c.x == cart.x and c.y == cart.y then c.crashed = true end
        end
        positions[hash] = nil
      else
        positions[hash] = true

        local current_point = map[cart.y][cart.x]
        if current_point == '\\' then
          if cart.direction == '>' then
            cart.direction = 'v'
          elseif cart.direction == '<' then
            cart.direction = '^'
          elseif cart.direction == '^' then
            cart.direction = '<'
          elseif cart.direction == 'v' then
            cart.direction = '>'
          end
        elseif current_point == '/' then
          if cart.direction == '>' then
            cart.direction = '^'
          elseif cart.direction == '<' then
            cart.direction = 'v'
          elseif cart.direction == '^' then
            cart.direction = '>'
          elseif cart.direction == 'v' then
            cart.direction = '<'
          end
        elseif current_point == '+' then
          cart.intersection_count = cart.intersection_count + 1
          if cart.intersection_count > 3 then
            cart.intersection_count = 1
          end
          if cart.intersection_count == 1 then
            if cart.direction == '>' then
              cart.direction = '^'
            elseif cart.direction == '<' then
              cart.direction = 'v'
            elseif cart.direction == 'v' then
              cart.direction = '>'
            elseif cart.direction == '^' then
              cart.direction = '<'
            end
          elseif cart.intersection_count == 3 then
            if cart.direction == '>' then
              cart.direction = 'v'
            elseif cart.direction == '<' then
              cart.direction = '^'
            elseif cart.direction == 'v' then
              cart.direction = '<'
            elseif cart.direction == '^' then
              cart.direction = '>'
            end
          elseif cart.intersection_count == 2 then
          end
        end
      end
    end
  end

  local new_carts = {}
  for _, cart in ipairs(carts) do
    if not cart.crashed then
      table.insert(new_carts, cart)
    end
  end
  carts = new_carts
  if #carts == 1 then
    print(carts[1].x - 1, carts[1].y - 1)
    return true
  end

  if #carts == 0 then return true end
  --print_map(map, carts)
  --we have to sort the cart to make sure each tick we process carts that are closer to top left corner first
  table.sort(carts, function(c1, c2)
    if c1.y < c2.y then return true end
    if c1.y > c2.y then return false end
    if c1.x < c2.x then return true end
    return false
  end)
  return false
end

repeat
  local crash = drive()
until crash
