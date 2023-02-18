module Day10

open System.Collections.Generic

let lookAndSay2 (input: List<int>) = 
   let mutable result = new List<int>()
   let mutable previousChar = input[0]
   let mutable currentGroupCount = 1
   let mutable index = 1
   while index < input.Count do 
      let char = input[index]
      if char = previousChar then 
         currentGroupCount <- currentGroupCount + 1
      else 
         result.Add currentGroupCount
         result.Add previousChar
         currentGroupCount <- 1
         previousChar <- char
      index <- index + 1
   result.Add currentGroupCount
   result.Add previousChar
   result
   
let toList (a: string) = 
   let mutable result = new List<int>()
   for char in a do 
      let int = char |> string |> int
      result.Add int
   result 

let toString a = 
   a |> Seq.fold (fun acc a -> acc + (a |> string)) ""

let run () =

   let list = new List<int> ()
   list.Add 1
   list |> lookAndSay2 |> toString |>  printfn "%s"

   let mutable input = "1113122113" |> toList

   for i in 1 .. 50 do 
      input <- lookAndSay2 input
      printfn "%d done" i
   input.Count |> printfn "%d"