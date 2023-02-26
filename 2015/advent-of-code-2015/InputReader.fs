module InputReader

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq
let readLines2 day = "Day" + day + "/input.txt" |> readLines
let readTestLines2 day = "Day" + day + "/testInput.txt" |> readLines