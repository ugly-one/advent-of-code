module Day23

open System.Collections.Generic


type Position = {
    X: int
    Y: int
}

type Elf = Position

type Direction = 
    | North 
    | South
    | East
    | West

let findCorners (positions: seq<Position>) = 
    let biggestX = positions |> Seq.fold (fun biggest position -> if position.X > biggest then position.X else biggest) 0
    let smallestX = positions |> Seq.fold (fun smallest position -> if position.X < smallest then position.X else smallest) 999999
    let biggestY = positions |> Seq.fold (fun biggest position -> if position.Y > biggest then position.Y else biggest) 0
    let smallestY = positions |> Seq.fold (fun smallest position -> if position.Y < smallest then position.Y else smallest) 999999
    {X = smallestX; Y = smallestY}, {X = biggestX; Y = biggestY}

let print (positions: seq<Position>) = 
    let (topLeft, bottomRight) = findCorners positions
    for y in topLeft.Y - 3 .. 1 .. bottomRight.Y + 3 do 
        for x in topLeft.X - 3 .. 1 .. bottomRight.X + 3 do 
            if Seq.contains {X = x; Y = y} positions then printf "#" else printf "."
        printfn ""

let parse (input: string[]) = 
    let rows = input.Length
    let elves = seq {
        for row in 0 .. rows - 1 do 
        let createPosition (x: int) item = 
            match item with 
            | '.' -> None
            | '#' -> Some {X = x; Y = row}
            | _ -> failwith "unknown character"
        let row = input[row]
            
        let maybePositions = row |> Seq.mapi (fun index item -> createPosition index item)
        let positions = 
            maybePositions 
            |> Seq.filter (fun item -> item.IsSome) 
            |> Seq.map (fun maybeItem -> maybeItem.Value)
        yield positions
    }

    elves |> Seq.concat

let getAdjacentPositions (pos: Position) = 
    seq {
        yield {X = pos.X - 1 ; Y = pos.Y - 1}
        yield {X = pos.X  ; Y = pos.Y - 1}
        yield {X = pos.X + 1 ; Y = pos.Y - 1}

        yield {X = pos.X - 1 ; Y = pos.Y}
        yield {X = pos.X + 1 ; Y = pos.Y}

        yield {X = pos.X - 1 ; Y = pos.Y + 1}
        yield {X = pos.X ; Y = pos.Y + 1}
        yield {X = pos.X + 1; Y = pos.Y + 1}
    }

let getAdjacentNorthPositions (pos: Position) = 
    seq {
        yield {X = pos.X - 1 ; Y = pos.Y - 1}
        yield {X = pos.X  ; Y = pos.Y - 1}
        yield {X = pos.X + 1 ; Y = pos.Y - 1}
    }

let getAdjacentSouthPositions (pos: Position) = 
    seq {
        yield {X = pos.X - 1 ; Y = pos.Y + 1}
        yield {X = pos.X  ; Y = pos.Y + 1}
        yield {X = pos.X + 1 ; Y = pos.Y + 1}
    }

let getAdjacentWestPositions (pos: Position) = 
    seq {
        yield {X = pos.X - 1 ; Y = pos.Y - 1}
        yield {X = pos.X - 1; Y = pos.Y}
        yield {X = pos.X - 1 ; Y = pos.Y + 1}
    }

let getAdjacentEastPositions (pos: Position) = 
    seq {
        yield {X = pos.X + 1 ; Y = pos.Y - 1}
        yield {X = pos.X + 1; Y = pos.Y}
        yield {X = pos.X + 1 ; Y = pos.Y + 1}
    }

let isAnotherElfHere (allElves: HashSet<Position>) (positions: seq<Position>) = 
    positions |> Seq.fold (fun state position -> if allElves.Contains position then true else state) false

let move elf direction allElves = 
    match direction with 
    | North -> if getAdjacentNorthPositions elf |> isAnotherElfHere allElves |> not then Some {X = elf.X; Y = elf.Y - 1} else None
    | South -> if getAdjacentSouthPositions elf |> isAnotherElfHere allElves |> not then Some {X = elf.X; Y = elf.Y + 1} else None
    | West -> if getAdjacentWestPositions elf |> isAnotherElfHere allElves |> not then Some {X = elf.X - 1; Y = elf.Y} else None 
    | East -> if getAdjacentEastPositions elf |> isAnotherElfHere allElves |> not then Some {X = elf.X + 1; Y = elf.Y} else None

let considerPosition (allElves: HashSet<Position>) (elf: Elf) (directionsToConsider: Direction[]) = 
    let adjacentPositions = getAdjacentPositions elf
    let isAnotherElfNearBy = isAnotherElfHere allElves adjacentPositions 
    if not isAnotherElfNearBy 
    then 
        Some elf 
    else 
        let fold positionOption direction = 
            match positionOption with 
            | Some position -> 
                if position = elf 
                then 
                    let newPositionOption = move elf direction allElves 
                    match newPositionOption with 
                    | None -> positionOption
                    | Some newPosition -> Some newPosition
                else positionOption
            | None -> 
                failwith "wrong!"

        let position = 
            directionsToConsider 
                |> Array.fold fold (Some elf)
        position

let moveFirstDirectionToEndOfSeq (directions: Direction[]) = 
    let secondPart = directions[1..]
    Array.append secondPart [| directions[0] |]

let run input  rounds = 
    let positionsWithElves = input |> parse
    let elves = new HashSet<Elf>()
    for position in positionsWithElves do 
        elves.Add position |> ignore

    let mutable directionsToConsider = [| North; South; West; East|]

    for round in 1 .. rounds do 
        let consideredPositions = new Dictionary<Position, (bool * Elf[])>()
        for elf in elves do 
            let maybeNewPosition = considerPosition elves elf directionsToConsider
            match maybeNewPosition with 
            | None -> ()
            | Some newPosition -> 
                if consideredPositions.ContainsKey newPosition then
                    let (_, elvesOnTheSamePosition) = consideredPositions[newPosition] 
                    consideredPositions[newPosition] <- (false, elvesOnTheSamePosition |> Array.append [| elf |])
                else 
                    consideredPositions[newPosition] <- (true, [| elf |])
        
        let previousElves = new HashSet<Elf>()
        for elf in elves do
            previousElves.Add elf |> ignore

        elves.Clear()

        for keyValuePair in consideredPositions do 
            let position = keyValuePair.Key
            let (isTheOnlyElfMovingHere, elvesOnThisPosition) = keyValuePair.Value
            if isTheOnlyElfMovingHere then elves.Add position |> ignore
            else 
                for elf in elvesOnThisPosition do 
                    elves.Add elf |> ignore

        let mutable allElvesTheSame = true
        for previousElf in previousElves do 
            if elves.Contains previousElf then () else allElvesTheSame <- false

        if allElvesTheSame then failwithf "%d" round else ()
        
        directionsToConsider <- moveFirstDirectionToEndOfSeq directionsToConsider
        // printfn "______________________"
        // print elves
        // printfn "______________________"

    let (topLeft, bottomRight) = findCorners elves
    let mutable emptySpacesCount = 0
    for y in topLeft.Y .. 1 .. bottomRight.Y do 
        for x in topLeft.X .. 1 .. bottomRight.X do 
            if Seq.contains {X = x; Y = y} elves 
            then () else emptySpacesCount <- emptySpacesCount + 1

    emptySpacesCount

let part1() = 
    // let input = inputReader.readLines "Day23/testInput.txt"
    // run input
    let input = inputReader.readLines "Day23/largerTestInput.txt"
    let result = run input 10
    if result <> 110 then failwithf "Test input, wrong answer. %d" result else printfn "Test input, OK"

    let input = inputReader.readLines "Day23/input.txt"
    let result = run input 10
    if result <> 3996 then failwithf "Input, wrong answer. %d" result else printfn "Input, OK"

let part2 () = 
    let input = inputReader.readLines "Day23/largerTestInput.txt"
    let result = run input 99999
    if result <> 110 then failwithf "Test input, wrong answer. %d" result else printfn "Test input, OK"
