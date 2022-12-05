module Day5

open System.Collections.Generic
open Day5_input
open System.Text.RegularExpressions


type Move = {
    Count: int;
    From: int;
    To: int
}

type SupplyStack = {
    Index: int;
    Crates: Stack<char>
}

let parsePosition (line: string) = 
    let startIndex = 1;
    let increment = 4;
    let indexes = seq { startIndex .. increment .. line.Length }
    seq {
        for i in indexes do
            yield line[i]
    }

let parsePositions (stacks: array<SupplyStack>) (positions: array<string>) = 
    let inputs = positions[..positions.Length-2] |> Array.rev |> Array.map parsePosition
    for n in 0..(inputs.Length-1) do 
        let input = Array.ofSeq inputs[n]
        for i in 0..(input.Length-1) do 
            if (input[i] <> ' ') then stacks[i].Crates.Push(input[i])

let parseMove (line: string) = 
    let numbers = Regex.Matches(line, "[0-9]+")
    {Count = (numbers[0].Value |> int); From = (numbers[1].Value |> int); To = (numbers[2].Value |> int) }

let move (stacks: array<SupplyStack>) (moves: array<Move>) = 
    for move in moves do
        let fromStack = stacks[move.From - 1]
        let toStack = stacks[move.To - 1]
        for i in 1..move.Count do 
            let crate = fromStack.Crates.Pop()
            toStack.Crates.Push(crate)

let move2 (stacks: array<SupplyStack>) (moves: array<Move>) = 
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

let run () = 
    let test = false
    let size = if test then 3 else 9
    let positions = if test then testStartingPositions else startingPositions
    let moves = if test then Day5_input.testMoves else Day5_input.moves

    let stacks = Array.ofSeq (seq { for i in 0 .. (size-1) do yield {Index = i; Crates = new Stack<char>() } })
    parsePositions stacks positions |> ignore
    let moves = Array.map parseMove moves
    move2 stacks moves
    let result = Array.fold (fun state item -> $"{state}{item.Crates.Peek()}") "" stacks
    result