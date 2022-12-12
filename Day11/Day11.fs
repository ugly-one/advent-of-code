module Day11

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
    Action: WorryLevel -> uint64 -> (MonkeyID * WorryLevel)
}

let executeOperation sign operationValue (worryLevel: WorryLevel) =
    if operationValue = "old" && sign = '*' then {Value = worryLevel.Value * worryLevel.Value }
    else if operationValue = "old" && sign = '+' then {Value = worryLevel.Value + worryLevel.Value}
    else if sign = '*' then {Value = worryLevel.Value * (operationValue |> uint64)}
    else {worryLevel with Value = worryLevel.Value + (operationValue |> uint64)}

let executeTurn operation calmDownOperation testCondition trueCondition falseCondition worryLevel modulo =
    let newWorryLevel = operation worryLevel
    let newValue = calmDownOperation modulo newWorryLevel 
    let nextMonkey = if newValue.Value % testCondition = 0UL then {Id = trueCondition} else {Id = falseCondition}
    (nextMonkey, newValue)

let getMonkeys input calmDown =
    let mutable monkeyIndex = 0
    let mutable items = List<WorryLevel>()
    let mutable testCondition = 0UL
    let mutable operation = (fun a -> a)
    let mutable trueCondition = 0
    let mutable falseCondition = 0
    let mutable modulo = 1UL
    let monkeys = new List<Monkey>()
    for line in input do
        if line = "" then
            ()
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
            modulo <- modulo * testCondition
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
            // create monkey
            let action = executeTurn operation calmDown testCondition trueCondition falseCondition 
            let monkey = {Id = {Id = monkeyIndex}; Action = action; Items = items}
            monkeys.Add(monkey)
 
    (monkeys, modulo)
   
let findMostActive activities =
    Seq.sort activities |> Seq.rev |> Seq.take 2 |> Seq.fold (fun s i -> s*i) 1  |> uint64
 
let run (input: array<string>) calmDown rounds =
    let (monkeys, modulo) = getMonkeys input calmDown
    let activities = new Dictionary<MonkeyID, int>()
    for monkey in monkeys do
        activities.Add(monkey.Id, 0)
    for i in 1..rounds do
        for monkey in monkeys do
            while monkey.Items.Count > 0 do 
                let item = monkey.Items[0]
                monkey.Items.RemoveAt(0)
                activities[monkey.Id] <- activities[monkey.Id] + 1
                let (nextMonkey, newWorryLevel) = monkey.Action item modulo
                monkeys[nextMonkey.Id].Items.Add(newWorryLevel)
    
    findMostActive activities.Values

let run1 input =  
    let calmDown modulo newWorryLevel =
        let value = ((float) newWorryLevel.Value) / 3.0 |> floor |> uint64
        {Value = value}
    run input calmDown 20
    
let run2 input rounds =  
    let calmDown modulo newWorryLevel = 
        {Value = (newWorryLevel.Value % modulo)}
    run input calmDown rounds