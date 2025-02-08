local M = {}

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
return M
