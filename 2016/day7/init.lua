local data = require 'lib'.getLines()

local function get(line)
  local aba_outside = {}
  local aba_inside = {}
  local window_size = 3
  for i = 1, #line do
    local window = string.sub(line, i, i + window_size - 1)
    if string.sub(window, 1, 1) == string.sub(window, 3, 3)
        and not (string.sub(window, 1, 1) == string.sub(window, 2, 2))
        and not (string.sub(window, 2, 2) == '[' or string.sub(window, 2, 2) == ']') then
      local within = false
      for k = i - 1, 1, -1 do
        -- check if we have opening bracket
        local char = string.sub(line, k, k)
        if char == '[' then
          -- check if we have closing bracket
          for l = i + 3, #line do
            local char = string.sub(line, l, l)
            if char == ']' then
              within = true
              break
            end
          end
        elseif char == ']' then
          break
        end
      end
      if not within then
        table.insert(aba_outside, window)
      else
        table.insert(aba_inside, window)
      end
    end
  end
  return { outside = aba_outside, inside = aba_inside }
end

local function supports_ssl(line)
  local result = get(line)
  for _, aba in ipairs(result.outside) do
    for _, bab in ipairs(result.inside) do
      if string.sub(aba, 1, 1) == string.sub(bab, 2, 2) and string.sub(aba, 2, 2) == string.sub(bab, 1, 1) then
        return true
      end
    end
  end
  return false
end

local sum = 0
for _, line in ipairs(data) do
  if supports_ssl(line) then
    sum = sum + 1
  end
end
print(sum)
