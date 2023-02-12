module Day24

open System.Collections.Generic

type Blizzard = char

type Field = array<Blizzard>

type Cell = 
    | Wall
    | Field of Field

type Position = {
    X: int;
    Y: int;
}


let getWidth (map: Cell[,]) = 
    Array2D.length2 map

let getHeight (map: Cell[,]) = 
    Array2D.length1 map

let printPos position = 
    printf "(%d,%d)" position.X position.Y

let printPositions (history: Position[]) = 
    for item in history do 
        printPos item
        printf ","

let print (map: Cell[,]) expedition = 
    let width = getWidth map
    let height = getHeight map
    for y in 0 .. 1 .. height - 1 do 
        for x in 0 .. 1 .. width - 1 do 
            let cell = map[y, x] 
            match cell with 
            | Wall -> printf "#"
            | Field field -> 
                match List.ofArray field with 
                    | [] -> if expedition.Y = y && expedition.X = x then printf "E" else printf "."
                    | x::[] -> printf "%c" x
                    | _ -> printf "%d" field.Length
        printfn ""

let createEmptyMap width height = 
    let getCell y x = 
        if y = 0 || x = 0 || y = height - 1 || x = width - 1 
        then 
            if (y = 0 && x = 1) || (y = height - 1 && x = width - 2) then Field[| |] else Wall 
        else Field [| |]
    Array2D.init height width (fun y x -> getCell y x )

let charToBlizzard char = 
    match char with 
    | '.' -> Field [||]
    | '#' -> Wall
    | a -> Field [| a |]

let getPositionToUpdate (map: Cell[,]) blizzard x y  width height  = 
    match blizzard with 
        | '>' -> 
            match map[y, x + 1] with 
            | Field f -> (y, x + 1)
            | Wall -> (y, 1)
        | 'v' -> 
            match map[y + 1, x] with 
            | Field f -> (y + 1, x)
            | Wall -> (1, x)
        | '<' -> 
            match map[y, x - 1] with 
            | Field f -> (y, x - 1)
            | Wall -> (y, width - 2)
        | '^' -> 
            match map[y - 1, x] with 
            | Field f -> (y - 1, x)
            | Wall -> (height - 2, x)
        | _ -> failwith "not possible"


let moveBlizzard map blizzard y x  width height = 
    let (targetY, targetX) = getPositionToUpdate map blizzard x y width height
    (targetY, targetX)

let getPossiblePositions position (map: Cell[,]) expeditionStart =
    let width = getWidth map
    let height = getHeight map
    let x = position.X
    let y = position.Y
    let allPosiblePositions = seq {

        yield {Y = y; X = x + 1}
        yield {Y = y + 1; X = x}
        yield {Y = y - 1; X =  x}
        yield {Y = y; X = x - 1}
    }

    // if we started at the top of the map, let's prioritize new positions that lead us down/right. Otherwise, let's try to move up/left
    let allPosiblePositions = if expeditionStart.Y = 0 then allPosiblePositions else Seq.rev allPosiblePositions

    let isAvailable cell = 
        match cell with 
        | Wall -> false
        | Field field -> if Array.length field = 0 then true else false

    let isWithinMap pos = 
        if pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height then true else false

    let positions = allPosiblePositions |> Seq.filter (fun pos -> isWithinMap pos && isAvailable map[pos.Y, pos.X] && expeditionStart = pos |> not)
    if Seq.contains expeditionStart positions 
        then 
            failwith "WHAT?!"
        else 
            ()
    positions

let moveBlizzards (map: Cell[,]) = 

    let width = getWidth map
    let height = getHeight map

    let newMap = createEmptyMap width height

    for y in 1 .. 1 .. height - 2 do 
        for x in 1 .. 1 .. width - 2 do 
            match map[y, x] with 
            | Wall -> () 
            | Field field -> 
                for blizzard in field do
                    let (y, x) = moveBlizzard map blizzard y x width height
                    let targetCell = newMap[y, x]
                    match targetCell with 
                    | Wall -> failwith "WHAT?!"
                    | Field field -> 
                        let newField = Array.append field [| blizzard |]
                        newMap[y, x] <- Field newField
    newMap

let rec walk currentPosition map minute historyOfActions bestResultSoFar targetPosition (cache: Dictionary<(Position * int), option<int>> ) expeditionStart = 

    if currentPosition = expeditionStart && (historyOfActions |> Seq.filter (fun historyPosition -> historyPosition.Y <> expeditionStart.Y) |> Seq.length > 0) then 
        failwith "You are not allowed to move back to starting pos" 
        else 
        ()
    // printPos currentPosition 

    //printfn "(%d,%d). %d" expedition.X expedition.Y minute
    // historyOfActions |> printPositions 
    // printfn ""
    // printfn "" 

    if currentPosition.Y = targetPosition.Y && currentPosition.X = targetPosition.X then 
        printfn "FOUND %d" (minute)
        printfn ""
        Some (minute)
    else    
        let makeSenseToTry = 

            let calcuateDistanceToTarget position = 
                targetPosition.X - position.X + targetPosition.Y - position.Y

            match bestResultSoFar with 
            | None -> true
            | Some bestResult -> 
                let tooFarToReachTargetAndBeatCurrentBestResult = calcuateDistanceToTarget currentPosition + minute >= bestResult
                if tooFarToReachTargetAndBeatCurrentBestResult then false else true

        if not makeSenseToTry then 
            // printfn "makes no sense" 
            None 
        else 
            let mapAfterMove = moveBlizzards map

            // find all possible moves from currentPosition
            let possiblePositions = 
                getPossiblePositions currentPosition mapAfterMove expeditionStart
                // filter out positions that were already visisted up to 4 times
                |> Seq.filter (fun pos -> Seq.filter (fun hisPos -> hisPos = pos) historyOfActions |> Seq.length < 3)
            
            

            // add a possibility to wait (if even possible)
            let possiblePositions = 
                match mapAfterMove[currentPosition.Y, currentPosition.X] with 
                | Wall -> failwith "WHAT?!"
                | Field field -> 
                    if (field |> Array.length) = 0  then 
                        Seq.append possiblePositions [| currentPosition |]
                    else 
                        possiblePositions 

            let mutable bestResult = bestResultSoFar
            
            for newPosition in possiblePositions do 

                let resultOption = 
                    if cache.ContainsKey (newPosition, minute + 1) then 
                        cache[(newPosition, minute + 1)]
                    else 
                        let historyOfActions = Array.append historyOfActions [| newPosition |]
                        let resultOption = walk newPosition mapAfterMove (minute + 1) historyOfActions bestResult targetPosition cache expeditionStart

                        match resultOption with 
                        | None -> 
                            if cache.ContainsKey (newPosition, minute + 1) then 
                                ()
                            else 
                                cache.Add((newPosition, minute + 1), None)
                        | Some result -> 
                            if cache.ContainsKey (newPosition, minute + 1) then 
                                let valueFromCacheOption = cache[(newPosition, minute + 1)]
                                match valueFromCacheOption with 
                                | None -> cache.Add((newPosition, minute + 1), resultOption)
                                | Some valueFromCache -> 
                                    if result < valueFromCache then 
                                        cache.Add((newPosition, minute + 1), resultOption)
                                    else ()
                            else 
                                cache.Add((newPosition, minute + 1), resultOption)
                        
                        resultOption
                match resultOption with 
                | None -> ()
                | Some result -> 
                    // update bestMinute if we found a shorter path
                    match bestResult with 
                    | None -> bestResult <- Some result 
                    | Some best -> if result < best then bestResult <- Some result else ()


            bestResult

let parseMap (input: string[]) = 
    let map = createEmptyMap input[0].Length input.Length

    for y in 0 .. 1 .. input.Length - 1 do 
        for x in 0 .. 1 .. input[0].Length - 1 do 
            let row = input[y]
            let char = row[x]
            let cell = charToBlizzard char
            map[y,x] <- cell
    map 

let part1 maxMinutes (input: string[]) = 

    let width = input[0].Length
    let height = input.Length
    let targetPosition = {Y = height - 1; X =  width - 2}
    let expeditionStart = {Y = 0; X = 1}
    
    let map = parseMap input
    let cache = new Dictionary<(Position * int), option<int>>()
    
    let result = walk expeditionStart map 0 [| |] (Some maxMinutes) targetPosition cache expeditionStart

    printfn "Finished!: %d" result.Value
    result.Value

let part2 maxMinutes (input: string[]) = 

    let mutable totalTime = 0

    let width = input[0].Length
    let height = input.Length
    let map = parseMap input
    let targetPosition = {Y = height - 1; X =  width - 2}
    let expeditionStart = {Y = 0; X = 1}
    let cache = new Dictionary<(Position * int), option<int>>()

    // to the end
    let minutesToTarget = walk expeditionStart map 0 [| |] (Some maxMinutes) targetPosition cache expeditionStart
    totalTime <- minutesToTarget.Value
    printfn "Finished going to target first time!: %d" minutesToTarget.Value

    // back to start again
    let mutable map = map
    for i in 1 .. 1 .. minutesToTarget.Value do 
        map <- moveBlizzards map

    let targetPosition = {Y = 0; X =  1}
    let expeditionStart = {Y = height - 1; X = width - 2}
    let cache = new Dictionary<(Position * int), option<int>>()

    let minutesToTarget = walk expeditionStart map 0 [| |] (Some maxMinutes) targetPosition cache expeditionStart

    printfn "Finished going back!: %d" minutesToTarget.Value
    totalTime <- totalTime + minutesToTarget.Value

    // again to the end
    let mutable map = map
    for i in 1 .. 1 .. minutesToTarget.Value do 
        map <- moveBlizzards map

    let targetPosition = {Y = height - 1; X = width - 2}
    let expeditionStart = {Y = 0; X = 1}
    let cache = new Dictionary<(Position * int), option<int>>()

    let minutesToTarget = walk expeditionStart map 0 [| |] (Some maxMinutes) targetPosition cache expeditionStart

    printfn "Finished going to the end again!: %d" minutesToTarget.Value
    totalTime <- totalTime + minutesToTarget.Value

    totalTime


let part1TestInput () = 
    let result = inputReader.readLines "Day24/testInput.txt" |> part1 19
    if result <> 18 then failwith "wrong answer" else printfn "ok"

let part1RealInput () = 
    let result = inputReader.readLines "Day24/input.txt" |> part1 497 // I was able to find out that 496 is one solution - so we can limit the results to something smaller than that
    if result <> 238 then failwith "wrong answer" else printfn "ok"

let part2TestInput () = 
    let result = inputReader.readLines "Day24/testInput.txt" |> part2 24
    if result <> 54 then failwith "wrong answer" else printfn "ok"

let part2RealInput () = 
    let result = inputReader.readLines "Day24/input.txt" |> part2 300
    if result <> 751 then failwithf "wrong answer %d" result else printfn "ok"

let run () = 
    // part1TestInput () 
    // part1RealInput ()
    part2TestInput ()
    part2RealInput ()
    // TODO there is definitely room for performance improvement here :)