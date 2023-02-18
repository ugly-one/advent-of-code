module Day8

open System.Text.RegularExpressions 

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let count (string: string) = 
   let string = string[1..string.Length - 2]
   let mutable charactersInMemory = Array.empty
   let mutable index = 0
   while index < string.Length do 
      let character1 = string[index]
      if index + 1 = string.Length 
      then 
         charactersInMemory <- charactersInMemory |> Array.append [| character1 |]
         index <- index + 1
      else 
         let character2 = string[index+1]
         match (character1, character2) with 
            | '\\', '\\' -> 
               charactersInMemory <- charactersInMemory |> Array.append [| character2 |]
               index <- index + 2
            | '\\', '"' -> 
               charactersInMemory <- charactersInMemory |> Array.append [| character2 |]
               index <- index + 2
            | '\\', 'x' -> 
               let hex = string[index + 2 .. index + 3]
               let isHex = Regex.Match(hex, "[0-9a-fA-F]+")
               if isHex.Success then 
                  charactersInMemory <- charactersInMemory |> Array.append [| '?' |]
                  index <- index + 4
               else 
                  charactersInMemory <- charactersInMemory |> Array.append [| character1 |]
                  index <- index + 1
            | _ -> 
               charactersInMemory <- charactersInMemory |> Array.append [| character1 |]
               index <- index + 1
   (charactersInMemory.Length, string.Length + 2)

let count2 (string: string) = 
   let mutable sum = 0
   for char in string do
      let characters =  
         match char with 
         | '"' -> 2
         | '\\' -> 2
         | _ -> 1
      sum <- sum + characters
   (string.Length, sum + 2)

let print (x,y) = 
   printfn "string literals: %d in memory: %d" x y

let run () =

   let do_ string = 
      string |> count |> print

   "\"\"" |> do_
   "\"abc\"" |> do_
   "\"aaa\\\"aaa\"" |> do_
   "\"\\x27\"" |> do_
   "\"\\\\\\x66\"" |> do_
   "\"\\\\\\\\x66\"" |> do_
   "\"\\\"o\"" |> do_

   let input = readLines "Day8/input.txt"

   let fold diff line = 
      let (a, b) = count line
      diff + (b - a)

   input |> Array.fold fold 0 |> printfn "%d"

   "\"\\x27\"" |> count2 |> print

   let fold2 acc line = 
      let (a, b) = count2 line
      acc + (b - a)

   input |> Array.fold fold2 0 |> printf "%d"