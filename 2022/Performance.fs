module Performance

open System.Diagnostics
open advent_of_code_2022
open advent_of_code_2022.Day10
open inputReader

let measure action label =
     let stopWatch = new Stopwatch()
     stopWatch.Start()
     let b = action ()
     let result = stopWatch.ElapsedMilliseconds
     printfn $"%s{label}: %d{result} ms"
     b

let run () =

    let input = readLines "Day15/input.txt"

    Day13.testcase "Day15/testInput.txt" 26 (Day15.part1 10)
    Day13.testcase "Day15/testInput.txt" 56000011UL (Day15.part2 20)

    let a = measure (fun () -> Day15.part1 2000000 input ) "day15 part1"
    let b = measure (fun () -> Day15.part2 4000000 input ) "day15 part2"
    printfn "%i  " a
    printfn "%i" b


    //measure (fun () -> Day12.part2 ()) "day12_2"

    //measure (fun () -> Day12.part1 ()) "day12_1"

    //measure (fun () -> Day12.part2 ()) "day12_2"

    //measure (fun () -> Day5.run2 true) "day5_2"
    //measure (fun () -> Day5.run2 false) "day5_2"


    //measure (fun () -> Day6.run1 ()) "day6_1"
    //measure (fun () -> Day6.run2 ()) "day6_2"

    //measure (fun () -> Day7.run1 Day7_input.input) "day7_1"
    //measure (fun () -> Day7.run2 Day7_input.input) "day7_2"

    //let expectedPart1 = 1835
    //let expectedPart2 = 263670

    //if measure (fun () -> Day8.run1 Day8_input.input) "day8_1" = expectedPart1 then () else failwith "part 1 is broken" 
    //if measure (fun () -> Day8.run2 Day8_input.input) "day8_2" = expectedPart2 then () else failwith "part 2 is broken"

    //let day9_input = readLines "Day9/input.txt" |> Array.ofSeq
    //let size = day9_input.Length
    //printfn "%d" size
    //if measure (fun () -> Day9.run2 day9_input) "day9_2"  = 2511 then () else failwith "broken"
    //if measure (fun () -> Day9.run1 day9_input) "day9_1" = 6332 then () else failwith "broken"

    //let day10_input = readLines "Day10/input.txt" |> Array.ofSeq
    //measure (fun () -> Day10.run1 day10_input) "day10_1"
    //measure (fun () -> Day10.run2 day10_input true) "day10_2"