module Day4

open System.Text.RegularExpressions
open Day4_input

type Area = {
    Start: int
    End: int
}

let parseArea areaAsString =
    let _match = Regex.Matches(areaAsString, "[0-9]+")
    {Start = _match[0].Value |> int; End = _match[1].Value |> int}

let areOverlapping (elf1, elf2) =
   let area1 = parseArea elf1
   let area2 = parseArea elf2
   if (area1.Start <= area2.Start && area1.End >= area2.End) then true
   else if (area2.Start <= area1.Start && area2.End >= area1.End) then true
   else false

let areOverlapping_part2 (elf1, elf2) =
   let area1 = parseArea elf1
   let area2 = parseArea elf2
   if (area1.Start <= area2.Start && area1.End >= area2.Start) then true
   else if (area2.Start <= area1.Start && area2.End >= area1.Start) then true
   else false

let run () =
    let results = Array.map areOverlapping input
    Array.filter id results |> Array.length
    
let run_part2 () =
    let results = Array.map areOverlapping_part2 input
    Array.filter id results |> Array.length
