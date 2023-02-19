module Day12

open System.Collections.Generic
open System.Text.RegularExpressions

let printResult input (s,e,o) = 
   printfn "%s: %d %d %b" input s e o

let findReds input = 
   Regex.Matches(input, "red")
   |> Seq.map ( fun match_ -> match_.Index)

let tryFindStartEndOfObject (input: string) index = 
   let mutable keepGoing = true
   let mutable currentIndex = index
   let mutable isObject = false
   let mutable startIndex = currentIndex
   let mutable objectBracketsToSkip = 0
   let mutable arrayBracketsToSkip = 0
   while keepGoing do 
      let previousIndex = currentIndex - 1
      let previousChar = input[previousIndex]
      match previousChar with 
      | '}' -> objectBracketsToSkip <- objectBracketsToSkip + 1
      | '{' -> 
         if objectBracketsToSkip > 0 then objectBracketsToSkip <- objectBracketsToSkip - 1
         else 
            isObject <- true
            startIndex <- previousIndex
            keepGoing <- false
      | '[' -> 
         if arrayBracketsToSkip > 0 then arrayBracketsToSkip <- arrayBracketsToSkip - 1
         else
            isObject <- false
            keepGoing <- false
      | ']' -> arrayBracketsToSkip <- arrayBracketsToSkip + 1
      | _ -> ()

      currentIndex <- currentIndex - 1
      if currentIndex < 0 then keepGoing <- false else ()

   let mutable keepGoing = true
   let mutable currentIndex = index
   let mutable isObject = false
   let mutable endIndex = index
   let mutable objectBracketsToSkip = 0
   let mutable arrayBracketsToSkip = 0
   while keepGoing do 
      let nextIndex = currentIndex + 1
      let nextChar = input[nextIndex]
      match nextChar with 
      | '{' -> objectBracketsToSkip <- objectBracketsToSkip + 1
      | '}' -> 
         if objectBracketsToSkip > 0 then objectBracketsToSkip <- objectBracketsToSkip - 1
         else 
            isObject <- true
            endIndex <- nextIndex
            keepGoing <- false
      | ']' -> 
         if arrayBracketsToSkip > 0 then 
            arrayBracketsToSkip <- arrayBracketsToSkip - 1
         else
            isObject <- false
            keepGoing <- false
      | '[' -> arrayBracketsToSkip <- arrayBracketsToSkip + 1
      | _ -> ()

      currentIndex <- currentIndex + 1
      if currentIndex >= input.Length then keepGoing <- false else ()
   (startIndex, endIndex, isObject)

let getnumbers input = 
   let matches = Regex.Matches(input, "[-0-9]+") 
   matches |> Seq.map (fun match_ -> (match_.Index, match_.Value |> string |> int))

let isWithin item ranges =
   let isWithin_ item (s,e) = if item > s && item < e then true else false
   ranges |> Seq.fold (fun acc range -> acc || (isWithin_ item range) ) false

let run () =
   // part1
   let input = InputReader.readLines "Day12/input.txt" 
   Regex.Matches(input[0], "[-0-9]+") 
   |> Seq.fold (fun acc match_ -> acc + (match_.Value |> string |> int)) 0
   |> printfn "%d"
   // part2
   let input = input[0]

   let numbers = getnumbers input
   let subStringsToSkip = 
      input 
      |> findReds
      |> Seq.map (fun red -> tryFindStartEndOfObject input red)
      |> Seq.filter (fun (start, end_, object) -> object)
      |> Seq.map (fun (start, end_, object) -> (start, end_))

   let numbers = 
      numbers 
      |> Seq.filter (fun (index, number) -> isWithin index subStringsToSkip |> not)

   numbers 
   |> Seq.map (fun (index, number) -> number) 
   |> Seq.sum
   |> printfn "%d"
   

   