module Day14

open System.Text.RegularExpressions

type Position = {
    X: int
    Y: int
}

type Path = {
    Positions: Position array
}

type Cell = 
    | Rock
    | Air

type Map = {
    Cells: Cell array
    Width: int
    Height: int
}

let print (map: Map) = 
    for y in 0..map.Height-1 do 
        for x in 0..map.Width-1 do 
            match map.Cells[x + y * map.Height] with 
            | Rock -> printf "#"
            | Air -> printf "."
        printfn ""

let part1 mapSize (input: string array)  = 
    let width = mapSize
    let height = mapSize
    let map = Array.create (width * height) Air

    let paths = Array.zeroCreate input.Length
    let mutable pathIndex = 0;

    for line in input do
        let matches = Regex.Matches(line, "([0-9]+),([0-9]+)")
        let path = Array.zeroCreate matches.Count
        let mutable index = 0
        for _match in matches do 
            let x = _match.Groups[1].Value |> int
            let y = _match.Groups[2].Value |> int
            let pos = {X = x - 480; Y = y}
            path[index] <- pos
            map[pos.X + height*pos.Y] <- Cell.Rock
            if index - 1 >= 0 
            then 
                let previousPosition = path[index-1]
                let diffX = pos.X - previousPosition.X
                let diffY = pos.Y - previousPosition.Y
                if (diffX <> 0) 
                then 
                    for a in 1..abs diffX do 
                        if diffX > 0 then map[pos.X + a + height*pos.Y] <- Cell.Rock else map[pos.X + a + height*pos.Y] <- Cell.Rock
                else 
                     for a in 1..abs diffY do 
                        if diffY > 0 then map[pos.X + height*(pos.Y-a)] <- Cell.Rock else map[pos.X + height*(pos.Y+a)] <- Cell.Rock
            else 
                ()
            index <- index + 1
            ()
        paths[pathIndex] <- path
        pathIndex <- pathIndex + 1

    print {Cells = map; Width = width; Height = height}
    let a = paths.Length
    0

let run () = 
    Day13.testcase "Day14/testInput.txt" 24 (part1 30)
    Day13.testcase "Day14/input.txt" 24 (part1 200)