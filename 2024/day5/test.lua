local file = io.open("./input.txt", "r")
if file == nil then return 1 end

local rules = {}
local testCases = {}
local content = file:read("L")

local addToRules = true
while content do
    if content == '\n' then
        addToRules = false
    else
        if addToRules then
            local pages = string.gmatch(content, "[^|]+")
            local t = {}
            for i in pages do
                t[#t + 1] = i
            end
            local beforePage = tonumber(t[1])
            local afterPage = tonumber(t[2])
            table.insert(rules, { before = beforePage, after = afterPage })
        else
            local pages = string.gmatch(content, "[^,]+")
            local t = {}
            local size = 0
            for i in pages do
                t[#t + 1] = tonumber(i)
                size = size + 1
            end
            table.insert(testCases, {pages = t, size = size})
        end
    end
    content = file:read("L")
end


local function check(firstPage, secondPage)
    for _, rule in pairs(rules) do
        if rule.before == firstPage and rule.after == secondPage then
            return true;
        end
        if rule.before == secondPage and rule.after == firstPage then
            return false;
        end
    end
end

local function checkTestCase(testCase)
    local totalResult = true
    for i, page in pairs(testCase) do
        for j, anotherPage in pairs(testCase) do
            if i ~= j then
                local firstPage;
                local secondPage;
                if (i < j) then
                    firstPage = page
                    secondPage = anotherPage
                end
                if (i > j) then
                    firstPage = anotherPage
                    secondPage = page
                end

                local result = check(firstPage, secondPage)
                --print(firstPage, secondPage, result)
                if result == false then
                    totalResult = false
                    break
                end
            end
        end
        if totalResult == false then break end
    end
    return totalResult
end

local correctUpdatesSum = 0
local wrongUpdatesSum = 0
for i, testCase in pairs(testCases) do
    local result = checkTestCase(testCase.pages)
    if result == true then
        local middleIndex = testCase.size / 2 + 0.5
        correctUpdatesSum = correctUpdatesSum + testCase.pages[middleIndex]
    else
        table.sort(testCase.pages, check)
        local string1 = ''
        for i,n in ipairs(testCase.pages) do string1 = string1 .. ',' .. n end
        local middleIndex = testCase.size / 2 + 0.5
        wrongUpdatesSum = wrongUpdatesSum + testCase.pages[middleIndex]
    end

end

print(correctUpdatesSum)
print(wrongUpdatesSum)
