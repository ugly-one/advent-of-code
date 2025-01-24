local file = io.open("./testInput.txt", "r")
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
            for i in pages do
                t[#t + 1] = tonumber(i)
            end
            table.insert(testCases, t)
        end
    end
    content = file:read("L")
end

local testCase = testCases[1]
local firstPage = testCase[1]
local secondPage = testCase[2]
print(firstPage)
print(secondPage)

for i, rule in pairs(rules) do
    print('checking: ', firstPage, ' ', secondPage, ' against rule: ', rule.before, ' ', rule.after)
    if rule.before == firstPage and rule.after == secondPage then
        print('found matching rule')
        break
    end
    if rule.before == secondPage and rule.after == firstPage then print('rule broken') end
end
