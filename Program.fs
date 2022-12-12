open Day11_tests
open inputReader

inputReader.readLines "Day12/testInput.txt" |> Array.ofSeq |> Day12.test_part1 
