local lib = require('lib')
local elves_count = tonumber(lib.getLines()[1])

local first = {}
local previous = {}
local test = {}
for i = 1, elves_count do
  local elf = { id = i, previous = nil, next = nil, gifts = 1 }
  table.insert(test, elf)
  if i == 1 then
    first = elf
  elseif i == elves_count then
    previous.next = elf
    elf.previous = previous
    elf.next = first
    first.previous = elf
  else
    elf.previous = previous
    previous.next = elf
  end
  previous = elf
end

local function part()
  local elf = first
  while true do
    elf.gifts = elf.gifts + elf.next.gifts
    if elf.gifts == elves_count then
      print(elf.id)
      break
    end
    local elf_to_remove = elf.next
    local elf_after_the_one_to_remove = elf_to_remove.next
    elf.next = elf_after_the_one_to_remove
    elf_after_the_one_to_remove.previous = elf
    elf = elf.next
  end
end

local elves_left_count = elves_count
local theif = first
local victim = theif
local steps_to_victim = 0
local directly = true
if elves_left_count % 2 == 0 then
  -- directly across
  steps_to_victim = elves_left_count / 2
else
  -- a bit to left
  directly = false
  steps_to_victim = (elves_left_count - 1) / 2
end
for _ = 1, steps_to_victim do
  victim = victim.next
end

local function part2()
  while true do
    --print('theif ' .. theif.id .. ' steals from victim ' .. victim.id)
    theif.gifts = theif.gifts + victim.gifts
    if theif.gifts == elves_count then
      print(theif.id)
      break
    end
    local new_victim = {}
    if directly then
      new_victim = victim.next
      directly = false
    else
      new_victim = victim.next.next
      directly = true
    end
    -- remove
    victim.next.previous = victim.previous
    victim.previous.next = victim.next
    elves_left_count = elves_left_count - 1
    theif = theif.next
    victim = new_victim
  end
end

part2()
