module Day20

open System.Collections.Generic

type Item = 
    {
        mutable PreviousItem: Item option
        mutable NextItem: Item option
        Number: int
        OriginalPosition: int
    }

let print itemPrep count = 
    let mutable itemToPrint = itemPrep
    for a in 1 .. count do 
        printf "[%d] (%d,%d), " itemToPrint.Number itemToPrint.PreviousItem.Value.Number itemToPrint.NextItem.Value.Number
        itemToPrint <- itemToPrint.NextItem.Value
    printfn ""

let parseInput input = 
    let lookupMap = new Dictionary<int, Item>()
    let mutable previousItem : Item option = None
    let mutable firstItem: Item option = None
    let mutable indexOfZeroNumber = 0
    for (value, index) in input do
        let item : Item = 
            match previousItem with
            | None -> 
                {Number = value; PreviousItem = None; NextItem = None; OriginalPosition = index;}
            | Some previousItem ->
                let item : Item = {Number = value; PreviousItem = Some previousItem; NextItem = None; OriginalPosition = index;}
                previousItem.NextItem <- Some item
                item
        if item.Number = 0 then indexOfZeroNumber <- index else ()
        lookupMap.Add(index, item) |> ignore
        if index = 0 then firstItem <- Some item else ()
        previousItem <- Some item

    let lastItem = previousItem
    lastItem.Value.NextItem <- firstItem
    firstItem.Value.PreviousItem <- lastItem
    (lookupMap, indexOfZeroNumber)

let mix (lookupMap: Dictionary<int, Item>) = 
    for itemToMove in lookupMap do 
        let steps = itemToMove.Value.Number
        let itemToMove = itemToMove.Value

        if steps > 0 then 
            for step in 1 .. steps do 
                let previousItem = itemToMove.PreviousItem.Value
                let nextItem = itemToMove.NextItem.Value
                let nextNextItem = nextItem.NextItem.Value
                // move B out of the link
                previousItem.NextItem <- Some nextItem
                nextItem.PreviousItem <- Some previousItem
                 // move B into the link back
                nextItem.NextItem <- Some itemToMove
                itemToMove.PreviousItem <- Some nextItem

                nextNextItem.PreviousItem <- Some itemToMove
                itemToMove.NextItem <- Some nextNextItem
        else 
            for step in steps .. 1 .. -1 do 
                let previousItem = itemToMove.PreviousItem.Value
                let previousPreviousItem = previousItem.PreviousItem.Value
                let nextItem = itemToMove.NextItem.Value
                // move C out of the link
                previousItem.NextItem <- Some nextItem
                nextItem.PreviousItem <- Some previousItem
                // move C into the link back
                previousItem.PreviousItem <- Some itemToMove
                itemToMove.NextItem <- Some previousItem

                previousPreviousItem.NextItem <- Some itemToMove
                itemToMove.PreviousItem <- Some previousPreviousItem

let findItem firstItem index = 
    let mutable item = firstItem
    for index in 1 .. index do 
        item <- item.NextItem.Value
    item

let run_ input = 
    let input = input |> Array.mapi (fun index item -> (int item, index))
    let (lookupMap, indexOfZeroNumber) = parseInput input

    mix lookupMap |> ignore

    let modulo = lookupMap.Count
    let _1000thItemindex = 1000 % modulo
    let _2000thItemindex = 2000 % modulo
    let _3000thItemindex = 3000 % modulo
    let firstItem = lookupMap[indexOfZeroNumber]

    let mutable answer1000 = findItem firstItem _1000thItemindex
    let mutable answer2000 = findItem firstItem _2000thItemindex
    let mutable answer3000 = findItem firstItem _3000thItemindex

    printfn "Sum %d" (answer1000.Number + answer2000.Number + answer3000.Number)

let run () = 
    let input = inputReader.readLines "Day20/testInput.txt" |> Array.ofSeq
    run_ input
    let input = inputReader.readLines "Day20/input.txt" |> Array.ofSeq
    run_ input