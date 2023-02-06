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

