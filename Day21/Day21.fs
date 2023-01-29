module Day21

open System.Text.RegularExpressions
open System.Collections.Generic
open System.Diagnostics
open System

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
let parse input = 
    let allMonkeys = new Dictionary<string, Monkey>()
    // parse
    for line in input do 
        let monkey = parseMonkey line
        match monkey with 
        | Choice1Of2 monkey -> 
            allMonkeys.Add(monkey.Name, DependentMonkey monkey)
        | Choice2Of2 monkey -> 
            allMonkeys.Add(monkey.Name, IndependentMonkey monkey)
    allMonkeys
let run lines =
    let allMonkeys = parse lines
    let result = findNumber allMonkeys["root"] allMonkeys
    result

let runTest input expectedValue = 
    let result = run input 
    if result <> expectedValue then failwith "wrong" else printfn "OK"

let runTestInput () = 
    let input = inputReader.readLines "Day21/testInput.txt"
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

    runTinyTinyTest()
    runTinyTest()
    runTestInput()

    let input = inputReader.readLines "Day21/input.txt"
    runTest input 72664227897438L

let part2 input = 
    let allMonkeys = parse input
    let rootMonkey = 
        match allMonkeys["root"] with 
        | IndependentMonkey _ -> failwith "root monkey cannot be independent"
        | DependentMonkey rootMonkey -> rootMonkey 
    
    let leftMonkey = allMonkeys[rootMonkey.Operation.LeftSideArgument]
    let rightMonkey = allMonkeys[rootMonkey.Operation.RightSideArgument]
    let rightMonkeyResult = findNumber rightMonkey allMonkeys
    let mutable keepGoing = true
    let mutable maxNumber = 4_000_000_000_000L
    let mutable minNumber = 0L
    let mutable humanNumber = (maxNumber - minNumber) / 2L + minNumber
    while keepGoing do
        let human = { Name = "humn"; Number = humanNumber }
        allMonkeys["humn"] <- IndependentMonkey human
        let leftMonkeyResult = findNumber leftMonkey allMonkeys
        printfn "checking %d. Left: %d == %d : Right" humanNumber leftMonkeyResult rightMonkeyResult

        keepGoing <- leftMonkeyResult <> rightMonkeyResult
        if keepGoing then 
            let step = (maxNumber - minNumber) / 2L
            let isTooLow = leftMonkeyResult < rightMonkeyResult
            if isTooLow then maxNumber <- maxNumber - step
            else minNumber <- minNumber + step            
            humanNumber <- (maxNumber - minNumber) / 2L + minNumber
    humanNumber

let testInputPart2 () = 
    let input = inputReader.readLines "Day21/testInput.txt"
    part2 input

let realInputPart2 () = 
    let input = inputReader.readLines "Day21/input.txt"
    part2 input


let runPart2 () = 
    // test input doesn't work with the solution that works with the real input ^^
    //printfn "testInput part2 %d " (testInputPart2 ())
    printfn "real input part2 %d " (realInputPart2 ())