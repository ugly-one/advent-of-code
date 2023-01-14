module Day20

open System.Collections.Generic

type Item = 
    {
        mutable PreviousItem: Item option
        mutable NextItem: Item option
        Number: int64
        OriginalPosition: int
    }

let print itemPrep count = 
    let mutable itemToPrint = itemPrep
    for a in 1 .. count do 
        printf "%d, " itemToPrint.Number
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
        let forward = itemToMove.Value.Number > 0
        let steps = itemToMove.Value.Number
        let steps = steps % ((lookupMap.Count |> int64) - 1L)
        let itemToMove = itemToMove.Value

        let steps = steps |> abs 
        if forward then 
            for step in 1L .. steps do 
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
            for step in 1L .. steps do 
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

let runPart1 input = 
    let input = input |> Array.mapi (fun index item -> (int64 item, index))
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

let runPart2 input = 
    let decryptionKey = 811589153L

    let input = input |> Array.mapi (fun index item -> ((int64 item) * decryptionKey, index))
    let (lookupMap, indexOfZeroNumber) = parseInput input

    for i in 1 .. 10 do 
        mix lookupMap |> ignore

    let modulo = lookupMap.Count
    let _1000thItemindex = 1000 % modulo
    let _2000thItemindex = 2000 % modulo
    let _3000thItemindex = 3000 % modulo
    let firstItem = lookupMap[indexOfZeroNumber]

    let mutable answer1000 = findItem firstItem _1000thItemindex
    let mutable answer2000 = findItem firstItem _2000thItemindex
    let mutable answer3000 = findItem firstItem _3000thItemindex
    printfn "%d %d %d" answer1000.Number answer2000.Number answer3000.Number
    printfn "Sum %d" (answer1000.Number + answer2000.Number + answer3000.Number)

let run () = 
    let testInput = inputReader.readLines "Day20/testInput.txt" |> Array.ofSeq
    runPart1 testInput
    let input = inputReader.readLines "Day20/input.txt" |> Array.ofSeq
    runPart1 input
    runPart2 testInput
    runPart2 input