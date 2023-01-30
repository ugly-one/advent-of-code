module Day22_part1

open Day22

let parseOnTestInput () = 
    let input = inputReader.readLines "Day22/testInput.txt"
    parse input

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
    if result <> 88226 then failwithf "failed real input - final calculation. %d" result else printfn "real input OK"
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