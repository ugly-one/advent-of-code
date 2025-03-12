local function part1()
  local b = 67
  local c = b
  local a = 0
  local d = 0
  local e = 0
  local h = 0

  local counter = 0
  if a ~= 0 then
    b = b * 100
    b = b + 100000
    c = b
    c = c + 17000
  end
  repeat
    d = 2
    local found = false
    repeat
      e = 2
      repeat
        counter = counter + 1
        if d * e == b then
          found = true
        end
        e = e + 1
      until e == b or found
      d = d + 1
    until d == b or found
    if found then
      h = h + 1
    end
    if b == c then
      break
    else
      b = b + 17
    end
  until false

  print(counter)
end

local function part2()
  local b = 67
  local c = b
  local a = 1
  local d = 0
  local e = 0
  local h = 0

  local counter = 0
  if a ~= 0 then
    b = b * 100
    b = b + 100000
    c = b
    c = c + 17000
  end
  repeat
    d = 2
    local found = false
    repeat
      e = 2
      -- the below loop can be replaced with this simple check
      if b % d == 0 then
        found = true
      end
      --repeat
      --  if d * e == b then
      --    found = true
      --    print(b, c, d, e)
      --  end
      --  e = e + 1
      --until e == b or found
      d = d + 1
    until d == b or found
    if found then
      h = h + 1
    end
    if b == c then
      break
    else
      b = b + 17
    end
  until false

  print(h)
end

part1()
part2()
