local M = {}

local function print(table)
  vim.api.nvim_command("botright vnew")
  local buffer = vim.api.nvim_get_current_buf()
  vim.api.nvim_buf_set_lines(buffer, 0, -1, true, table)
end

local function copy_table(table)
  local new_table = {}
  for _, a in ipairs(table) do
    table.insert(new_table, a)
  end
  return new_table
end

local function getLines(fileName)
  local str = debug.getinfo(2, "S").source:sub(2)
  if not fileName then fileName = "input.txt" end
  local path = str:match("(.*/)") .. fileName
  local file = assert(io.open(path, 'r'))

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

M.getLines = getLines
M.print = print
M.copy_table = copy_table
M.sort = sort
return M
