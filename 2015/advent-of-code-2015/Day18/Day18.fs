module Day18

open System.Collections.Generic
open System.Text.RegularExpressions

let print (array: char[,]) =
    let size1 = (Array2D.length1 array)
    let size2 = (Array2D.length2 array)

    for y in 0 .. 1 .. size1 - 1 do
        for x in 0 .. 1 .. size2 - 1 do
            printf "%c" array[x,y]
        printfn ""
    ()

let getNeighbors x y max =
    [|
       (x-1, y-1)
       (x-1, y)
       (x-1, y+1)
       (x, y-1)
       (x, y+1)
       (x+1, y-1)
       (x+1, y)
       (x+1, y+1)
    |]
    |> Array.filter (fun (x,y) -> x >= 0 && x <= max && y >= 0 && y <= max)
    
let run () =
    let input = InputReader.readLines "Day18/input.txt"
    let size = input.Length
    
    let mutable array = Array2D.init size size (fun x y -> input[y][x])
    array[0,0] <- '#'
    array[size - 1,0] <- '#'
    array[0, size - 1] <- '#'
    array[size - 1, size - 1] <- '#'

        
    for step in 1 .. 1 .. 100 do
        
        let newArray = Array2D.copy array
        for x in 0 .. (size - 1) do
            for y in 0 .. (size - 1) do
                if (x = 0 && y = 0) then ()
                elif (x = 0 && y = size - 1) then ()
                elif (x = size - 1 && y = 0) then ()
                elif (x = size - 1 && y = size - 1) then ()
                else                 
                    let neighbors = getNeighbors x y (size - 1)
                    let charsOfNeighbors = neighbors |> Array.map (fun (x,y) -> array[x,y])
                    let onLightsOfNeighbors = charsOfNeighbors |> Array.filter (fun c -> c = '#') |> Array.length
                    let currentPoint = array[x,y]
                    match (currentPoint, onLightsOfNeighbors) with
                    | '#', 2 | '#', 3 -> ()
                    | '#', _ -> newArray[x,y] <- '.'
                    | '.', 3 -> newArray[x,y] <- '#'
                    | _ -> ()
        array <- newArray
    let mutable on = 0
    
    array |> Array2D.iter (fun i -> if i = '#' then on <- on + 1 else ())
    
    on |> printfn "%d"
