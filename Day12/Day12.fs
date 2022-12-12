module Day12

let charToHeight (char:char) = 
    (char |> int) - 97

let run (input: string[]) = 
    let mapWidth = input[0].Length
    let mutable currentPos = (0,0)
    let mutable bestSignalPos = (0,0)
    let map = Array.init (input.Length * mapWidth) (fun i -> 0)
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
            map[x + y * mapWidth] <- char |> charToHeight
    0

let test_part1 input = 
    let result = run input
    if result = 31 then () else failwith $"{result} is wrong"
