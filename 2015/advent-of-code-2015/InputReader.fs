module InputReader

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq