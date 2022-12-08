module advent_of_code_2022.Day8

type Coordinate = {
    X: int
    Y: int
}

type Map = {
    Values: int[]
    ColumnsCount: int
    RowsCount: int
}

let printCoordinate coordinate =
    printf "x: %d y: %d" coordinate.X coordinate.Y
    
let printCoordinates seq =
    for item in seq do
        printCoordinate item

let private toMap (input: string[]) =
    let values = seq {
                        for line in input do
                            for i in 0 .. (line.Length - 1) do
                                yield line[i] |> int
                    } |> Array.ofSeq
    {Values = values; RowsCount = input.Length; ColumnsCount = input[0].Length}
    
let private getDirections x y map =
    let up =  seq { for i in y - 1 .. -1 .. 0 do {X = x; Y = i} }
    let down = seq { for i in y + 1 .. 1 .. (map.RowsCount - 1) do {X = x; Y = i} }
    let left = seq { for i in x - 1 .. -1 .. 0 do {X = i; Y = y} }
    let right = seq { for i in x + 1 .. 1 .. (map.ColumnsCount - 1) do {X = i; Y = y} }
    (up, right, down, left)

let takeValue map coordinate =
    let index = coordinate.Y * map.RowsCount + coordinate.X
    map.Values[index]

let hasOnlyLowerTreesInDirection (map: Map) (coordinate: Coordinate) (direction: seq<Coordinate>) =
    let coreValue = takeValue map coordinate
    let isHighest = Seq.fold (fun state coordinate -> if takeValue map coordinate >= coreValue then false else state) true direction
    isHighest
    
let run () =
    let map = toMap Day8_input.input
    let mutable counter = 0
    for x in 0 .. (map.ColumnsCount - 1) do
            for y in 0 .. (map.RowsCount - 1) do
                let coordinate = {X=x; Y=y}
                let (up, right, down, left) = getDirections x y map
                if (x = 0 || y = 0 || x = map.ColumnsCount - 1 || y = map.RowsCount - 1 ) then counter <- counter + 1
                else 
                    let isHighestInUpDirection = hasOnlyLowerTreesInDirection map coordinate up
                    let isHighestInRightDirection = hasOnlyLowerTreesInDirection map coordinate right
                    let isHighestInDownDirection = hasOnlyLowerTreesInDirection map coordinate down
                    let isHighestInLeftDirection = hasOnlyLowerTreesInDirection map coordinate left
                    if (isHighestInDownDirection || isHighestInRightDirection || isHighestInLeftDirection || isHighestInUpDirection)
                        then counter <- counter + 1
                        else ()
    counter
   
let countVisibleTreesInDirection (map: Map) (coordinate: Coordinate) (direction: seq<Coordinate>) =
    let coreValue = takeValue map coordinate
    let array = Array.ofSeq direction
    if array.Length = 0 then 0 else
        let mutable counter = 0
        let mutable index = 0;
        let mutable keepLooking = true
        while (keepLooking) do
            let nextTree = array[index]
            let value = takeValue map nextTree
            if (value < coreValue) then counter <- counter + 1
            else 
                counter <- counter + 1
                keepLooking <- false
            index <- index + 1
            if (index >= array.Length) then keepLooking <- false else ()
        counter
        
let run2 () =
    let map = toMap Day8_input.input
    let mutable highestScore = 0
    for x in 0 .. (map.ColumnsCount - 1) do
            for y in 0 .. (map.RowsCount - 1) do
                let coordinate = {X=x; Y=y}
                let (up, right, down, left) = getDirections x y map
                let scoreUp = countVisibleTreesInDirection map coordinate up
                let scoreRight = countVisibleTreesInDirection map coordinate right
                let scoreDown = countVisibleTreesInDirection map coordinate down
                let scoreLeft = countVisibleTreesInDirection map coordinate left
                let score = scoreDown * scoreLeft * scoreRight * scoreUp
                if (score > highestScore) then highestScore <- score else ()
    highestScore
    