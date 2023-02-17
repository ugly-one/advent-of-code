module Day14

open System.Text.RegularExpressions
open System.Collections.Generic
open System

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
            match map.Cells[x + y * map.Width] with 
            | Rock ->   printf "#"
            | Air ->    printf "."
            | Sand ->   printf "o"
        printfn ""
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
    paths.ToArray (), minX, maxX, maxY

let createMap (input: string array) floor additionalWidth = 
    let (paths, minX, maxX, maxY) = createPaths input
    let height = maxY + 1 + floor
    let width = maxX + 1 + additionalWidth
    let map = Array.create (width * height) Air
    for path in paths do
        for positionIndex in 0..(path.Positions.Length - 1) do
            let pos = path.Positions[positionIndex]
            let pos = {X = pos.X; Y = pos.Y}
            map[pos.X + width*pos.Y] <- Cell.Rock
            if positionIndex - 1 >= 0 
            then 
                let previousPosition = path.Positions[positionIndex-1]
                let previousPosition =  {X = previousPosition.X; Y = previousPosition.Y}
                let diffX = pos.X - previousPosition.X
                let diffY = pos.Y - previousPosition.Y
                if (diffX <> 0) 
                then 
                    for a in 1..abs diffX do 
                        if diffX > 0 then map[pos.X - a + width*pos.Y] <- Cell.Rock else map[pos.X + a + width*pos.Y] <- Cell.Rock
                else 
                     for a in 1..abs diffY do 
                        if diffY > 0 then map[pos.X + width*(pos.Y-a)] <- Cell.Rock else map[pos.X + width*(pos.Y+a)] <- Cell.Rock
            else 
                ()
    if floor <> 0 then 
        for x in 0..width-1 do 
            map[x + (height-1)*width] <- Cell.Rock
        else ()
    {Cells = map; Height = height; Width = width}, minX

let isEmpty cell = 
    if cell = Air then true else false

let isWithinMap map position = 
    if position.X < 0 || position.Y < 0 then false else 
        if position.X >= map.Width || position.Y >= map.Height then false else true

let rec dropSand position map = 
    let positionBelow = {X = position.X; Y = position.Y + 1}
    if not (isWithinMap map positionBelow) then false else
        let cellBelow = map.Cells[positionBelow.X + (positionBelow.Y) * map.Width]
        if isEmpty cellBelow
        then dropSand positionBelow map
        else 
            let positionBelowLeft = {X = position.X - 1; Y = position.Y + 1}
            if not (isWithinMap map positionBelowLeft) then false else
                let cellBelowLeft = map.Cells[positionBelowLeft.X + positionBelowLeft.Y * map.Width]
                if isEmpty cellBelowLeft 
                then dropSand positionBelowLeft map 
                else 
                    let positionBelowRight = {X = position.X + 1; Y = position.Y + 1}
                    if not (isWithinMap map positionBelowRight) then false else
                        let cellBelowRight = map.Cells[positionBelowRight.X + positionBelowRight.Y * map.Width]
                        if isEmpty cellBelowRight 
                        then dropSand positionBelowRight map 
                        else 
                            if isWithinMap map position then  
                                map.Cells[position.X + position.Y * map.Width] <- Sand
                                true
                            else false


let part1 (input: string array)  = 
    let (map, minX) = createMap input 0 0
    let mutable counter = 0
    let mutable drop = true
    while drop do 
        //print map
        drop <- dropSand {X = 500; Y = 0} map
        if drop then counter <- counter + 1 else ()
       // Console.Clear ()
    counter

let part2 (input: string array)  = 
    let (map, minX) = createMap input 2 500
    let mutable counter = 0
    let mutable drop = true
    while drop do 
        //print map
        drop <- dropSand {X = 500; Y = 0} map
        if drop then counter <- counter + 1 else ()
        if map.Cells[500] = Sand then drop <- false else ()
        //Console.Clear ()
    counter

let run () = 
    Day13.testcase "Day14/testInput.txt" 24 part1
    Day13.testcase "Day14/input.txt" 737 part1

    Day13.testcase "Day14/testInput.txt" 93 part2
    Day13.testcase "Day14/input.txt" 28145 part2

