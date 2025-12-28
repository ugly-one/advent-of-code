local lib = require('lib')
local input = lib.getLines()
-- local input = lib.getLines('test.txt')

local boxes = {}
for y = 1, #input do
  local x, y, z = string.match(input[y], "(%d+),(%d+),(%d+)")
  table.insert(boxes, { x = tonumber(x), y = tonumber(y), z = tonumber(z), circuit = 0 })
end

local function distance(box1, box2)
  local part = math.pow(box1.x - box2.x, 2) + math.pow(box1.y - box2.y, 2) + math.pow(box1.z - box2.z, 2)
  return math.sqrt(part)
end

local new_circuit_nr = 1

local next_distance = 0

local connected_boxes = 0
local min_distance_box1
local min_distance_box2

while connected_boxes < #boxes do
  local min_distance

  for i = 1, #boxes - 1 do
    local box1 = boxes[i]
    for y = i + 1, #boxes do
      local box2 = boxes[y]
      local dist = distance(box1, box2)
      if (dist > next_distance) then
        if min_distance == nil or dist < min_distance then
          min_distance_box1 = box1
          min_distance_box2 = box2
          min_distance = dist
        end
      end
    end
  end

  next_distance = min_distance

  if min_distance_box1.circuit == 0 and min_distance_box2.circuit == 0 then
    min_distance_box1.circuit = new_circuit_nr
    min_distance_box2.circuit = new_circuit_nr
    new_circuit_nr = new_circuit_nr + 1
    connected_boxes = connected_boxes + 2
  elseif min_distance_box1.circuit == 0 then
    min_distance_box1.circuit = min_distance_box2.circuit
    connected_boxes = connected_boxes + 1
  elseif min_distance_box2.circuit == 0 then
    min_distance_box2.circuit = min_distance_box1.circuit
    connected_boxes = connected_boxes + 1
  elseif min_distance_box1.circuit ~= min_distance_box2.circuit then
    -- find all boxes connected to box2 and connect it to circuit of box1
    -- vim.print(min_distance_box1)
    -- vim.print(min_distance_box2)
    local circuit_to_find = min_distance_box2.circuit
    for _, box in ipairs(boxes) do
      if box.circuit == circuit_to_find then
        box.circuit = min_distance_box1.circuit
      end
    end
  else
  end

  -- lib.print(boxes)
  -- vim.print(min_distance_box1, min_distance_box2)
end
local result = 0

result = min_distance_box1.x * min_distance_box2.x

-- local circuit_counts = {}
-- -- lib.print(boxes)
--
-- for _, box in ipairs(boxes) do
--   local key = tostring(box.circuit)
--   if circuit_counts[key] ~= nil then
--     circuit_counts[key] = circuit_counts[key] + 1
--   else
--     circuit_counts[key] = 1
--   end
-- end
--
-- -- vim.print(circuit_counts)
--
-- local sizes = {}
-- for key, value in pairs(circuit_counts) do
--   if key ~= '0' then
--     table.insert(sizes, value)
--   end
-- end
--
--
-- -- lib.print(boxes)
-- table.sort(sizes)
-- vim.print(sizes)
-- -- vim.print(sizes)
-- --
-- local last_index = #sizes
-- result = sizes[last_index] * sizes[last_index - 1] * sizes[last_index - 2]
-- --
-- -- -- for key, value in pairs(circuit_counts) do
-- -- --   print(key, value)
-- -- -- end
--
print('result:', result)
vim.fn.setreg('+', result)
