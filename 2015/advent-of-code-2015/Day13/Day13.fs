module Day13

open System.Collections.Generic
open System.Text.RegularExpressions

let print (people: string[]) = 
    for person in people do 
        printf $"%s{person},"
    printfn ""

let calculateHappiness (happiness: Map<(string*string),int>) (people: string[]) =
    let numberOfPeople = people.Length
    let lastIndex = numberOfPeople - 1
    let mutable totalHappiness = 0
    for index in 0 .. lastIndex do
        let currentPerson = people[index]
        let leftPerson = if index = 0 then people[lastIndex] else people[index - 1]
        let rightPerson = if index = lastIndex then people[0] else people[index + 1]

        let leftHappiness = happiness[(currentPerson, leftPerson)]
        let rightHappiness = happiness[(currentPerson, rightPerson)]
        totalHappiness <- totalHappiness + leftHappiness + rightHappiness
    totalHappiness

let generateCombinations happiness people =
    let rec generate remainingPeople (takenPeople: string[]) =
        if Array.length remainingPeople = 0 then
            calculateHappiness happiness takenPeople
            // [| takenPeople |]
        else
            let mutable maxHappiness = 0
            for index in 0 .. remainingPeople.Length - 1 do
                let person = remainingPeople[index]
                let remainingPeople = remainingPeople |> Array.filter (fun p -> p <> person)
                let takenPeople = Array.append takenPeople [| person |]
                let result = generate remainingPeople takenPeople
                if result > maxHappiness then maxHappiness <- result else ()
            maxHappiness
            
    generate people Array.empty 
    
let parse line =
    let match_ = Regex.Match(line, "([A-Za-z]+) would (gain|lose) ([0-9]+) happiness units by sitting next to ([A-Za-z]+).")
    let person1 = match_.Groups[1] |> string
    let person2 = match_.Groups[4] |> string
    let value = match_.Groups[3] |> string |> int
    let sign = match_.Groups[2] |> string
    let sign =
        match sign with
        | "gain" -> 1
        | "lose" -> -1
        | _ -> failwith "HE"
    ((person1, person2), value * sign) 
    
let parseInput (lines: string[]) =
    lines |> Seq.map parse

let runInput input =
    let happiness =
        input
        |> parseInput
        |> Map.ofSeq
        
    let people =
        happiness
        |> Map.map (fun (p1 ,p2) -> fun (v) -> p1)
        |> Map.values
        |> Set.ofSeq
        |> Array.ofSeq

        
    let happiness =
        people
        |> Seq.fold (fun state person ->
                        Map.add ("Tomek", person) 0 state
                        |> Map.add (person, "Tomek") 0) happiness
        
    let people = people |> Array.append [| "Tomek" |]        
    people
    |> generateCombinations happiness
    |> printfn "%d"
    
let run () =
    InputReader.readLines "Day13/testInput.txt" |> runInput
    InputReader.readLines "Day13/input.txt" |> runInput
