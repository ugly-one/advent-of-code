local lib = require 'lib'
local data = lib.getLines()

local function get(line)
  local aba_outside = {}
  local aba_inside = {}
  local window_size = 3
  local line_table = lib.to_table(line)
  for i = 1, #line do
    local window = lib.sub(line_table, i, i + window_size - 1)
    if window[1] == window[3]
        and not (window[1] == window[2])
        and not (window[2] == '[' or window[2] == ']') then
      local within = false
      for k = i - 1, 1, -1 do
        -- check if we have opening bracket
        local char = line_table[k]
        if char == '[' then
          -- check if we have closing bracket
          for l = i + 3, #line do
            local char = line_table[l]
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
      if aba[1] == bab[2] and aba[2] == bab[1] then
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
