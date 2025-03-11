local lib = require('lib')
local input = lib.getLines()
local rules = {}
local function print_pixels(pixels)
  for row = 1, #pixels do
    local line = ""
    for column = 1, #pixels[1] do
      line = line .. pixels[row][column]
    end
    print(line)
  end
end

local function to_array(pattern)
  local result = {}
  local match = string.gmatch(pattern, "([%.#]+)")
  for group in match do
    table.insert(result, lib.to_table(group))
  end
  return result
end

local function rotate_once(source_array)
  if #source_array == 2 then
    local rotated = { {}, {} }
    rotated[1][1] = source_array[1][2]
    rotated[1][2] = source_array[2][2]
    rotated[2][1] = source_array[1][1]
    rotated[2][2] = source_array[2][1]
    return rotated
  else
    local rotated = { {}, {}, {} }
    rotated[1][1] = source_array[1][3]
    rotated[1][2] = source_array[2][3]
    rotated[1][3] = source_array[3][3]
    rotated[2][1] = source_array[1][2]
    rotated[2][2] = source_array[2][2]
    rotated[2][3] = source_array[3][2]
    rotated[3][1] = source_array[1][1]
    rotated[3][2] = source_array[2][1]
    rotated[3][3] = source_array[3][1]
    return rotated
  end
end

local function flip(source_array)
  local result = {}
  if #source_array == 2 then
    -- horizontally
    local flipped = {}
    flipped[1] = source_array[2]
    flipped[2] = source_array[1]
    table.insert(result, flipped)
    -- vertically
    flipped = { {}, {} }
    flipped[1][1] = source_array[1][2]
    flipped[1][2] = source_array[1][1]
    flipped[2][1] = source_array[2][2]
    flipped[2][2] = source_array[2][1]
    table.insert(result, flipped)
  else
    -- horizontally
    local flipped = {}
    flipped[1] = source_array[3]
    flipped[2] = source_array[2]
    flipped[3] = source_array[1]
    table.insert(result, flipped)
    -- vertically
    flipped = { {}, {}, {} }
    flipped[1][1] = source_array[1][3]
    flipped[1][2] = source_array[1][2]
    flipped[1][3] = source_array[1][1]
    flipped[2][1] = source_array[2][3]
    flipped[2][2] = source_array[2][2]
    flipped[2][3] = source_array[2][1]
    flipped[3][1] = source_array[3][3]
    flipped[3][2] = source_array[3][2]
    flipped[3][3] = source_array[3][1]

    table.insert(result, flipped)
  end
  return result
end

local function rotate(source_array)
  local result = {}
  table.insert(result, source_array)
  local flipped = flip(source_array)
  for _, r in ipairs(flipped) do
    table.insert(result, r)
  end

  local rotated = rotate_once(source_array)
  table.insert(result, rotated)
  local flipped = flip(rotated)
  for _, r in ipairs(flipped) do
    table.insert(result, r)
  end

  rotated = rotate_once(rotated)
  table.insert(result, rotated)
  local flipped = flip(rotated)
  for _, r in ipairs(flipped) do
    table.insert(result, r)
  end

  rotated = rotate_once(rotated)
  table.insert(result, rotated)
  local flipped = flip(rotated)
  for _, r in ipairs(flipped) do
    table.insert(result, r)
  end
  return result
end


for _, line in ipairs(input) do
  local source, destination = string.match(line, "(.+) => (.+)")
  local source_array = to_array(source)
  local destination_array = to_array(destination)
  table.insert(rules, {
    source = source_array,
    destination = destination_array
  })
  local rotated = rotate(source_array)
  for _, r in ipairs(rotated) do
    table.insert(rules, {
      source = r,
      destination = destination_array
    })
  end
end

local image = {
  { ".", "#", ".", },
  { ".", ".", "#", },
  { "#", "#", "#", },
}
local function match(rule, image)
  if #rule == #image then
    local size = #rule
    for y = 1, size do
      for x = 1, size do
        if rule[y][x] ~= image[y][x] then return false end
      end
    end
    return true
  else
    return false
  end
end
local function transform(size, chunk_size, image)
  local new_image = {}
  local chunks = size / chunk_size
  for chunk_y = 1, chunks do
    for chunk_x = 1, chunks do
      local sub_image = {}
      for y = 1, chunk_size do
        sub_image[y] = {}
        for x = 1, chunk_size do
          sub_image[y][x] = image[(chunk_y - 1) * chunk_size + (y)][(chunk_x - 1) * chunk_size + (x)]
        end
      end
      local found_match = false
      for _, rule in ipairs(rules) do
        if match(rule.source, sub_image) then
          for sub_y = 1, #rule.destination do
            for sub_x = 1, #rule.destination[1] do
              local y = (chunk_y - 1) * #rule.destination + sub_y
              if new_image[y] == nil then new_image[y] = {} end
              new_image[y][(chunk_x - 1) * #rule.destination[1] + sub_x] = rule.destination
                  [sub_y][sub_x]
            end
          end
          found_match = true
          break
        end
      end
      if not found_match then print('no match??') end
    end
  end
  return new_image
end

-- [SLOW] not super slow, takes less than a minute, but can be improved.
-- for example we can divide rules into 2 buckets (2 and 3 sized images - based on source size), and thanks to that when we try to find matching rule, we can search only withing one bucket since we know the size of the sub image we try to convert
for iter = 1, 18 do
  print(iter)
  local size = #(image[1])
  if size % 2 == 0 then
    image = transform(size, 2, image)
  else
    image = transform(size, 3, image)
  end
end

local counter = 0
for y = 1, #image do
  for x = 1, #image[1] do
    if image[y][x] == '#' then counter = counter + 1 end
  end
end
print(counter)
