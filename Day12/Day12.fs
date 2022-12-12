module Day12

open System.Collections.Generic

let charToHeight (char:char) = 
    (char |> int) - 97

type Cell = {
    Position: (int * int)
    Elevation: int
    mutable DistanceToBest: int
}

type Map = {
    Cells: Cell array
    Width: int
    Height: int
}

let getCell map (x,y) =
    map.Cells[x + y * map.Width]

let getAdjacentCells (map: Map) cell =
    let x = cell.Position |> fst
    let y = cell.Position |> snd
    let leftX = x - 1
    let rightX = x + 1
    let upY = y - 1
    let downY = y + 1
    let positions = [| (leftX, y); (x, upY); (rightX, y); (x, downY)|] 
                        |> Array.filter (fun (x,y) -> x >= 0 && x < map.Width && y >= 0 && y < map.Height)
    Array.map (getCell map) positions

let getMap (input: string[]) = 
    let mapWidth = input[0].Length
    let mutable lowestPositions = new List<(int * int)>()
    let mutable startingPosition = (0,0)
    let mutable bestSignalPos = (0,0)
    let map = Array.init (input.Length * mapWidth) (fun i -> {Elevation = -1; DistanceToBest = -1; Position = (-1,-1)})
    for y in 0..(input.Length-1) do 
        let line = input[y]
        for x in 0..(mapWidth-1) do 
            let mutable char = line[x]
            let mutable distanceToBest = -1
            if line[x] = 'S' then 
                char <- 'a'
                startingPosition <- (x,y)
            else if line[x] = 'E' then 
                bestSignalPos <- (x, y) 
                char <- 'z'
                distanceToBest <- 0
            if char = 'a' then lowestPositions.Add(x, y) else ()

            map[x + y * mapWidth] <- {Elevation = char |> charToHeight; DistanceToBest = distanceToBest; Position = (x, y)}
    ({Cells = map; Width = mapWidth; Height = input.Length}, lowestPositions, startingPosition, bestSignalPos)
    
let rec markDistance map cellToConsider previousCell =
    let adjacentCells = getAdjacentCells map cellToConsider |> Array.filter (fun cell -> cell.Position <> previousCell.Position)
    let doableAdjacentCells = Array.filter (fun (cell) -> cell.Elevation >= (cellToConsider.Elevation - 1)) adjacentCells
    let modifiedCells = new List<Cell>()
    for cell in doableAdjacentCells do
        let currentDistance = cellToConsider.DistanceToBest + 1
        if currentDistance < cell.DistanceToBest || cell.DistanceToBest = -1 then 
            cell.DistanceToBest <- currentDistance
            modifiedCells.Add(cell)
        else ()
    
    for cell in modifiedCells do 
        markDistance map cell cellToConsider
    
let run (input: string[]) = 
    let (map, _, startingPos, bestSignalPos) = getMap input
    let cellToConsider = getCell map bestSignalPos
    markDistance map cellToConsider cellToConsider
    let result = getCell map startingPos
    result.DistanceToBest

let run2 (input: string[]) = 
    let (map, lowestPositions, _, bestSignalPos) = getMap input
    let cellToConsider = getCell map bestSignalPos
    markDistance map cellToConsider cellToConsider
    let mutable shortest = 999
    for pos in lowestPositions do
        let cell = getCell map pos
        if cell.DistanceToBest < shortest && cell.DistanceToBest <> -1 then shortest <- cell.DistanceToBest else ()
    shortest

let part2 () =
    let result = inputReader.readLines "Day12/input.txt" |> Array.ofSeq |> run2
    if result = 345 then printfn "OK" else failwith $"{result} is wrong"

let test_part2 () =
    let result = inputReader.readLines "Day12/testInput.txt" |> Array.ofSeq |> run2
    if result = 29 then printfn "OK" else failwith $"{result} is wrong"
    
let part1 () =
    let result = inputReader.readLines "Day12/input.txt" |> Array.ofSeq |> run
    if result = 352 then printfn "OK" else failwith $"{result} is wrong"
    
let test_part1 () = 
    let result = inputReader.readLines "Day12/testInput.txt" |> Array.ofSeq |> run
    if result = 31 then printfn "OK" else failwith $"{result} is wrong"
