module Day13
open System.Collections.Generic

/// [1,1,3,1,1]
///

type Item = 
    | Number of int
    | Array of Item array

let rec parse (signal: string) = 
    let array = new List<Item>()
    let mutable number = ""
    let mutable index = 0
    let mutable keepProcessing = true
    while index < signal.Length && keepProcessing do 
        let char = signal[index]
        if char = ',' 
        then 
            if number.Length <> 0 then 
                array.Add(number |> int |> Item.Number)
            else 
                ()
            number <- ""
            index <- index + 1
        else if char = ']' && number.Length <> 0 
        then 
                array.Add(number |> int |> Item.Number)
                index <- index + 1
                keepProcessing <- false
        else if char = ']' 
        then 
            index <- index + 1
            keepProcessing <- false
        else if char = '[' 
        then 
                let (subArray, subIndex) = parse signal[(index+1)..]
                array.Add(subArray |> Item.Array)
                index <- index + subIndex
                index <- index + 1
        else 
            number <- $"{number}{char}"
            index <- index + 1
        
    (array.ToArray(), index)

type Decision = 
    | Unknown
    | RightOrder
    | WrongOrder

let rec areInWrongOrder (left: Item array) (right: Item array) = 
    let mutable decision = Unknown
    let mutable index = 0

    while index < left.Length do 
        let leftItem = left[index]
        if index >= right.Length 
        then 
            decision <- WrongOrder 
            index <- left.Length
        else 
            let rightItem = right[index]
            match (leftItem, rightItem) with 
            | (Number l, Number r ) -> 
                if r < l 
                then 
                    decision <- WrongOrder
                    index <- left.Length
                else if r = l 
                then ()
                else 
                    decision <- RightOrder
                    index <- left.Length
            | (Array l, Array r) -> 
                decision <- areInWrongOrder l r
            | (Number l, Array r) -> 
                decision <- areInWrongOrder [|Number l|] r
            | (Array l, Number r) -> 
                decision <- areInWrongOrder l [|Number r|] 
            if decision = WrongOrder then index <- left.Length else ()
            index <- index + 1

    decision
    

let part1 (input: string array) = 
    //let expected = [|
    //    true; true; false; true; false; true; false; false 
    //|]

    //let testCase = 1

    //for i in (testCase - 1)*3 .. 3 .. ((testCase - 1)*3 + 2) do 
    //    let (left, _) = input[i] |> parse
    //    let (right, _) = input[i+1] |> parse

    //    let goodOrder = areInWrongOrder left right |> not
    //    printfn $"Expected: {expected[testCase-1]}. Got: {goodOrder}" 
    //    ()

    let mutable sum = 0;
    for i in 0 .. 3 .. input.Length - 1 do 
        let (left, _) = input[i] |> parse
        let (right, _) = input[i+1] |> parse

        let decision = areInWrongOrder left right
        let goodOrder = if decision = RightOrder then true else false
        if goodOrder then sum <- sum + i / 3 + 1 else ()
        //printfn $"{i/3 + 1} Expected: {expected[i/3]}. Got: {goodOrder}" 
        ()
    sum

let testcase () = 
    let result = inputReader.readLines "Day13/testCases.txt" |> Array.ofSeq |> part1
    if ( result ) = 1 then printfn "OK" else printfn $"{result} BAD BAD"

let test_part1 () = 
    let result = inputReader.readLines "Day13/testInput.txt" |> Array.ofSeq |> part1
    if ( result ) = 13 then printfn "OK" else printfn $"{result} BAD BAD"

let run_part1 () = 
    let result = inputReader.readLines "Day13/input.txt" |> Array.ofSeq |> part1
    if ( result ) = 13 then printfn "OK" else printfn $"{result}: BAD BAD"

let run input = 
    testcase ()
    test_part1 ()
    run_part1 ()
    ()