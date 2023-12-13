let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

readLines "testInput.txt" |> printfn "%A"