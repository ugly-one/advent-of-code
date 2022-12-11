module Day11

open System
open System.Collections.Generic

type MonkeyID = {
    Id: int
}

type WorryLevel = {
    Part1: uint64[]
    Value: uint64
}

type Monkey = {
    Id: MonkeyID
    Items: List<WorryLevel>
    Action: WorryLevel -> (MonkeyID * WorryLevel)
}

let executeOperation sign operationValue (worryLevel: WorryLevel) =
    if operationValue = "old" && sign = '*' then worryLevel
    else if operationValue = "old" && sign = '+' then {Part1 = Array.append worryLevel.Part1 [|2UL|]; Value = worryLevel.Value * 2UL}
    else if sign = '*' then {Part1 = Array.append worryLevel.Part1 [|operationValue |> uint64|]; Value = worryLevel.Value * (operationValue |> uint64)}
    else {worryLevel with Value = worryLevel.Value + (operationValue |> uint64)}

let check (worryLevel: WorryLevel) (testCondition: uint64) =
    true // TODO
    
let executeTurn operation testCondition trueCondition falseCondition action worryLevel =
    let newWorryLevel = operation worryLevel
    let newValue = action newWorryLevel
    let nextMonkey = if check newValue testCondition then {Id = trueCondition} else {Id = falseCondition}
    (nextMonkey, newValue)

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
            let c = Array.map (fun s -> {Value = s; Part1 = Array.Empty()}) b
            items <- new List<WorryLevel>(c)
        else if "  Test: divisible by " = line.Substring(0, 21) then
            let a = line.Substring(20)
            testCondition <- a |> uint64
        else if "  Operation: new = old " = line.Substring(0, 23) then
            let a = line.Substring(23).Split( )
            let sign = a[0]
            let operationValue = a[1]
            operation <- executeOperation sign[0] operationValue
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
        let value = ((float) newWorryLevel.Value) / 3.0 |> floor |> uint64
        {Value = value; Part1 = Array.Empty()}
    run input calmDown 20
    
let run2 input rounds =  
    let calmDown newWorryLevel = newWorryLevel
    run input calmDown rounds