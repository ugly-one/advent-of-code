local lib = require('lib')

local input = lib.getLines()
local initial_molecule = ""
local replacements = {}
for _, line in ipairs(input) do
  if line ~= "" then
    local replacement = string.match(line, "=>")
    if replacement then
      local parts = string.gmatch(line, "(%a+)")
      local source = parts()
      local dest = parts()
      local size = #lib.to_table(source)
      local dest_size = #lib.to_table(dest)
      table.insert(replacements, { source = source, source_size = size, dest = dest, dest_size = dest_size })
    else
      initial_molecule = line
    end
  end
end

local function part1()
  local function try_replace(molecule_string, molecule_array, replacement)
    local source_size = replacement.source_size
    local results = {}
    for i = 1, #molecule_array do
      local window = string.sub(molecule_string, i, i + source_size - 1)
      if window == replacement.source then
        local result = string.sub(molecule_string, 1, i - 1) ..
            replacement.dest .. string.sub(molecule_string, i + source_size)
        table.insert(results, result)
      else
      end
    end
    return results
  end

  local molecules = {}
  local molecule_array = lib.to_table(initial_molecule)
  for _, replacement in ipairs(replacements) do
    local results = try_replace(initial_molecule, molecule_array, replacement)
    for _, result in ipairs(results) do
      molecules[result] = true
    end
  end

  local sum = 0
  for k, v in pairs(molecules) do
    sum = sum + 1
  end
  print(sum)
end

--part1()

local function part2()
  local function try_replace(molecule_string, molecule_array, replacement)
    local dest_size = replacement.dest_size
    local results = {}
    for i = 1, #molecule_array do
      local window = string.sub(molecule_string, i, i + dest_size - 1)
      if window == replacement.dest then
        local result = string.sub(molecule_string, 1, i - 1) ..
            replacement.source .. string.sub(molecule_string, i + dest_size)
        table.insert(results, result)
      else
      end
    end
    return results
  end

  local target_molecule = 'e'
  local min = nil
  local function find_min(molecule_string, molecule_array, steps)
    if min ~= nil and steps > min then return end
    if molecule_string == target_molecule then
      if min == nil or steps < min then
        min = steps
        print(min)
      end
    end
    for _, replacement in ipairs(replacements) do
      local result = try_replace(molecule_string, molecule_array, replacement)
      for _, option in ipairs(result) do
        find_min(option, lib.to_table(option), steps + 1)
      end
    end
  end

  find_min(initial_molecule, lib.to_table(initial_molecule), 0)
  print(min)
end

part2() -- it prints correct result but never finishes...
