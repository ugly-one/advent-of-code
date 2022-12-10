module advent_of_code_2022.Day10.Day10

open System.Collections.Generic
open System.Text.RegularExpressions

let testInputFile = "Day10/testInput.txt"
let inputFile = "Day10/input.txt"

let interestingCycles () =
    let result = new Stack<int>()
    let interestingCycles = seq {220 .. -40 .. 20}
    for cycle in interestingCycles do
        result.Push(cycle)
    result

let run_private (input: array<string>) action =
    let mutable cycle = 0
    let mutable registerValue = 1
    let mutable keepLooping = true
    let mutable index = 0
    while keepLooping do
        let line = input[index]
        match line with
        | "noop" ->
            cycle <- cycle + 1
            keepLooping <- action cycle registerValue
        | x ->
            let _match = Regex.Match(x, "([-0-9]+)")
            let number = _match.Value |> int
            cycle <- cycle + 1
            keepLooping <- action cycle registerValue 
            cycle <- cycle + 1
            keepLooping <- action cycle registerValue 
            registerValue <- registerValue + number
        index <- index + 1

let onEachCycleAction_part1 (interestingCycles: Stack<int>) (result: List<(int*int)>) cycle registerValue =
    if (interestingCycles.Count = 0) then false else
        let interestingCycle = interestingCycles.Peek()
        if (cycle = interestingCycle) then
            result.Add((registerValue, interestingCycle))
            interestingCycles.Pop () |> ignore
            true
        else true
    
let run_part1 (input: array<string>) =
    let result = new List<(int*int)>()
    let interestingCycles = interestingCycles ()
    let action = onEachCycleAction_part1 interestingCycles result
    run_private input (action)
    Array.fold (fun state (value,cycle) -> state + value*cycle) 0 (result.ToArray())
   
let onEachCycleAction_part2 (result: char[]) cycle register =
    let position = cycle % 40 - 1
    if register - 1 = position || register = position || register + 1 = position then result[cycle-1] <- '#' else result[cycle-1] <- '.'
    if cycle = 240 then false else true
    
let run_part2 (input: array<string>) =
    let result = Array.init 240 (fun i -> ' ')
    let action = onEachCycleAction_part2 result
    run_private input action
    for i in 0..239 do
        printf "%c" result[i]
        if (i + 1) % 40 = 0 
        then
            printfn ""
        else ()
    ()
    
let test_part1 (input: array<string>) =
    let result = run_part1 input
    printfn "%d" result
    if result = 12560 then true else failwith "something doesn't work"