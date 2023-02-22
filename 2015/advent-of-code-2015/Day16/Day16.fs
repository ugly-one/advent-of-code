module Day16

open System.Collections.Generic
open System.Text.RegularExpressions

let run () =
    let auntSue =
        Map.empty
        |> Map.add "children" 3
        |> Map.add "cats" 7
        |> Map.add "samoyeds" 2
        |> Map.add "pomeranians" 3
        |> Map.add "akitas" 0
        |> Map.add "vizslas" 0
        |> Map.add "goldfish" 5
        |> Map.add "trees" 3
        |> Map.add "cars" 2
        |> Map.add "perfumes" 1
        
    let aunt1Input = "Sue 1: goldfish: 9, cars: 0, samoyeds: 9"
    let parseAunt input =
        let mutable aunt = Map.empty
        let _matches = Regex.Matches(input, "(goldfish|cars|samoyeds|pomeranians|akitas|vizslas|trees|children|cats|perfumes): ([0-9]+)")
        for _match in _matches do
            let property = _match.Groups[1] |> string
            let count = _match.Groups[2] |> string |> int
            aunt <- aunt |> Map.add property count
        aunt
        
    let aunts = InputReader.readLines "Day16/input.txt" |> Array.map (parseAunt)
    
    let isAuntSue (aunt: Map<string,int>) =
        let mutable result = true
        for a in aunt do
            let property = a.Key
            let count = a.Value
            match property with
            | "cats" | "trees" ->
                if count > auntSue[property] then () else result <- false
            | "pomeranians" | "goldfish" ->
                if count < auntSue[property] then () else result <- false
            | _ -> 
                if auntSue[property] = count then () else result <- false
        result
            
    aunts |> Array.findIndex isAuntSue |> printfn "%d"
