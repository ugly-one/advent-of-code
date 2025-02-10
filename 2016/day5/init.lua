require('md5')
local door_id = "abc"
local index = 3231929
local pass = ""
while false do
  local value_to_hash = door_id .. index
  local handle = io.popen("echo -n '" .. value_to_hash .. "' | md5sum")
  local result = handle:read("*a")
  handle:close()
  if string.sub(result, 1, 5) == "00000" then
    pass = pass .. string.sub(result, 6, 6)
    print(pass)
  end
  index = index + 1
  if index % 1000000 == 0 then print(index) end
end
