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


   let numberOfCharactersOfCode = string.Length + 2
   let numberOfCharactersInMemory = charactersInMemory.Length
   (numberOfCharactersInMemory, numberOfCharactersOfCode)

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
   // let testInput = [|
   //    "\"\""
   //    "\"abc\""
   //    "\"aaa\\\"aaa\""
   //    "\"\\x27\""
   // |]

   let input = readLines "Day8/input.txt"

   let fold diff line = 
      let (a, b) = count line
      printfn "%s = %d %d" line a b
      diff + (b - a)

   // testInput |> Array.fold fold 0 |> printfn "%d"   
   input |> Array.fold fold 0 |> printfn "%d"