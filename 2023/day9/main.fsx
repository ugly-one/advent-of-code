#load "../general.fsx"

open System.Collections.Generic

let test1 = "0 3 6 9 12 15"
let test2 = "1 3 6 10 15 21"
let test3 = "10 13 16 21 30 45"

let testInput = [| test1; test2; test3 |]

let toNumbers (line: string) =
    line.Split(' ') |> Array.map (fun n -> n |> int)

let calculateDiffs (numbers: int array) =
    let result: List<int> = List<int>()
    let differences =
        numbers[1..] |>
        (Array.fold (fun previous current ->
                        result.Add(current - previous)
                        current)
                    (numbers[0]))
    result
 
let areAllZeros (numbers: int array) =
    numbers |> Array.fold (fun state number -> state && number = 0) true
    
let rec calculateDiffsRec (numbers: int array) =
    if areAllZeros numbers then numbers[(numbers.Length-1)]
    else
        let diffs = calculateDiffs numbers |> Array.ofSeq
        let lastNumberFromDiffs = calculateDiffsRec diffs
        let lastNumber = numbers[(numbers.Length-1)]
        let newLastNumber = lastNumber + lastNumberFromDiffs
        newLastNumber   

let rec calculateDiffsRec_part2 (numbers: int array) =
    if areAllZeros numbers then numbers[0]
    else
        let diffs = calculateDiffs numbers |> Array.ofSeq
        let firstNumberFromDiffs = calculateDiffsRec_part2 diffs
        let firstNumber = numbers[0]
        let newfirstNumber = firstNumber - firstNumberFromDiffs
        newfirstNumber   
  
let getNextValue (line) =
    line |>toNumbers |> calculateDiffsRec
    
let getSum input =
    input |> Array.map (toNumbers) |> Array.map calculateDiffsRec |> Array.sum |> General.print
    
let getSum_part2 input =
    input |> Array.map (toNumbers) |> Array.map calculateDiffsRec_part2 |> Array.sum |> General.print
    
testInput |> getSum
General.readLines "input.txt" |> getSum

[| test3 |] |> getSum_part2
testInput |> getSum_part2
General.readLines "input.txt" |> getSum_part2

