open advent_of_code_2022
open advent_of_code_2022.Day10

let readLines file = System.IO.File.ReadLines(file)
let input = readLines Day10.inputFile |> Array.ofSeq

Day10.run_part2 input