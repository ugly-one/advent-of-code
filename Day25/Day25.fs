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

let rec decimalToSnafu (decimal: uint64) = 
    if decimal = 0UL then "" else 
        decimalToSnafu (decimal/5UL) + ((decimal % 5UL) |> string)

let testSnafuToDecimal snafu (expected: uint64) = 
    let decimal = snafuToDecimal snafu
    if decimal <> expected then failwithf "wrong. %s [SNAFU] = %d [DECIMAL]. %d is not correct" snafu expected decimal else printfn "OK"

let testDecimalToSnafu decimal expected = 
    let snafu = decimalToSnafu decimal
    if snafu <> expected then failwithf "wrong. %s [SNAFU] = %d [DECIMAL]. %s is not correct" expected decimal snafu else printfn "OK"

let tryGetPlace (snafu: string) (index: int) = 
    if index >= String.length snafu then None else 
        let reversed = Seq.rev snafu |> Array.ofSeq
        Some (reversed[index])

let run () = 
    
    let input = inputReader.readLines "Day25/testInput.txt"

   // let input = [| "122"; "1"|]

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
        
        let columnResult = if columnSum > 2 then -2 else columnSum
        let overflowOption = if columnSum > 2 then Some (columnSum - 2) else None
        result <- (columnResult |> toSnafuChar) + result
        match overflowOption with 
        | None -> columnSum <- 0
        | Some overflow -> columnSum <- overflow

    printf "%s" result
