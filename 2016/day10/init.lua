local lib = require 'lib'
local lines = lib.getLines()

local bots = {}
local bots_to_process = {}
for i = 1, #lines do
  local line = lines[i]
  local input_match = string.gmatch(line, "goes to bot")
  if input_match() then
    local input_match = string.gmatch(line, "%d+")
    local value = tonumber(input_match())
    local bot_nr = tonumber(input_match())
    if bots[bot_nr] then
      table.insert(bots[bot_nr].content, value)
      if #bots[bot_nr].content > 1 then
        table.insert(bots_to_process, bot_nr)
      end
    else
      local bot = { nr = bot_nr, content = { value }, low_bot = nil, high_bot = nil, low_output = nil, high_output = nil }
      bots[bot_nr] = bot
    end
  else
    local aa = string.gmatch(line, "%d+")
    local bot_nr = tonumber(aa())
    local bot = {}
    if bots[bot_nr] then
      bot = bots[bot_nr]
    else
      bot = { nr = bot_nr, content = {}, low_bot = nil, high_bot = nil, low_output = nil, high_output = nil }
      bots[bot_nr] = bot
    end

    local low_output = string.match(line, "low to output (%d+)")
    if low_output then
      bot.low_output = tonumber(low_output)
    end

    local high_output = string.match(line, "high to output (%d+)")
    if high_output then
      bot.high_output = tonumber(high_output)
    end
    local high_bot = string.match(line, "high to bot (%d+)")
    if high_bot then
      bot.high_bot = tonumber(high_bot)
    end

    local low_bot = string.match(line, "low to bot (%d+)")
    if low_bot then
      bot.low_bot = tonumber(low_bot)
    end
  end
end

local outputs = {}
local new_bots_to_process = {}
while true do
  for _, bot_nr in ipairs(bots_to_process) do
    local bot = bots[bot_nr]
    if bot == nil then
      print('?????????')
      return
    end
    local low_value
    local high_value
    if bot.content[1] > bot.content[2] then
      low_value = bot.content[2]
      high_value = bot.content[1]
    else
      low_value = bot.content[1]
      high_value = bot.content[2]
    end
    local low_bot = bots[bot.low_bot]
    local high_bot = bots[bot.high_bot]
    if low_bot then
      table.insert(low_bot.content, low_value)
      if #(low_bot.content) > 1 then
        table.insert(new_bots_to_process, low_bot.nr)
      end
    end
    if high_bot then
      table.insert(high_bot.content, high_value)
      if #(high_bot.content) > 1 then
        table.insert(new_bots_to_process, high_bot.nr)
      end
    end

    if bot.low_output == 0 then outputs[1] = low_value end
    if bot.low_output == 1 then outputs[2] = low_value end
    if bot.low_output == 2 then outputs[3] = low_value end
    if bot.high_output == 0 then outputs[1] = high_value end
    if bot.high_output == 1 then outputs[2] = high_value end
    if bot.high_output == 2 then outputs[3] = high_value end
    bot.content = {}
  end

  bots_to_process = new_bots_to_process
  new_bots_to_process = {}
  if #bots_to_process == 0 then
    break
  end
end

local result = 1
for i = 1, #outputs do
  result = result * outputs[i]
end
print(result)
