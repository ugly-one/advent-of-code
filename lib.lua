local M = {}

local function _print(table)
  vim.api.nvim_command("botright vnew")
  local buffer = vim.api.nvim_get_current_buf()
  vim.api.nvim_buf_set_lines(buffer, 0, -1, true, table)
end

local function copy_table(input_table)
  local new_table = {}
  for _, a in ipairs(input_table) do
    table.insert(new_table, a)
  end
  return new_table
end

local function getLines(fileName)
  local str = debug.getinfo(2, "S").source:sub(2)
  if not fileName then fileName = "input.txt" end

  local file = 'input.txt'
  local path = str:match("(.*/)")
  local construct_input_path = false
  local get_base_path = true
  local base_path = ""
  local input_repo = "advent-of-code-input"
  local input_path = input_repo
  for folder in string.gmatch(path, "([^/]+)") do
    if folder == 'advent-of-code' then
      construct_input_path = true
      get_base_path = false
    else
      if get_base_path then
        base_path = base_path .. '/' .. folder
      end

      if construct_input_path then
        input_path = input_path .. '/' .. folder
      end
    end
  end
  local entire_path = base_path .. '/' .. input_path .. '/' .. file
  local file = assert(io.open(entire_path, 'r'))

  local lines = {}
  while true do
    local line = file:read('*l')
    if not line then return lines end
    table.insert(lines, line)
  end
end

local function sort(map)
  local sorted = {}
  for key, value in pairs(map) do
    table.insert(sorted, { key = key, value = value })
  end

  table.sort(sorted, function(a, b)
    if a.value == b.value then
      if a.key < b.key then
        return true
      else
        return false
      end
    elseif a.value > b.value then
      return true
    else
      return false
    end
  end)

  return sorted
end

local function to_table(input)
  local t = {}
  for i = 1, #input do
    t[i] = input:sub(i, i)
  end
  return t
end

local function sub(input_table, start, end_)
  local t = {}
  for i = start, end_ do
    table.insert(t, input_table[i])
  end
  return t
end

local function to_bits(num)
  -- returns a table of bits, least significant first.
  local t = {}
  while num > 0 do
    local rest = math.fmod(num, 2)
    t[#t + 1] = rest
    num = (num - rest) / 2
  end
  return t
end

M.getLines = getLines
M.print = _print
M.copy_table = copy_table
M.sort = sort
M.to_table = to_table
M.sub = sub
M.to_bits = to_bits
return M
