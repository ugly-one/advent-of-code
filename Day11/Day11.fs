module Day11

open System
open System.Collections.Generic

type MonkeyID = {
    Id: int
}

type WorryLevel = {
    Value: uint64
}

type Monkey = {
    Id: MonkeyID
    Items: List<WorryLevel>
    Action: WorryLevel -> (MonkeyID * WorryLevel)
}

let operation sign (value1: uint64) value2 =
    if sign = '*' then value1 * value2 else value1 + value2

let bla sign operationValue worryLevel =
    if operationValue = "old" then operation sign worryLevel worryLevel else operation sign (operationValue |> uint64) worryLevel

let executeTurn operation testCondition trueCondition falseCondition action worryLevel =
    let newWorryLevel = operation worryLevel.Value
    let newValue = action newWorryLevel
    let nextMonkey = if newValue % testCondition = 0UL then {Id = trueCondition} else {Id = falseCondition}
    (nextMonkey, {Value = newValue})

let getMonkeys input calmDown =
    let mutable monkeyIndex = 0
    let mutable items = List<WorryLevel>()
    let mutable testCondition = 0UL
    let mutable operation = (fun a -> a)
    let mutable trueCondition = 0
    let mutable falseCondition = 0
    let monkeys = new List<Monkey>()
    for line in input do
        if line = "" then
            let action = executeTurn operation testCondition trueCondition falseCondition calmDown
            let monkey = {Id = {Id = monkeyIndex}; Action = action; Items = items}
            monkeys.Add(monkey)
            monkeyIndex <- monkeyIndex + 1
        else if "Monkey" = line.Substring(0, 6) then
            monkeyIndex <- line.Substring(7,1) |> int
        else if "  Starting items: " = line.Substring(0, 18) then
            let a = line.Substring(17).Split(',')
            let b = Array.map (fun (s: string) -> s.TrimEnd() |> uint64) a
            let c = Array.map (fun s -> {Value = s}) b
            items <- new List<WorryLevel>(c)
        else if "  Test: divisible by " = line.Substring(0, 21) then
            let a = line.Substring(20)
            testCondition <- a |> uint64
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
    let action = executeTurn operation testCondition trueCondition falseCondition calmDown
    let monkey = {Id = {Id = monkeyIndex}; Action = action; Items = items}
    monkeys.Add(monkey)
    monkeys
   
let findMostActive activities =
    Seq.sort activities |> Seq.rev |> Seq.take 2 |> Seq.fold (fun s i -> s*i) 1  |> uint64
 
let run (input: array<string>) calmDown rounds =
    let monkeys = getMonkeys input calmDown
    let activities = new Dictionary<MonkeyID, int>()
    for monkey in monkeys do
        activities.Add(monkey.Id, 0)
    for i in 1..rounds do
        for monkey in monkeys do
            while monkey.Items.Count > 0 do 
                let item = monkey.Items[0]
                monkey.Items.RemoveAt(0)
                activities[monkey.Id] <- activities[monkey.Id] + 1
                let (nextMonkey, newWorryLevel) = monkey.Action item
                monkeys[nextMonkey.Id].Items.Add(newWorryLevel)
    
    findMostActive activities.Values

let run1 input =  
    let calmDown newWorryLevel =
        ((float) newWorryLevel) / 3.0 |> floor |> uint64
    run input calmDown 20
    
let run2 input rounds =  
    let calmDown newWorryLevel = newWorryLevel
    run input calmDown rounds