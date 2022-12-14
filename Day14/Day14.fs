module Day14

open System.Text.RegularExpressions
open System.Collections.Generic

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
    | Sand

type Map = {
    Cells: Cell array
    Width: int
    Height: int
}

let print (map: Map) = 
    for y in 0..map.Height-1 do 
        for x in 0..map.Width-1 do 
            match map.Cells[x + y * map.Height] with 
            | Rock ->   printf "#"
            | Air ->    printf "."
            | Sand ->   printf "o"
        printfn ""

let createPaths (input: string array) = 
    let paths = new List<Path>()
    let mutable minX = 9999
    let mutable maxX = 0
    let mutable maxY = 0
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
            if (x < minX) then minX <- x else ()
            if (x > maxX) then maxX <- x else ()
            if (y > maxY) then maxY <- y else ()
        paths.Add({Positions = path})
    paths, minX, maxX, maxY

let createMap mapSize (input: string array) = 
    let width = mapSize
    let height = mapSize
    let map = Array.create (width * height) Air
    for line in input do
        let matches = Regex.Matches(line, "([0-9]+),([0-9]+)")
        // TODO no need to have an array of paths, we need only the previous one
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
    {Cells = map; Width = width; Height = height}

let isEmpty cell = 
    if cell = Air then true else false

let rec dropSand position map = 
    let positionBelow = {X = position.X; Y = position.Y + 1}
    let cellBelow = map.Cells[positionBelow.X + (positionBelow.Y) * map.Height]
    if isEmpty cellBelow
    then dropSand positionBelow map
    else 
        let positionBelowLeft = {X = position.X - 1; Y = position.Y + 1}
        let cellBelowLeft = map.Cells[positionBelowLeft.X + positionBelowLeft.Y * map.Height]
        if isEmpty cellBelowLeft 
        then dropSand positionBelowLeft map 
        else 
            let positionBelowRight = {X = position.X + 1; Y = position.Y + 1}
            let cellBelowLeft = map.Cells[positionBelowRight.X + positionBelowRight.Y * map.Height]
            if isEmpty cellBelowLeft 
            then dropSand positionBelowRight map 
            else map.Cells[position.X + position.Y * map.Height] <- Sand


let part1 mapSize (input: string array)  = 
    let map = createMap mapSize input
    print map
    dropSand {X = 20; Y = 0} map
    print map
    dropSand {X = 20; Y = 0} map
    print map
    dropSand {X = 20; Y = 0} map
    print map
    dropSand {X = 20; Y = 0} map
    print map
    dropSand {X = 20; Y = 0} map
    print map
    dropSand {X = 20; Y = 0} map
    print map
    0

let run () = 
    Day13.testcase "Day14/testInput.txt" 24 (part1 30)
    //Day13.testcase "Day14/input.txt" 24 (part1 200)