local DIRECTION = { UP = 1, DOWN = 2, LEFT = 3, RIGHT = 4 }
local function printMap(map)
    for a, b in ipairs(map) do
        local lineString = ""
        for a, c in ipairs(b) do
            lineString = lineString .. c
        end
        print(lineString)
    end
end

local function move(position, direction)
    if direction == DIRECTION.UP then return { X = position.X, Y = position.Y - 1 } end
    if direction == DIRECTION.DOWN then return { X = position.X, Y = position.Y + 1 } end
    if direction == DIRECTION.LEFT then return { X = position.X - 1, Y = position.Y } end
    if direction == DIRECTION.RIGHT then return { X = position.X + 1, Y = position.Y } end
end

local function printPos(position)
    print(position.X, position.Y)
end

local function checkHit(map, position)
    if map[position.Y][position.X] == '#' then return true end
    return false
end

local file = io.open("day6/input.txt", "r")
if file == nil then return 1 end

local line = file:read("l")
local size = string.len(line)
local lineNumber = 1

local function withinMap(position)
    if position.X < 1 then
        return false
    elseif position.Y < 1 then
        return false
    elseif position.X > size then
        return false
    elseif position.Y > size then
        return false
    else
        return true
    end
end

local map = {}

local guard = {}
while line do
    map[lineNumber] = {}
    for i = 1, #line do
        local c = line:sub(i, i)
        map[lineNumber][i] = c
        if c == '^' then
            guard.position = { X = i, Y = lineNumber }
            guard.direction = DIRECTION.UP
        elseif c == '>' then
            guard.position = { X = i, Y = lineNumber }
            guard.direction = DIRECTION.RIGHT
        elseif c == '<' then
            guard.position = { X = i, Y = lineNumber }
            guard.direction = DIRECTION.LEFT
        elseif c == 'v' then
            guard.position = { X = i, Y = lineNumber }
            guard.direction = DIRECTION.DOWN
        end
    end
    lineNumber = lineNumber + 1
    line = file:read("l")
end

local function rotate(direction)
    if direction == DIRECTION.UP then return DIRECTION.RIGHT end
    if direction == DIRECTION.DOWN then return DIRECTION.LEFT end
    if direction == DIRECTION.LEFT then return DIRECTION.UP end
    if direction == DIRECTION.RIGHT then return DIRECTION.DOWN end
end

local function part1()
    local fieldsVisited = { [guard.position.X .. '.' .. guard.position.Y] = 1 }
    while true do
        local newPosition = move(guard.position, guard.direction)
        if not withinMap(newPosition) then break end
        if checkHit(map, newPosition) then
            guard.direction = rotate(guard.direction)
        else
            guard.position = newPosition
            fieldsVisited[guard.position.X .. '.' .. guard.position.Y] = 1
        end
    end

    local count = 0
    for i, a in pairs(fieldsVisited) do
        count = count + 1
    end

    print(count)
end

local function copyMap(table)
    local result = {}
    for k, v in pairs(table) do
        if type(v) == "string" then
            result[k] = v
        else
            result[k] = copyMap(v)
        end
    end
    return result
end

local function copyGuard(guard)
    return { position = { X = guard.position.X, Y = guard.position.Y }, direction = guard.direction }
end

local function isInLoop(map, guard)
    local loop = false
    local fieldsVisited = { [guard.position.X .. '.' .. guard.position.Y .. '.' .. guard.direction] = 1 }
    while true do
        local newPosition = move(guard.position, guard.direction)
        if not withinMap(newPosition) then break end
        local characterToPrint = '|'
        if checkHit(map, newPosition) then
            guard.direction = rotate(guard.direction)
            characterToPrint = '+'
        else
            guard.position = newPosition
            if guard.direction == DIRECTION.UP or guard.direction == DIRECTION.DOWN then
                characterToPrint = '|'
            else
                characterToPrint = '-'
            end
        end
        map[guard.position.Y][guard.position.X] = characterToPrint
        if fieldsVisited[newPosition.X .. '.' .. newPosition.Y .. '.' .. guard.direction] == 1 then
            loop = true
            break
        else
            fieldsVisited[guard.position.X .. '.' .. guard.position.Y .. '.' .. guard.direction] = 1
        end
    end
    return loop
end

local count = 0
for i = 1, size do
    for j = 1, size do
        if guard.position.X == j and guard.position.Y == i then
        elseif map[i][j] == '#' then
        else
            local newMap = copyMap(map)
            local newGuard = copyGuard(guard)
            newMap[i][j] = '#'
            if isInLoop(newMap, newGuard) then count = count + 1 end
        end
    end
end
print(count)
