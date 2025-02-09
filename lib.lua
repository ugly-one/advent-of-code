local M = {}

local function print(table)
  vim.api.nvim_command("botright vnew")
  local buffer = vim.api.nvim_get_current_buf()
  vim.api.nvim_buf_set_lines(buffer, 0, -1, true, table)
end

local function getLines(fileName)
  local str = debug.getinfo(2, "S").source:sub(2)
  if not fileName then fileName = "input.txt" end
  local path = str:match("(.*/)") .. fileName
  local file = assert(io.open(path, 'r'))

  local lines = {}
  while true do
    local line = file:read('l')
    if not line then return lines end
    table.insert(lines, line)
  end
end

M.getLines = getLines
M.print = print
return M
