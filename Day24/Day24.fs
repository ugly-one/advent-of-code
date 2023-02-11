module Day24

type Blizzard = char

type Field = array<Blizzard>

type Cell = 
    | Wall
    | Field of Field

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

let isNotInitialPosition y x = 
    if y = 0 && x = 1 then false else true

let part1 () = 
    let input = inputReader.readLines "Day24/testInput.txt"

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
    
    let mutable expedition = (0, 1)

    let getPossiblePositions (y,x) (map: Cell[,]) = 
        let allPosiblePositions = seq {
            yield (y - 1, x)
            yield (y, x - 1)
            yield (y, x + 1)
            yield (y + 1, x)
        }

        let isAvailable cell = 
            match cell with 
            | Wall -> false
            | Field field -> if Array.length field = 0 then true else false
        let isWithinMap y x = 
            if x >= 0 && x < width && y >= 0 && y < height then true else false

        allPosiblePositions |> Seq.filter (fun (y,x) -> isWithinMap y x && isAvailable map[y, x] && isNotInitialPosition y x)
    
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

    let targetPosition = (height - 1, width - 2)

    let rec walk (expeditionY, expeditionX) map minute historyOfActions minMinuteSoFar = 

        if expeditionY = 0 && expeditionX = 1 && (Seq.length historyOfActions > 0) then failwith "You are not allowed to move back to starting pos" else ()
        printfn "%d %d" expeditionX expeditionY
        // print map expedition width height

        if expeditionY + 1 = (targetPosition |> fst) && expeditionX = (targetPosition |> snd) then 
            Some minute 
        else    
            let makeSenseToEvenTry = 
                match minMinuteSoFar with 
                | None -> true
                | Some minTime -> if minute > minTime then false else true

            if not makeSenseToEvenTry then None 
            else 
                let mapAfterMove = moveBlizzards map
                let possiblePositions = getPossiblePositions (expeditionY, expeditionX) mapAfterMove
                
                let visitedTheSamePlaceFewTimes y x = 
                    historyOfActions 
                    |> Seq.filter (fun (hisY,hisX) -> x = hisX && y = hisY)
                    |> Seq.length < 5

                // try to avoid walking in circles by checking if the same place has already been visited multiple times
                let possiblePositions = 
                    if possiblePositions |> Seq.length = 0 then possiblePositions else 
                    possiblePositions |> Seq.filter (fun (y,x) -> visitedTheSamePlaceFewTimes y x)

                // add a possibility to wait (if even possible)
                let possiblePositions = 
                    match mapAfterMove[expeditionY, expeditionX] with 
                    | Wall -> failwith "WHAT?!"
                    | Field field -> 
                        if (field |> Array.length) = 0  then 
                            Seq.append possiblePositions [| (expeditionY, expeditionX) |]
                        else 
                            possiblePositions 

                let mutable bestMinute = None
                for newPosition in possiblePositions do 
                    let historyOfActions = Array.append historyOfActions [| newPosition |]
                    let minutes = walk newPosition mapAfterMove (minute + 1) historyOfActions bestMinute
                    match minutes with 
                    | None -> ()
                    | Some m -> match bestMinute with 
                                | None -> bestMinute <- Some m 
                                | Some best -> if m < best then bestMinute <- Some m else ()
                bestMinute

    let result = walk expedition map 1 [| |] None

    printfn "%d" result.Value
