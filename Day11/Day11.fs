module Day11

open System.Collections.Generic

type MonkeyID = {
    Id: int
}

type WorryLevel = {
    Value: int
}

type Monkey = {
    Id: MonkeyID
    Items: List<int>
    Action: WorryLevel -> (MonkeyID * WorryLevel)
}

let operation sign (value1: int) value2 =
    if sign = '*' then value1 * value2 else value1 + value2

let bla sign operationValue worryLevel =
    if operationValue = "old" then operation sign worryLevel worryLevel else operation sign (operationValue |> int) worryLevel

let executeTurn operation testCondition trueCondition falseCondition worryLevel =
    let newValue = ((float) (operation worryLevel.Value)) / 3.0 |> floor |> int
    let nextMonkey = if newValue % testCondition = 0 then {Id = trueCondition} else {Id = falseCondition}
    (nextMonkey, {Value = newValue})

let getMonkeys input =
    let mutable monkeyIndex = 0
    let mutable items = List<int>()
    let mutable testCondition = 0
    let mutable operation = (fun a -> a)
    let mutable trueCondition = 0
    let mutable falseCondition = 0
    let monkeys = new List<Monkey>()
    for line in input do
        if line = "" then
            let action = executeTurn operation testCondition trueCondition falseCondition
            let monkey = {Id = {Id = monkeyIndex}; Action = action; Items = items}
            monkeys.Add(monkey)
            monkeyIndex <- monkeyIndex + 1
        else if "Monkey" = line.Substring(0, 6) then
            monkeyIndex <- line.Substring(7,1) |> int
        else if "  Starting items: " = line.Substring(0, 18) then
            let a = line.Substring(17).Split(',')
            let b = Array.map (fun (s: string) -> s.TrimEnd() |> int) a
            items <- new List<int>(b)
        else if "  Test: divisible by " = line.Substring(0, 21) then
            let a = line.Substring(20)
            testCondition <- a |> int
        else if "  Operation: new = old " = line.Substring(0, 23) then
            let a = line.Substring(23).Split( )
            let sign = a[0]
            let operationValue = a[1]
            operation <- bla sign[0] operationValue
        else if "    If true: throw to monkey " = line.Substring(0, 29) then
            let a = line.Substring(28)
            trueCondition <- a |> int
        else if "    If false: throw to monkey " = line.Substring(0, 30) then
            let a = line.Substring(29)
            falseCondition <- a |> int
    let action = executeTurn operation testCondition trueCondition falseCondition
    let monkey = {Id = {Id = monkeyIndex}; Action = action; Items = items}
    monkeys.Add(monkey)
    monkeys
   
let findMostActive activities =
    let mutable mostActive = 0
    let mutable secondMostActive = 0
    
    for activity in activities do
        let value = activity
        if value > mostActive then
            secondMostActive <- mostActive
            mostActive <- value
        else if value> secondMostActive then
            secondMostActive <- value
        else ()        
    mostActive * secondMostActive
    

let run (input: array<string>) =
    let monkeys = getMonkeys input
    let activities = new Dictionary<MonkeyID, int>()
    for monkey in monkeys do
        activities.Add(monkey.Id, 0)
    for i in 0..19 do
        for monkey in monkeys do
            while monkey.Items.Count > 0 do 
                let item = monkey.Items[0]
                monkey.Items.RemoveAt(0)
                activities[monkey.Id] <- activities[monkey.Id] + 1
                let (nextMonkey, newWorryLevel) = monkey.Action {Value = item}
                monkeys[nextMonkey.Id].Items.Add(newWorryLevel.Value)
    
    findMostActive activities.Values