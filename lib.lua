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

local function copy_dic(input_table)
  local new_table = {}
  for k, v in pairs(input_table) do
    new_table[k] = v
  end
  return new_table
end

local function to_string(table)
  local line = ""
  for i, c in ipairs(table) do
    line = line .. c
  end
  return line
end

local function getLines(file_name)
  local str = debug.getinfo(2, "S").source:sub(2)
  if not file_name then file_name = "input.txt" end

  local path = str:match("(.*/)")
  local construct_input_path = false
  local get_base_path = true
  local base_path = ""
  if string.sub(str, 1, 1) ~= '/' then
    -- if the path we have is a relative path then we assume that we have to just move
    -- one directory to get to the folder where all inputs are
    -- and from there we can reconstruct the path to the specific input file
    -- this happens when executing lua directly from terminal.
    -- the other case happens when lua is run from neovim and since I want to be able to work from
    -- both places, we have support (hack?) for both
    construct_input_path = true
    get_base_path = false
    base_path = "../"
  end
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
  local entire_path = base_path .. '/' .. input_path .. '/' .. file_name
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

local home = os.getenv("HOME")
package.path = package.path .. ";" .. home .. "/lua/?.lua"
local md5 = require 'md5'
M.md5 = md5
M.getLines = getLines
M.print = _print
M.copy_table = copy_table
M.sort = sort
M.to_table = to_table
M.sub = sub
M.to_bits = to_bits
M.to_string = to_string
M.copy_dic = copy_dic
return M
