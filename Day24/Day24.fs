module Day24

type Blizzard = char

type Field = array<Blizzard>

type Cell = 
    | Wall
    | Field of Field


let print (map: Cell[,]) width height= 
    for y in 0 .. 1 .. height - 1 do 
        for x in 0 .. 1 .. width - 1 do 
            let cell = map[y, x] 
            match cell with 
            | Wall -> printf "#"
            | Field field -> 
                match List.ofArray field with 
                    | [] -> printf "."
                    | x::[] -> printf "%c" x
                    | _ -> printf "%d" field.Length
        printfn ""


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

    print map width height

    let getPositionToUpdate blizzard x y  width height = 
        match blizzard with 
            | '>' -> 
                match map[y, x + 1] with 
                | Field f -> (x + 1, y, f)
                | Wall -> 
                    match map[y, 1] with 
                    | Field f -> (1, y, f)
                    | Wall -> failwith "not possible"
            | 'v' -> 
                match map[y + 1, x] with 
                | Field f -> (x, y + 1, f)
                | Wall -> 
                    match map[1, x] with 
                    | Field f -> (x, 1, f)
                    | Wall -> failwith "not possible"
            | '<' -> 
                match map[y, x - 1] with 
                | Field f -> (x - 1, y, f)
                | Wall -> 
                    match map[y, width - 2] with 
                    | Field f -> (width - 2, y, f)
                    | Wall -> failwith "not possible"
            | '^' -> 
                match map[x, y - 1] with 
                | Field f -> (x, y - 1, f)
                | Wall -> 
                    match map[height - 2, x] with 
                    | Field f -> (x, height - 2, f)
                    | Wall -> failwith "not possible"
            | _ -> failwith "not possible"

    let moveBlizzard blizzard y x  width height = 
        let (targetX, targetY, _) = getPositionToUpdate blizzard x y width height
        (targetY, targetX)
    
    for i in 0 .. 1 .. 7 do 
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
        map <- newMap
        print map width height
