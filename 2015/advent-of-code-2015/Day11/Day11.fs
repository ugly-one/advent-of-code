module Day11

open System.Collections.Generic
open System.Text.RegularExpressions

let toArray text = 
   let result = new List<char>()
   for char in text do 
      result.Add char
   result |> Array.ofSeq

let toString text = 
   let mutable result = ""
   for char in text do 
      result <- result + (char |> string)
   result
   
let increaseChar c = 
   if c = 'z' then 
      ('a', true)
   else 
      let int = c |> int
      let newInt = int + 1
      let newChar = newInt |> char
      (newChar, false)

let increase (text: char[]) =
   let text = text |> Array.rev
   let result = Array.copy text
   let mutable keepGoing = true
   let mutable index = 0
   while keepGoing do 
      let char = text[index]
      let (newChar, overflown) = char |> increaseChar
      result[index] <- newChar
      if overflown then 
         index <- index + 1
         ()
      else keepGoing <- false
   result |> Array.rev

let meetsRequirements (text: char[]) =
   let rule1 (text: char[]) = 
      let int1 = text[0] |> int
      let int2 = text[1] |> int
      let int3 = text[2] |> int
      if int1 + 1 = int2 && int2 + 1 = int3 then true else false

   let mutable rule1passed = false
   for i in 0 .. 1 .. (text.Length - 3) do 
      let subText = text[i..i + 2]
      rule1passed <- rule1passed || rule1 subText

   let rule2 char = 
      match char with 
      | 'i' | 'l' | 'o' -> false
      | _ -> true

   let rule2passed = text |> Array.fold (fun acc c -> rule2 c && acc) true
   
   let mutable pairs = Set.empty
   for i in 0 .. 1 .. (text.Length - 2) do 
      let subText = text[i..i+1]
      if subText[0] = subText[1] then 
         pairs <- Set.add subText pairs
      else 
         ()
   let rule3passed = if Set.count pairs > 1 then true else false

   rule1passed && rule2passed && rule3passed

let run () =
   'z' |> increaseChar |> fst |> printfn "%c"
   let do_ input = 
      input |> toArray |> increase |> toString |> printfn "%s"
   "aa" |> do_
   "azz" |> do_

   "hijklmmn" |> toArray |> meetsRequirements |> printfn "%b"
   "abcdffaa" |> toArray |> meetsRequirements |> printfn "%b"

   let mutable password = "vzbxxyzz" |> toArray
   password <- increase password
   while meetsRequirements password |> not do 
      password <- increase password
   
   password |> toString |> printfn "%s"
