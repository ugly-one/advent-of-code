module Day12

let charToHeight (char:char) = 
    (char |> int) - 97

type Cell = {
    Height: int
    DistanceToBest: int
}

type Map = {
    Cells: Cell array
    Width: int
    Height: int
}

let getAdjacentCells (map: Map) (x, y) = 
    let leftX = x - 1
    let rightX = x + 1
    let upY = y - 1
    let downY = y + 1

    [| (leftX, y); (x, upY); (rightX, y); (x, downY)|] 
        |> Array.filter (fun (x,y) -> x >= 0 && x < map.Width && y >= 0 && y < map.Height)



let getMap (input: string[]) = 
    let mapWidth = input[0].Length
    let mutable currentPos = (0,0)
    let mutable bestSignalPos = (0,0)
    let map = Array.init (input.Length * mapWidth) (fun i -> {Height = -1; DistanceToBest = -1})
    for y in 0..(input.Length-1) do 
        let line = input[y]
        for x in 0..(mapWidth-1) do 
            let mutable char = line[x]
            if line[x] = 'S' then 
                currentPos <- (x, y)
                char <- 'a'
            else if line[x] = 'E' then 
                bestSignalPos <- (x, y) 
                char <- 'z'
            map[x + y * mapWidth] <- {Height = char |> charToHeight; DistanceToBest = -1}
    ({Cells = map; Width = mapWidth; Height = input.Length}, currentPos, bestSignalPos)

let run (input: string[]) = 
    let (map, currentPos, bestSignalPos) = getMap input
    let adjacentCells = getAdjacentCells map bestSignalPos
    0

let test_part1 input = 
    let result = run input
    if result = 31 then () else failwith $"{result} is wrong"
