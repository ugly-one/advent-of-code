module Day24

type Blizzard = char

type Field = array<Blizzard>

type Cell = 
    | Wall
    | Field of Field

type Position = {
    X: int;
    Y: int;
}

let print (map: Cell[,]) (expeditionY, expeditionX) width height= 
    for y in 0 .. 1 .. height - 1 do 
        for x in 0 .. 1 .. width - 1 do 
            let cell = map[y, x] 
            match cell with 
            | Wall -> printf "#"
            | Field field -> 
                match List.ofArray field with 
                    | [] -> if expeditionY = y && expeditionX = x then printf "E" else printf "."
                    | x::[] -> printf "%c" x
                    | _ -> printf "%d" field.Length
        printfn ""

let expeditionStart = {Y = 0; X = 1}

let isInitialPosition pos = 
    pos = expeditionStart

let part1 (input: string[]) = 

    let width = input[0].Length
    let height = input.Length

    let createEmptyMap () = 
        let getCell y x = 
            if y = 0 || x = 0 || y = height - 1 || x = width - 1 
            then 
                if (y = 0 && x = 1) || (y = height - 1 && x = width - 2) then Field[| |] else Wall 
            else Field [| |]
        Array2D.init height width (fun y x -> getCell y x )

    let mutable map = createEmptyMap ()

    let charToBlizzard char = 
        match char with 
        | '.' -> Field [||]
        | '#' -> Wall
        | a -> Field [| a |]

    for y in 0 .. 1 .. input.Length - 1 do 
        for x in 0 .. 1 .. input[0].Length - 1 do 
            let row = input[y]
            let char = row[x]
            let cell = charToBlizzard char
            map[y,x] <- cell

    let getPositionToUpdate blizzard x y  width height = 
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

    let moveBlizzard blizzard y x  width height = 
        let (targetY, targetX) = getPositionToUpdate blizzard x y width height
        (targetY, targetX)
    
    let getPossiblePositions position (map: Cell[,]) = 
        let x = position.X
        let y = position.Y
        let allPosiblePositions = seq {
            yield {Y = y; X = x + 1}
            yield {Y = y + 1; X = x}
            yield {Y = y - 1; X =  x}
            yield {Y = y; X = x - 1}
        }

        let isAvailable cell = 
            match cell with 
            | Wall -> false
            | Field field -> if Array.length field = 0 then true else false

        let isWithinMap pos = 
            if pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height then true else false

        let positions = allPosiblePositions |> Seq.filter (fun pos -> isWithinMap pos && isAvailable map[pos.Y, pos.X] && isInitialPosition pos |> not)
        if Seq.contains expeditionStart positions 
            then 
                failwith "WHAT?!"
            else 
                ()
        positions
    
    let moveBlizzards (map: Cell[,]) = 
        let newMap = createEmptyMap ()

        for y in 1 .. 1 .. height - 2 do 
            for x in 1 .. 1 .. width - 2 do 
                match map[y, x] with 
                | Wall -> () 
                | Field field -> 
                    for blizzard in field do
                        let (y, x) = moveBlizzard blizzard y x width height
                        let targetCell = newMap[y, x]
                        match targetCell with 
                        | Wall -> failwith "WHAT?!"
                        | Field field -> 
                            let newField = Array.append field [| blizzard |]
                            newMap[y, x] <- Field newField
        newMap

    let targetPosition = {Y = height - 1; X =  width - 2}

    let printPos position = 
        printf "(%d,%d)" position.X position.Y

    let printHistory (history: Position[]) = 
        for item in history do 
            printPos item
            printf ","

    let rec walk expedition map minute historyOfActions minMinuteSoFar = 

        if expedition.Y = 0 && expedition.X = 1 && (historyOfActions |> Seq.filter (fun historyPosition -> historyPosition.Y <> 0) |> Seq.length > 0) then 
            failwith "You are not allowed to move back to starting pos" 
            else 
            ()
        //printfn "(%d,%d). %d" expedition.X expedition.Y minute

        if expedition.Y + 1 = (targetPosition.Y) && expedition.X = (targetPosition.X) then 
            printfn "FOUND %d" minute
            printHistory historyOfActions
            printfn ""
            Some minute 
        else    
            let makeSenseToEvenTry = 
                match minMinuteSoFar with 
                | None -> 
                    if minute < 700 then true else false // rather arbitrary max limit, but seems to work, it appears that for the real input, I can find a solution in 634 minutes (which is not the most optimal)
                | Some minTime -> if minute >= minTime then false else true

            if not makeSenseToEvenTry then None 
            else 
                let mapAfterMove = moveBlizzards map
                let possiblePositions = getPossiblePositions expedition mapAfterMove
                
                // add a possibility to wait (if even possible)
                let possiblePositions = 
                    match mapAfterMove[expedition.Y, expedition.X] with 
                    | Wall -> failwith "WHAT?!"
                    | Field field -> 
                        if (field |> Array.length) = 0  then 
                            Seq.append possiblePositions [| expedition |]
                        else 
                            possiblePositions 

                let mutable bestMinute = minMinuteSoFar
                for newPosition in possiblePositions do 
                    let historyOfActions = Array.append historyOfActions [| newPosition |]
                    let minutes = walk newPosition mapAfterMove (minute + 1) historyOfActions bestMinute
                    match minutes with 
                    | None -> ()
                    | Some m -> 
                                // update bestMinute if we found a shorter path
                                match bestMinute with 
                                | None -> bestMinute <- Some m 
                                | Some best -> if m < best then bestMinute <- Some m else ()

                bestMinute

    let result = walk expeditionStart map 1 [| |] None

    printfn "%d" result.Value


let part1TestInput () = 
    inputReader.readLines "Day24/testInput.txt" |> part1


let part1RealInput () = 
    inputReader.readLines "Day24/input.txt" |> part1

let run () = 
    part1TestInput () 
    part1RealInput ()