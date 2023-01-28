module Day21

open System.Text.RegularExpressions
open System.Collections.Generic

type MonkeyName = string

type OperationType = 
    | Plus
    | Minus
    | Multiply
    | Divide

type Operation = {
    Type: OperationType
    LeftSideArgument: MonkeyName
    RightSideArgument: MonkeyName
}

type Job =
    | YellNumber of int64
    | YellMathOperation of Operation

type DependentMonkey = {
    Name: MonkeyName
    Operation: Operation
    mutable LeftSideParameter: int64 option
    mutable RightSideParameter: int64 option
}

type IndependentMonkey = {
    Name: MonkeyName
    Number: int64
}

type Monkey = 
    | DependentMonkey of DependentMonkey
    | IndependentMonkey of IndependentMonkey

let parseMonkey line = 
    let mathOperationMatch = Regex.Match(line, "([a-z]+): ([a-z]+) ([+\-*/]) ([a-z]+)")
    if mathOperationMatch.Success then 
        let monkeyName = mathOperationMatch.Groups[1].Value
        let leftSideOperation = mathOperationMatch.Groups[2].Value
        let rightSideOperation = mathOperationMatch.Groups[4].Value
        let operationTypeAsString = mathOperationMatch.Groups[3].Value
        let operationType = 
            match operationTypeAsString with 
            | "+" -> Plus
            | "-" -> Minus
            | "/" -> Divide
            | "*" -> Multiply
            | _ -> failwithf "Unknown operation %s" operationTypeAsString
        let operation = {Type = operationType; LeftSideArgument = leftSideOperation; RightSideArgument = rightSideOperation}
        Choice1Of2 {Name = monkeyName; Operation = operation; LeftSideParameter = None; RightSideParameter = None}
    else 
        let numberMatch = Regex.Match(line, "([a-z]+): ([0-9]+)")
        let monkeyName = numberMatch.Groups[1].Value
        let number = numberMatch.Groups[2].Value |> int64
        Choice2Of2 {Name = monkeyName; Number = number}

let tryApplyIndependentMonkey independentMonkey dependentMonkey = 
    let newMonkey = 
        if dependentMonkey.Operation.LeftSideArgument = independentMonkey.Name then
            dependentMonkey.LeftSideParameter <- Some independentMonkey.Number
            dependentMonkey
        elif dependentMonkey.Operation.RightSideArgument = independentMonkey.Name then
            dependentMonkey.RightSideParameter <- Some independentMonkey.Number
            dependentMonkey
        else 
            dependentMonkey
    if newMonkey.LeftSideParameter.IsNone || newMonkey.RightSideParameter.IsNone then 
        Choice1Of2 newMonkey
    else 
        let number = 
            match newMonkey.Operation.Type with 
            | Plus -> newMonkey.LeftSideParameter.Value + newMonkey.RightSideParameter.Value
            | Minus -> newMonkey.LeftSideParameter.Value - newMonkey.RightSideParameter.Value 
            | Divide -> newMonkey.LeftSideParameter.Value / newMonkey.RightSideParameter.Value 
            | Multiply -> newMonkey.LeftSideParameter.Value * newMonkey.RightSideParameter.Value 
        Choice2Of2 {Name = newMonkey.Name; Number = number}

let rec findNumber (monkey: Monkey) (allMonkeys: Dictionary<string, Monkey>)= 
    match monkey with 
    | IndependentMonkey independentMonkey -> independentMonkey.Number
    | DependentMonkey dependentMonkey -> 
        // find the left number
        let leftNumber = 
            match dependentMonkey.LeftSideParameter with 
            | Some value -> value
            | None -> findNumber allMonkeys[dependentMonkey.Operation.LeftSideArgument] allMonkeys
        // find the right number
        let rightNumber = 
            match dependentMonkey.RightSideParameter with 
            | Some value -> value
            | None -> findNumber allMonkeys[dependentMonkey.Operation.RightSideArgument] allMonkeys
        // do the operation
        let result = 
            match dependentMonkey.Operation.Type with 
            | Plus -> leftNumber + rightNumber
            | Minus -> leftNumber - rightNumber
            | Divide -> leftNumber / rightNumber
            | Multiply -> leftNumber * rightNumber
        result

let run lines =
    let allMonkeys = new Dictionary<string, Monkey>()
    // parse
    for line in lines do 
        let monkey = parseMonkey line
        match monkey with 
        | Choice1Of2 monkey -> 
            allMonkeys.Add(monkey.Name, DependentMonkey monkey)
        | Choice2Of2 monkey -> 
            allMonkeys.Add(monkey.Name, IndependentMonkey monkey)

    let result = findNumber allMonkeys["root"] allMonkeys
    result

let runTest input expectedValue = 
    let result = run input 
    if result <> expectedValue then failwith "wrong" else printfn "OK"

let runTestInput () = 
    let input = inputReader.readLines "Day21/testInput.txt" |> Array.ofSeq
    runTest input 152

let runTinyTest () = 
    let input = [| 
        "root: hmdt - zczc";
        "hmdt: 32";
        "zczc: 2"
    |]
    runTest input 30

let runTinyTinyTest () = 
    let input = [| 
        "root: 99";
    |]
    runTest input 99

let runPart1 () = 
    let input = inputReader.readLines "Day21/input.txt" |> Array.ofSeq
    runTest input 72664227897438L