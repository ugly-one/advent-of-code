local lines = require('lib').getLines()

local result = {}
local function isValid(lines)
  local state = {
    byr = false,
    iyr = false,
    eyr = false,
    hgt = false,
    hcl = false,
    ecl = false,
    pid = false,
    cid = true
  }
  for i, line in ipairs(lines) do
    local match = string.gmatch(line, "([^%s]+)")
    for m in match do
      local a = string.gmatch(m, "([^:]+)")
      local key = a()
      local value = a()
      if key == 'byr' then
        local n = tonumber(value)
        if not (n == nil) and n >= 1920 and n <= 2002 then state[key] = true end
      elseif key == 'iyr' then
        local n = tonumber(value)
        if not (n == nil) and n >= 2010 and n <= 2020 then state[key] = true end
      elseif key == 'eyr' then
        local n = tonumber(value)
        if not (n == nil) and n >= 2020 and n <= 2030 then state[key] = true end
      elseif key == 'hgt' then
        local length = #value
        local unit = string.sub(value, length - 1, length)
        local num = tonumber(string.sub(value, 0, length - 2))
        if num == nil then
        else
          if unit == 'cm' then
            if num >= 150 and num <= 193 then state[key] = true end
          elseif unit == 'in' then
            if num >= 59 and num <= 76 then state[key] = true end
          end
        end
      elseif key == 'hcl' then
        local e = string.match(value,
          "^#[0123456789abcdef][0123456789abcdef][0123456789abcdef][0123456789abcdef][0123456789abcdef][0123456789abcdef]$")
        if e then state[key] = true end
      elseif key == 'ecl' then
        if value == 'amb' or value == 'blu' or value == 'brn' or value == 'gry' or value == 'grn' or value == 'hzl' or value == 'oth' then state[key] = true end
      elseif key == 'pid' then
        local e = string.match(value,
          "^[0123456789][0123456789][0123456789][0123456789][0123456789][0123456789][0123456789][0123456789][0123456789]$")
        if e then state[key] = true end
      end
    end
  end
  for k, v in pairs(state) do
    if not v then
      return false
    end
  end

  return true
end

local valid_sum = 0
local passport = {}
for i, line in ipairs(lines) do
  if line == "" then
    if isValid(passport) then valid_sum = valid_sum + 1 end
    passport = {}
  else
    table.insert(passport, line)
  end
end

if isValid(passport) then valid_sum = valid_sum + 1 end

print(valid_sum)
require('lib').print(result)
