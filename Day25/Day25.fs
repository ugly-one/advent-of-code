module Day25 
open System


//   Decimal          SNAFU
//         1              1
//         2              2
//         3             1=
//         4             1-
//         5             10
//         6             11
//         7             12
//         8             2=
//         9             2-
//        10             20
//        15            1=0
//        20            1-0
//      2022         1=11-2
//     12345        1-0---0
// 314159265  1121-1110-1=0


let toNumber char = 
    match char with 
    | '2' -> 2
    | '1' -> 1
    | '0' -> 0
    | '-' -> -1
    | '=' -> -2
    | _ -> failwith "NOT SUPPORTED"

let toSnafuChar number = 
    match number with 
    | 2 -> "2"
    | 1 -> "1"
    | 0 -> "0"
    | -1 -> "-"
    | -2 -> "="
    | _ -> failwith "NOT SUPPORTED"

let snafuToDecimal (snafu: string) : uint64 = 
    let reversed = Seq.rev snafu

    let get (index: uint64) char = 
        let result = Math.Pow (5,index |> float) * ((toNumber char) |> float)
        result |> uint64

    let decimal = Seq.fold (fun (index: uint64, sum: uint64) char -> (index + (1UL), sum + (get index char))) (0UL, 0UL) reversed
    decimal |> snd

let pow5 y = 
    let five = 5 |> float
    let y = y |> float
    Math.Pow (five, y) |> int


let testSnafuToDecimal snafu (expected: uint64) = 
    let decimal = snafuToDecimal snafu
    if decimal <> expected then failwithf "wrong. %s [SNAFU] = %d [DECIMAL]. %d is not correct" snafu expected decimal else printfn "OK"

let tryGetPlace (snafu: string) (index: int) = 
    if index >= String.length snafu then None else 
        let reversed = Seq.rev snafu |> Array.ofSeq
        Some (reversed[index])

let addTwoSnafu snafu1 snafu2 = 
    let input = [| snafu1; snafu2 |]
    let lastIndex = input |> Seq.fold (fun max line -> if String.length line > max then String.length line else max) 0

    let mutable result = ""
    let mutable columnSum = 0
    for index in 0 .. 1 .. (lastIndex - 1) do 

        for snafu in input do 
            let placeOption = tryGetPlace snafu index
            match placeOption with 
            | None -> ()
            | Some place -> 
                let number = toNumber place
                columnSum <- columnSum + number
        
        let (columnResult, columnOverflow) = 
            if columnSum > 2 then 
                if columnSum = 3 then 
                    (-2, 1) 
                elif columnSum = 4 then 
                    (-1, 1)
                else 
                    failwith "he?!"
            elif columnSum < -2 then 
                if columnSum = -3 then 
                    (2, -1)
                elif columnSum = -4 then 
                    (1, -1)
                else 
                    failwith "he?!"
            else 
                (columnSum, 0)
        result <- (columnResult |> toSnafuChar) + result
        columnSum <- columnOverflow

    if columnSum > 0 then 
        result <- (columnSum |> toSnafuChar) + result
    else ()
    result

let runSum input count expectedSnafu expectedDecimal= 
    let input = input |> Array.take count
    let result = input |> Array.fold (fun sum line -> addTwoSnafu sum line) "0"
    let resultAsDecimal = snafuToDecimal result
    printfn "Result as decimal: %d. Expected decimal: %d" resultAsDecimal expectedDecimal
    if result <> expectedSnafu then failwithf "wrong. Expected %s. Observed: %s" expectedSnafu result else printfn "OK"

let testSum snafu1 snafu2  = 

    let result = addTwoSnafu snafu1 snafu2
    let decimal1 = snafuToDecimal snafu1
    let decimal2 = snafuToDecimal snafu2
    let expectedDecimal = decimal1 + decimal2
    printfn "Snafu: %s" result
    let observedDecimal = snafuToDecimal result
    printfn "Expected decimal: %d" expectedDecimal
    printfn "Observed decimal: %d" observedDecimal
    if expectedDecimal <> observedDecimal then failwithf "%s is not correct (%d). Expected %d" result observedDecimal expectedDecimal else printf "OK"

let run () = 
    
    let input = inputReader.readLines "Day25/testInput.txt"
    let runSum = runSum input
    
    runSum 1 "21" 11
    runSum 2 "1=-" 14
    runSum 3 "1-1" 21
    runSum 4 "21=" 53
    runSum 5 "1=2-" 84
    let result = addTwoSnafu "1=2-" "122"
    if result <> "10-1" then failwith "unable to sum 84 and 37"
    runSum 6 "10-1" 121

    runSum (Array.length input) "2=-1=0" 4890

    testSum "1=" "1-"
    testSum "1=" "1="
