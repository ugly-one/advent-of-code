module Day22 

type Position = 
    {
        X: int
        Y: int
    }

type Direction = 
    | Up
    | Down
    | Right
    | Left

type Map = 
    {
        Content: string[]
        Height: int
    }

let getOppositeDirection (direction: Direction) = 
    match direction with 
    | Up -> Down
    | Down -> Up
    | Left -> Right
    | Right -> Left

let move (pos: Position) (direction: Direction) (map: Map) = 
    let (x, y) = 
        match direction with 
        | Up -> if pos.Y = 0 then (pos.X, map.Height - 1) else (pos.X, pos.Y - 1)
        | Down -> if pos.Y = map.Height - 1 then (pos.X, 0) else (pos.X, pos.Y + 1)
        | Left -> if pos.X = 0 then (map.Content[pos.Y].Length - 1, pos.Y) else (pos.X - 1, pos.Y)
        | Right -> if pos.X = map.Content[pos.Y].Length - 1 then (0, pos.Y) else (pos.X + 1, pos.Y)
    {X = x; Y = y}    

let moveUntilMapsEnd (pos: Position) (direction: Direction) (map: Map) = 
    let mutable keepGoing = true
    let mutable result = pos
    let mutable newPos = pos
    while keepGoing do 
        newPos <- move newPos direction map
        if map.Content[newPos.Y][newPos.X] = ' ' then keepGoing <- false else result <- newPos
    result

let walk (pos: Position) (direction: Direction) (steps: int) (map: Map) = 
    let mutable endPosition = pos
    for step in 1 .. 1 .. steps do 
        let newPosition = move endPosition direction map
        if map.Content[newPosition.Y][newPosition.X] = '.' 
            then 
                endPosition <- newPosition
        elif map.Content[newPosition.Y][newPosition.X] = '#'
            then () // TODO stop the loop!
        else 
            let positionAfterWrappingAround = moveUntilMapsEnd newPosition (getOppositeDirection direction) map
            if map.Content[positionAfterWrappingAround.Y].[positionAfterWrappingAround.X] = '#' 
                then () // TODO stop the loop
                else
                    endPosition <- positionAfterWrappingAround
    endPosition

type Rotation = 
    | CounterClockWise
    | Clockwise

type Movement = 
    | Rotate of Rotation
    | Walk of int

let parsePath (path: string) = 
    let mutable steps = ""
    seq {
        for char in path do 
            match char with 
            | 'R' -> 
                yield Walk (steps |> int)
                yield Rotate Clockwise
                steps <- ""
            | 'L' ->
                yield Walk (steps |> int)
                yield Rotate CounterClockWise
                steps <- ""
            | _ -> steps <- steps + (char |> string)
        if steps <> "" then yield Walk (steps |> int)
    }
let rotate rotation direction = 
    match direction with 
    | Up -> if rotation = Clockwise then Right else Left
    | Down -> if rotation = Clockwise then Left else Right
    | Left -> if rotation = Clockwise then Up else Down
    | Right -> if rotation = Clockwise then Down else Up

let directionValue direction = 
    match direction with 
    | Up -> 3
    | Down -> 1
    | Left -> 2
    | Right -> 0

let parse (input: string[]) = 
    let map = input[0..input.Length - 3]
    let mapWidth = map |> Array.fold (fun width row -> if row.Length > width then row.Length else width) 0
    let extend (row: string) = 
        if row.Length < mapWidth then 
            let missingPartLength = mapWidth - row.Length
            let missingPart = String.init missingPartLength (fun i -> " ")
            row + missingPart
        else 
            row
    let map = map |> Array.map (fun row -> extend row)
    let startPosition = Seq.findIndex (fun char -> char = '.') map[0]
    let pathToTake = input[input.Length - 1] |> parsePath
    ({Content = map; Height = map.Length}, startPosition, pathToTake)

let parseOnTestInput () = 
    let input = inputReader.readLines "Day22/testInput.txt"
    parse input

let runPart1 input = 
    let (map, startingPosition, path) = parse input
    let mutable position = {X = startingPosition; Y = 0}
    let mutable direction = Right
    for movement in path do 
        match movement with 
        | Walk steps -> position <- walk position direction steps map
        | Rotate rotation -> direction <- rotate rotation direction
    (position, direction)

let testGoingDown () = 
    let (map, startPosition, _) = parseOnTestInput ()
    let direction = Down
    let steps = 1
    let position = walk {X = startPosition; Y = 0} direction steps map
    if position <> {X = 8; Y = 1} then failwith "you cannot go even one step down" else printfn "Going down one step OK"

let testHittingWall () = 
    let (map, startPosition, _) = parseOnTestInput ()
    let direction = Right
    let steps = 678
    let position = walk {X = startPosition; Y = 0}  direction steps map
    if position <> {X = 10; Y = 0} then failwith "did you miss the wall??" else printfn "Stopping on a wall OK"

let testWrappingAroundVertical () = 
    let (map, _, _) = parseOnTestInput ()
    let direction = Up
    let steps = 1
    let position = walk {X = 5; Y = 4} direction steps map
    if position <> {X = 5; Y = 7} then failwith "did you just fall from the cliff?? (simple UP)" else printfn "Wrapping around (simple UP) OK"

let testWrappingAroundHorizontal() = 
    let (map, _, _) = parseOnTestInput ()
    let direction = Left
    let steps = 100
    let position = walk {X = 5; Y = 7} direction steps map
    if position <> {X = 11; Y = 7} then failwith "did you just fall from the cliff?? (simple LEFT)" else printfn "Wrapping around (simple LEFT) OK"

let testWrappingAroundVertical_2 () = 
    let (map, startingPosition, _) = inputReader.readLines "Day22/testInput2.txt" |> parse 
    let direction = Up
    let steps = 1
    let position = walk {X = startingPosition; Y = 0} direction steps map
    if position <> {X = startingPosition; Y = 7} then failwith "did you just fall from the cliff?? (complex UP)" else printfn "Wrapping around (complex UP) OK"


let testInput () = 
    let input = inputReader.readLines "Day22/testInput.txt"
    let (position, direction) = runPart1 input
    if position <> {X = 7; Y = 5} then failwith "failed test input" else printfn "Test input OK"
    let result = 1000 * (position.Y + 1) + 4 * (position.X + 1) + (directionValue direction)
    if result <> 6032 then failwith "failed test input - final calculation" else printfn "Test input OK"
    ()

let testReal () = 
    let input = inputReader.readLines "Day22/input.txt"
    let (position, direction) = runPart1 input
    let result = 1000 * (position.Y + 1) + 4 * (position.X + 1) + (directionValue direction)
    if result <> 6032 then failwithf "failed real input - final calculation. %d" result else printfn "real input OK"
    ()

let part1() = 
    testGoingDown ()
    testHittingWall ()
    testWrappingAroundVertical ()
    testWrappingAroundHorizontal ()
    testWrappingAroundVertical_2 ()
    testInput ()
    testReal ()
    ()