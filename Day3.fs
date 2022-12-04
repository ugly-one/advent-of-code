module Day3

open Day3_input
open System.Collections.Generic

let rec foldRucksack length (result: char) (currentIndex: int) (firstCompartment: HashSet<char>) (items: char[]) =
    if items.Length = 0 then result
    else 
        let item = items[0]
        if currentIndex < length /2 then 
            // we are in the first compartment
            firstCompartment.Add item |> ignore
            foldRucksack length result (currentIndex + 1) firstCompartment items[1..]
        else 
            // we are in the second compartment
            if firstCompartment.Contains item then item
            else foldRucksack length result (currentIndex + 1) firstCompartment items[1..]

let inline charToPriority (c: char) = 
    let result = int c - int '0'   
    if (result > 48) then result - 48
    else result + 10 

let checkRucksack (items: string) = 
    let itemsAsArray = items.ToCharArray()
    let character = foldRucksack itemsAsArray.Length ' ' 0 (new HashSet<char>()) itemsAsArray
    let priority = charToPriority character 
    priority

let run () = 
    Array.fold (fun state item -> state + (checkRucksack item)) 0 input
    
let lineToHashSet (line: string) =
    let hashSet = new HashSet<char>()
    let arrayInput = line.ToCharArray()
    Array.fold (fun (state: HashSet<char>) item -> (state.Add item) |> ignore; state) hashSet arrayInput

let checkGroup (group: string[]) =
    let line1 = group[0].ToCharArray()
    let hashSetGroup = Array.map (lineToHashSet) [| group[1]; group[2]|]
    let result = Array.fold (fun state item -> if hashSetGroup[0].Contains item && hashSetGroup[1].Contains item then Some(item) else state) None line1
    result.Value

let createGroups (lines: string[]) =
    seq{
        for groupCount in 0..((lines.Length / 3)-1) do
            let groupIndex = groupCount * 3;
            yield [|lines[groupIndex]; lines[groupIndex+1]; lines[groupIndex+2]|]
    }
let run_part2 () =
    let badges = Seq.map checkGroup (createGroups input)
    Seq.fold (fun state item -> state + (charToPriority item)) 0 badges
