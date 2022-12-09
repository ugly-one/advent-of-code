module Day9

open System.Text.RegularExpressions
open System.Collections.Generic

let readLines file = System.IO.File.ReadLines(file)
let input = readLines "Day9/input.txt"
let testInput = readLines "Day9/testInput.txt"
let testInput2 = readLines "Day9/testInput2.txt"


type Position = {
    X: int;
    Y: int
}

let print head (knots: array<Position>) = 
    let size = 20
    printfn ""
    printfn "---------------------"
    for y in size .. -1 .. (-size) do
        for x in -size..size do 
            if head = {X = x; Y = y} 
            then 
                printf "H" 
            else
                if Array.contains {X = x; Y = y} knots then printf "%d" ( Array.findIndex (fun p -> p = {X = x; Y = y}) knots) else printf "."
        printfn ""
    printfn "---------------------"
    printfn ""

let makeStep position direction = 
    match direction with 
    | "U" -> {position with Y = position.Y + 1}
    | "D" -> {position with Y = position.Y - 1}
    | "L" -> {position with X = position.X - 1}
    | "R" -> {position with X = position.X + 1}
    | _ -> failwith "bad direction"

let hasToMove tailPosition headPosition = 
    if (abs (tailPosition.X - headPosition.X) >= 2) || (abs (tailPosition.Y - headPosition.Y) >= 2)
    then true
    else false

let moveKnot knotToMove knotToFollow = 
    let xDiff = if knotToFollow.X - knotToMove.X < 0 then -1 else if knotToFollow.X - knotToMove.X > 0 then 1 else 0
    let yDiff = if knotToFollow.Y - knotToMove.Y < 0 then -1 else if knotToFollow.Y - knotToMove.Y > 0 then 1 else 0
    { X = knotToMove.X + xDiff; Y = knotToMove.Y + yDiff }

let moveRope size moves = 
    let position = { X = 0; Y = 0; }
    let mutable head = position
    let knots = Array.create size position
    let visitedPositions = new HashSet<Position>()
    visitedPositions.Add(knots[knots.Length-1]) |> ignore
    for line in moves do 
        let _match = Regex.Match(line, "([RULD]) (.*)")
        let direction = _match.Groups[1].Value
        let steps = _match.Groups[2].Value |> int
        for step in 1..steps do 
            head <- makeStep head direction
            for knotIndex in 0..(knots.Length - 1) do
                let knot = knots[knotIndex]
                let knotTofollow = if knotIndex = 0 then head else knots[knotIndex-1]
                if hasToMove knot knotTofollow 
                then 
                    knots[knotIndex] <- moveKnot knot knotTofollow
                else ()
            visitedPositions.Add(knots[knots.Length-1]) |> ignore
        //print head knots
    visitedPositions.Count

let run () = 
    let knotsCount = 9
    moveRope knotsCount input
