module Day13
open System.Collections.Generic

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

            if decision <> Unknown then index <- left.Length else ()
            index <- index + 1
    if decision = Unknown && left.Length < right.Length then RightOrder else decision
    

let part1 (input: string array) = 
    let mutable sum = 0;
    for i in 0 .. 3 .. input.Length - 1 do 
        let (left, _) = input[i] |> parse
        let (right, _) = input[i+1] |> parse
        let decision = areInWrongOrder left right
        let goodOrder = if decision = RightOrder then true else false
        if goodOrder then sum <- sum + i / 3 + 1 else ()
        ()
    sum

let isBigger (left: Item array) (right: Item array) = 
    let result = areInWrongOrder left right
    if result = WrongOrder then 1 else -1

let part2 (input: string array) = 
    
    let items = new List<Item array>()
    for line in input do 
        if (line = "") then () else
            let (item, _) = line |> parse
            items.Add(item)
    
    let item2 = [| Array [|Number 2|] |]
    let item6 = [| Array [|Number 6|] |]
    items.Add(item2)
    items.Add(item6)

    let comparer = Comparer.Create(fun a b -> isBigger a b)
    items.Sort(comparer)
    let mutable indexOf2 = 0
    let mutable indexOf6 = 0
    for i in 0 .. items.Count - 1 do 
        if (items[i] = item2) then indexOf2 <- i + 1 else ()
        if (items[i] = item6) then indexOf6 <- i + 1 else ()
    indexOf2 * indexOf6

let testcase file expectedValue part = 
    let result = inputReader.readLines file |> Array.ofSeq |> part
    if result = expectedValue then printfn "OK" else printfn $"Wrong answer: {result}. expected {expectedValue}"

let run () = 
    testcase "Day13/testInput.txt" 13 part1
    testcase "Day13/testCases.txt" 1 part1
    testcase "Day13/input.txt" 6076 part1
    testcase "Day13/testInput.txt" 140 part2
    testcase "Day13/input.txt" 24805 part2
    ()