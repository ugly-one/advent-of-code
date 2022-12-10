open System.Diagnostics
open advent_of_code_2022
open advent_of_code_2022.Day10

let readLines file = System.IO.File.ReadLines(file)

let measure action label =
    let stopWatch = new Stopwatch()
    stopWatch.Start()
    let b = action ()
    let result = stopWatch.ElapsedMilliseconds
    printfn $"%s{label}: %d{result}"
    ()

measure (fun () -> Day5.run2 ()) "day5_2"

measure (fun () -> Day6.run1 ()) "day6_1"
measure (fun () -> Day6.run2 ()) "day6_2"

measure (fun () -> Day7.run1 Day7_input.input) "day7_1"
measure (fun () -> Day7.run2 Day7_input.input) "day7_2"

measure (fun () -> Day8.run1 Day8_input.input) "day8_1"
measure (fun () -> Day8.run2 Day8_input.input) "day8_2"

let day9_input = readLines "Day9/input.txt" |> Array.ofSeq
measure (fun () -> Day9.run1 day9_input) "day9_1"
measure (fun () -> Day9.run2 day9_input) "day9_2"

let day10_input = readLines "Day10/input.txt" |> Array.ofSeq
measure (fun () -> Day10.run1 day10_input) "day10_1"
measure (fun () -> Day10.run2 day10_input true) "day10_2"