module Day12

open System.Collections.Generic
open System.Text.RegularExpressions

let run () =
   let input = InputReader.readLines "Day12/input.txt" 
   Regex.Matches(input[0], "[-0-9]+") 
   |> Seq.fold (fun acc match_ -> acc + (match_.Value |> string |> int)) 0
   |> printfn "%d"