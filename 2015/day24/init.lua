local lib = require('lib')
local input = lib.getLines()
local presents = {}
for _, line in ipairs(input) do
  table.insert(presents, tonumber(line))
end

local target_weight = lib.sum(presents) / 4

local function get_presents_except(all, table2)
  local result = {}
  for _, item in ipairs(all) do
    local add = true
    for _, item2 in ipairs(table2) do
      if item == item2 then
        add = false
        break
      end
    end
    if add == true then table.insert(result, item) end
  end
  return result
end

local min_count = nil
local min_entanglement = nil

-- TODO [SLOW] this solution is not nice
-- a) it doesn't finish in reasonable time (but one of the prints give correct answer)
-- b) for part 2 I do not check if the third space and the trunk can be ballanced. I assume
-- that if the front and one side are correct weight, the remaining side and the truck can be ballanced
-- but I am not 100% sure that is always correct
local function add_present(presents, start_index, available_weight, front, side, add_to_front)
  if min_count ~= nil and #front > min_count then
  else
    for index = start_index, 1, -1 do
      local present = presents[index]
      if available_weight - present >= 0 then
        if add_to_front then
          table.insert(front, present)
        else
          table.insert(side, present)
        end
        if available_weight - present == 0 then
          if add_to_front == false then
            if min_count == nil then
              min_count = #front
              min_entanglement = lib.aggregate(front, function(a, i) return a * i end, 1)
              print(min_entanglement, min_count)
            else
              if #front < min_count then
                min_count = #front
                min_entanglement = lib.aggregate(front, function(a, i) return a * i end, 1)
                print(min_entanglement, min_count)
              else
                local entanglement = lib.aggregate(front, function(a, i) return a * i end, 1)
                if entanglement < min_entanglement then
                  min_entanglement = entanglement
                  print(min_entanglement, min_count)
                end
              end
            end
          else
            local remaining_presents = get_presents_except(presents, front)
            add_present(remaining_presents, #remaining_presents, target_weight, front, side, false)
          end
        else
          add_present(presents, index - 1, available_weight - present, front, side, add_to_front)
        end
        if add_to_front then
          table.remove(front, #front)
        else
          table.remove(side, #side)
        end
      end
    end
  end
end

add_present(presents, #presents, target_weight, {}, {}, true)
