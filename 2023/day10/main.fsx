#load "../general_typo_here_fixed.fsx"
open System.Collections.Generic
let testInput ="""
.....
.S-7.
.|.|.
.L-J.
.....
"""

let testInput2 = """
..F7.
.FJ|.
SJ.L7
|F--J
LJ...
"""

let input = General.readLines "input.txt"

type Direction =
    | Up
    | Down
    | Left
    | Right
    
type Position = {X: int; Y: int}

let toMapFromStringArray (input: string array) =
    input
    |> Seq.map (fun l -> l.Trim())
    |> Seq.filter (fun f -> f.Length > 0)
    |> Array.ofSeq
let toMap (input: string) =
    input.Split('\n')
    |> toMapFromStringArray

let findStart (map: string array) =
    let mutable row = 0
    let mutable column = 0
    let mutable result = (0,0)
    for line in map do
        for char in line do
            if char = 'S' then result <- (row, column)
            else column <- column + 1
        row <- row + 1
        column <- 0
    {X = result |> snd; Y = result |> fst}
    
let goLeft pos =
    {pos with X = pos.X - 1}
   
let goRight pos =
    {pos with X = pos.X + 1}
    
let goUp pos =
    {pos with Y = pos.Y - 1}
    
let goDown pos =
    {pos with Y = pos.Y + 1}
    
let getChar (map: string array) pos =
    if pos.Y >= (map.Length) || (pos.Y < 0) || (pos.X < 0) || (pos.X >= map[0].Length) then None
    else Some (map[pos.Y][pos.X])
    
let getConnectedPipes (map: string array) (pos:Position) =
    let getChar = getChar map
    let result = List<Position>()
    let leftPos = goLeft pos
    let leftChar = getChar leftPos
    match leftChar with
    | Some '-' -> result.Add(leftPos)
    | Some 'F' -> result.Add(leftPos)
    | Some 'L' -> result.Add(leftPos)
    | _ -> ()
    let rightPos = goRight pos
    let rightChar = getChar rightPos
    match rightChar with
    | Some '-' -> result.Add(rightPos)
    | Some 'J' -> result.Add(rightPos)
    | Some '7' -> result.Add(rightPos)
    | _ -> ()
    
    let upPos = goUp pos
    let topChar = getChar upPos
    match topChar with
    | Some '|' -> result.Add(upPos)
    | Some 'F' -> result.Add(upPos)
    | Some '7' -> result.Add(upPos)
    | _ -> ()
    let downPos = goDown pos
    let bottomChar = getChar downPos
    match bottomChar with
    | Some '|' -> result.Add(downPos)
    | Some 'F' -> result.Add(downPos)
    | Some '7' -> result.Add(downPos)
    | _ -> ()
    result    
    
let getDirection (pos1: Position) (pos2: Position) =
    let xDiff = pos2.X - pos1.X
    let yDiff = pos2.Y - pos1.Y
    match (xDiff, yDiff) with
    | (-1, 0) -> Left
    | (1, 0) -> Right
    | (0, -1) -> Up
    | (0, 1) -> Down
    | _ -> failwithf $"%A{pos1} %A{pos2}"
    
let getDirections (position: Position) (positions: List<Position>) =
    positions |> Seq.map (getDirection position)
  
let walk (map: string array) (pos: Position, direction: Direction) =
    let newPos =
        match direction with
        | Up -> goUp pos
        | Down -> goDown pos
        | Left -> goLeft pos
        | Right -> goRight pos
    
    let newChar = getChar map newPos
    let newChar = newChar.Value // we know we cannot go outside of map when walking
    let newDirection =
        match (newChar, direction) with
        | ('|', _) -> direction
        | ('-', _) -> direction
        | ('L', Down) -> Right
        | ('L', Left) -> Up
        | ('J', Down) -> Left
        | ('J', Right) -> Up
        | ('7', Up) -> Left
        | ('7', Right) -> Down
        | ('F', Up) -> Right
        | ('F', Left) -> Down
        | _ -> failwithf $"walk failed... %A{pos} %A{direction} "
       
    (newPos, newDirection) 

let rec walkBothDirections (pos1: Position, direction1: Direction) (pos2: Position, direction2: Direction) (map: string array) =
    if (pos1 = pos2) then 0
    else
        let new1 = walk map (pos1, direction1)
        let new2 = walk map (pos2, direction2)
        1 + (walkBothDirections new1 new2 map)
   
let solvePart1 map =

    let startPosition =
        map
        |> findStart

    let directionsToCheck =
        startPosition
        |> getConnectedPipes map
        |> getDirections (startPosition)

    let firstDirection = directionsToCheck |> Seq.head
    let secondDirection = directionsToCheck |> Seq.skip 1 |> Seq.head
    let pos1 = (startPosition, firstDirection) |> walk map
    let pos2 = (startPosition, secondDirection) |> walk map

    walkBothDirections pos1 pos2 map
    |> (fun x -> x + 1)
    |> General.print
    
testInput2 |> toMap |> solvePart1
input |> toMapFromStringArray |> solvePart1 

// TODO part2 - I think the way to solve part 2 is to count any tiles (dots) that are on the correct side of the pipes
// TODO find the right side to check
// TODO everytime a direction changes, the side also changes
// TODO we should could all tiles on the correct side, regardless how far they are

let testInput3 = """
...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........
"""

let map = testInput |> toMap

let start () =  
    let startPosition =
        map
        |> findStart
    
    let directionsToCheck =
        startPosition
        |> getConnectedPipes map
        |> getDirections (startPosition)
        
    ()
