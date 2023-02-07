module Day24

type Blizzard = char

type Field = array<Blizzard>

type Cell = 
    | Wall
    | Field of Field


let print (map: Cell[][]) = 
    for y in 0 .. 1 .. map.Length - 1 do 
        for x in 0 .. 1 .. map[y].Length - 1 do 
            let cell = map[y][x] 
            match cell with 
            | Wall -> printf "#"
            | Field field -> 
                let charToPrint = 
                    match List.ofArray field with 
                        | [] -> '.'
                        | x::[] -> x
                        | _ -> field.Length |> char
                printf "%c" charToPrint
        printfn ""


let part1 () = 
    let input = inputReader.readLines "Day24/testInput.txt"

    let width = input[0].Length
    let height = input.Length

    let blizzards = seq {
        for y in 0 .. 1 .. input.Length - 1 do 
            for x in 0 .. 1 .. input[0].Length - 1 do 
                match input[y][x] with 
                | '.' -> ()
                | '#' -> ()
                | a -> yield (x, y)
    }

    let map : Cell[][] = Array.create width (Array.create height Wall)

    let charToBlizzard char = 
        match char with 
        | '.' -> Field [||]
        | '#' -> Wall
        | a -> Field [| a |]

    for y in 0 .. 1 .. input.Length - 1 do 
        let row = 
            input[y].ToCharArray() 
            |> Array.map charToBlizzard
        map[y] <- row

    print map

    let getPositionToUpdate blizzard x y = 
        match blizzard with 
            | '>' -> 
                match map[y][x + 1] with 
                | Field f -> (x + 1, y, f)
                | Wall -> 
                    match map[y][1] with 
                    | Field f -> (1, y, f)
                    | Wall -> failwith "not possible"
            | 'v' -> 
                match map[y + 1][x] with 
                | Field f -> (x, y + 1, f)
                | Wall -> 
                    match map[1][x] with 
                    | Field f -> (x, 1, f)
                    | Wall -> failwith "not possible"
            | '<' -> 
                match map[y][x - 1] with 
                | Field f -> (x - 1, y, f)
                | Wall -> 
                    let width = map[y].Length
                    match map[y][width - 2] with 
                    | Field f -> (width - 2, y, f)
                    | Wall -> failwith "not possible"
            | '^' -> 
                match map[x][y - 1] with 
                | Field f -> (x, y - 1, f)
                | Wall -> 
                    let height = map.Length
                    match map[height - 2][x] with 
                    | Field f -> (x, height - 2, f)
                    | Wall -> failwith "not possible"
            | _ -> failwith "not possible"


    let moveBlizzard blizzard remainingBlizzards (map: Cell[][]) y x = 
        let (targetX, targetY, targetField) = getPositionToUpdate blizzard x y
        map[y][x] <- Field remainingBlizzards
        let newTargetField = Array.append targetField [| blizzard |] |> Field
        map[targetY][targetX] <- newTargetField
    
    // TODO all fields should be updated simultaneously, not like now - we end-up moving the same blizzard multiple times
    for i in 0 .. 1 .. 7 do 
        // move blizzards
        for y in 1 .. 1 .. map.Length - 2 do 
            for x in 1 .. 1 .. map[0].Length - 2 do 
                match map[y][x] with 
                | Wall -> () 
                | Field field -> 
                    match List.ofArray field with 
                    | [] -> () // empty field
                    | blizzard::[] -> // field with one blizzard
                        moveBlizzard blizzard [||] map y x
                    | blizzard::blizzards -> // field with multiple blizzards
                        moveBlizzard blizzard (Array.ofList blizzards) map y x
        print map


