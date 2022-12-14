module Day14

open System.Text.RegularExpressions

type Position = {
    X: int
    Y: int
}

type Path = {
    Positions: Position array
}

let part1 (input: string array) = 

    let paths = Array.zeroCreate input.Length
    let mutable pathIndex = 0;

    for line in input do
        let matches = Regex.Matches(line, "([0-9]+),([0-9]+)")
        let path = Array.zeroCreate matches.Count
        let mutable index = 0
        for _match in matches do 
            let x = _match.Groups[1].Value |> int
            let y = _match.Groups[2].Value |> int
            let pos = {X = x; Y = y}
            path[index] <- pos
            index <- index + 1
            ()
        paths[pathIndex] <- path
        pathIndex <- pathIndex + 1

    let a = paths.Length
    0

let run () = 
    Day13.testcase "Day14/testInput.txt" 24 part1
    Day13.testcase "Day14/input.txt" 24 part1