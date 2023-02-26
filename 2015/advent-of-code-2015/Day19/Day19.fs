module Day19

open System.Collections.Generic
open System.Text.RegularExpressions

let parse (replacement: string) =
    //H => HO
    let split = replacement.Split '='
    let part1 = split[0].TrimEnd ()
    let part2 = split[1].TrimStart [| '>'; ' '|]
    (part1, part2)
    
let run () =
    let input = InputReader.readLines2 "19"
    let replacements = input[0.. (input.Length - 3)] |> Seq.map parse
    let molecule = input[input.Length - 1]
    // let molecule = "HOHOHO"
    // let replacements = InputReader.readTestLines2 "19" |> Seq.map parse
    //
    // let fold (map: Map<string, List<string>>) (input, output) =
    //      Map.change input (
    //          fun option ->
    //              match option with
    //              | None ->
    //                  let value = new List<string>()
    //                  value.Add output
    //                  Some value
    //              | Some value ->
    //                  value.Add output
    //                  Some value
    //              ) map
    //     
    // let replacementsMap = replacements |> Seq.fold fold Map.empty
    
    let molecule = molecule |> Array.ofSeq
    
    let modifications = seq {
        for (input, output) in replacements do
            let input = input |> Array.ofSeq
            let windowSize = input.Length
            for i in 0 .. 1 .. molecule.Length - windowSize do
                let window = molecule[i..(i+windowSize - 1)]
                if window = input then
                    yield (i, windowSize, output)
                else
                    ()
    }
    
    let mutable results = Set.empty
    for (index, inputLength, output) in modifications do
        let newMolecule = Array.zeroCreate (molecule.Length + (output.Length - inputLength))
        let mutable newMoleculeCurrentIndex = 0
        let mutable moleculeCurrentIndex = 0
        while moleculeCurrentIndex < molecule.Length do
            
            if moleculeCurrentIndex <> index then
                newMolecule[newMoleculeCurrentIndex] <- molecule[moleculeCurrentIndex]
                newMoleculeCurrentIndex <- newMoleculeCurrentIndex + 1
                moleculeCurrentIndex <- moleculeCurrentIndex + 1
            else
                for outputCharIndex in 0 .. 1 .. output.Length - 1 do
                    newMolecule[newMoleculeCurrentIndex + outputCharIndex] <- output[outputCharIndex]
                newMoleculeCurrentIndex <- newMoleculeCurrentIndex + output.Length
                moleculeCurrentIndex <- moleculeCurrentIndex + inputLength
        
        results <- results |> Set.add newMolecule
    
    results |> Set.count |> printfn "%d"
    //
    // for result in results do
    //     for char in result do
    //         printf "%c" char
    //     printfn ""
    //     
