local lib = require('lib')
local input = lib.getLines()

local state_line = lib.to_table(string.match(input[1], "initial state: ([%.#]+)"))
local rules = {}
for i = 3, #input do
  local rule, outcome = string.match(input[i], "([%.#]+) => ([%.#])")
  table.insert(rules, { rule = rule, outcome = outcome })
end

local min = 0
local max = 0
local state = {}
for i, pot in ipairs(state_line) do
  if pot == '#' then
    state[i - 1] = pot
    max = i - 1
  end
end

local previous_counter = 0
local previous_diff = 0
local generations = 50000000000
for i = 1, generations do
  local new_state = {}
  for pos = min - 2, max + 2 do
    -- find matching rule
    local pots = ""
    for a = pos - 2, pos + 2 do
      if state[a] == '#' then
        pots = pots .. "#"
      else
        pots = pots .. '.'
      end
    end
    local outcome = nil
    for _, rule in ipairs(rules) do
      if rule.rule == pots then
        outcome = rule.outcome
      end
    end
    if outcome == nil then
      -- test data does not have rules which has . as outcome
      outcome = '.'
      -- when running against real input, would be nice to not fix missing data
      -- but to raise an error
    end
    if outcome == "#" then
      new_state[pos] = outcome
    else
      new_state[pos] = nil
    end
  end
  for k, v in pairs(new_state) do
    if k < min then min = k end
    if k > max then max = k end
  end
  state = new_state
  local counter = 0
  for id, pot in pairs(state) do
    if pot == '#' then
      counter = counter + id
    end
  end
  if previous_diff == counter - previous_counter then
    local remaining_generations = generations - i
    print(counter + remaining_generations * previous_diff)
    break
  end

  previous_diff = counter - previous_counter
  previous_counter = counter
end
