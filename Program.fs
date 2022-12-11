open System.Diagnostics
open advent_of_code_2022
open advent_of_code_2022.Day10

let readLines file = System.IO.File.ReadLines(file)

let testInput = readLines "Day11/testInput.txt"
let result = Day11.run testInput
if result = 10605 then printfn "DONE" else failwith $"wrong result {result}" 

//
// let measure action label =
//     let stopWatch = new Stopwatch()
//     stopWatch.Start()
//     let b = action ()
//     let result = stopWatch.ElapsedMilliseconds
//     printfn $"%s{label}: %d{result}"
//     b
//
// measure (fun () -> Day5.run2 true) "day5_2"
// measure (fun () -> Day5.run2 false) "day5_2"
//
//
// measure (fun () -> Day6.run1 ()) "day6_1"
// measure (fun () -> Day6.run2 ()) "day6_2"
//
// measure (fun () -> Day7.run1 Day7_input.input) "day7_1"
// measure (fun () -> Day7.run2 Day7_input.input) "day7_2"
//
// let expectedPart1 = 1835
// let expectedPart2 = 263670
//
// if measure (fun () -> Day8.run1 Day8_input.input) "day8_1" = expectedPart1 then () else failwith "part 1 is broken" 
// if measure (fun () -> Day8.run2 Day8_input.input) "day8_2" = expectedPart2 then () else failwith "part 2 is broken"
//
// let day9_input = readLines "Day9/input.txt" |> Array.ofSeq
// let size = day9_input.Length
// printfn "%d" size
// if measure (fun () -> Day9.run2 day9_input) "day9_2"  = 2511 then () else failwith "broken"
// if measure (fun () -> Day9.run1 day9_input) "day9_1" = 6332 then () else failwith "broken"
//
// let day10_input = readLines "Day10/input.txt" |> Array.ofSeq
// measure (fun () -> Day10.run1 day10_input) "day10_1"
// measure (fun () -> Day10.run2 day10_input true) "day10_2"