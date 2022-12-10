module Day5

open System.Collections.Generic
open System.Text.RegularExpressions

type private Move = {
    Count: int;
    From: int;
    To: int
}

type private SupplyStack = {
    Crates: Stack<char>
}

let private parsePosition (line: string) = 
    seq{
        for i in seq { 1 .. 4 .. line.Length } do 
            yield line[i]
    }

let private parseMove (line: string) = 
    let numbers = Regex.Matches(line, "[0-9]+")
    {Count = (numbers[0].Value |> int); From = (numbers[1].Value |> int); To = (numbers[2].Value |> int) }
    
let private createStacks size (positions: array<string>) = 
    let stacks = Array.ofSeq (seq { for i in 0 .. (size-1) do yield { Crates = new Stack<char>() } })
    let inputs = positions[..positions.Length-2] |> Array.rev |> Array.map parsePosition
    for n in 0..(inputs.Length-1) do 
        let input = Array.ofSeq inputs[n]
        for i in 0..(input.Length-1) do 
            if (input[i] <> ' ') then stacks[i].Crates.Push(input[i])
    stacks

let private createMoves moves = 
     Array.map parseMove moves

let private moveOneCrateAtTheTime (stacks: array<SupplyStack>) (moves: array<Move>) = 
    for move in moves do
        let fromStack = stacks[move.From - 1]
        let toStack = stacks[move.To - 1]
        for i in 1..move.Count do 
            let crate = fromStack.Crates.Pop()
            toStack.Crates.Push(crate)

let private moveMultipleCratesAtTheTime (stacks: array<SupplyStack>) (moves: array<Move>) = 
    for move in moves do
        let fromStack = stacks[move.From - 1]
        let toStack = stacks[move.To - 1]
        let temporaryStorage = new Stack<char>()
        for i in 1..move.Count do 
            let crate = fromStack.Crates.Pop()
            temporaryStorage.Push(crate)
        for i in 1..temporaryStorage.Count do 
            let crate = temporaryStorage.Pop()
            toStack.Crates.Push(crate)

let run2 () = 
    let test = false
    let size = if test then 3 else 9
    let positions = if test then Day5_input.testStartingPositions else Day5_input.startingPositions
    let moves = if test then Day5_input.testMoves else Day5_input.moves
    
    let stacks = createStacks size positions
    let moves = createMoves moves
    moveMultipleCratesAtTheTime stacks moves
    let result = Array.fold (fun state item -> $"{state}{item.Crates.Peek()}") "" stacks
    result