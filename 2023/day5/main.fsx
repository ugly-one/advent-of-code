#load "../general.fsx"

open System.Collections.Generic
open Microsoft.FSharp.Core

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let lines = readLines "input.txt"

type MapType =
    | SeedToSoil
    | SoilToFertilizer
    | FertilizerToWater
    | WaterToLight
    | LightToTemperature
    | TemperatureToHumidity
    | HumidityToLocation

type Map_ = { DestinationStart: int64 ; SourceStart: int64; Range: int64 }

type ParsedInput = {
    Seeds: int64[]
    SeedToSoil: Map_[]
    SoilToFertilizer: Map_[]
    FertilizerToWater: Map_[]
    WaterToLight: Map_[]
    LightToTemperature: Map_[]
    TemperatureToHumidity: Map_[]
    HumidityToLocation: Map_[]
}

let toArray (line: string) =
    let numbersString = line.Split(" ")
    numbersString |> Array.map int64

let arrayToMap (numbers: int64 array) =
    { DestinationStart = numbers[0]; SourceStart = numbers[1]; Range = numbers[2] }

let toMap (line: string) =
    line |> toArray |> arrayToMap

let parseSeeds_part1 (line: string) =
    let numbersString = line.Split(" ")[1..]
    let numbers = numbersString |> Array.map int64
    numbers
    
let parseSeeds_part2 (line: string) =
    let numbers = parseSeeds_part1 line
    let result = new List<int64>()
    for i in 0..2..(numbers.Length - 2) do
        let start = numbers[i]
        let range = numbers[i+1]
        for j in start..(start+range - 1L) do
            result.Add(j)
    result |> Array.ofSeq
    
let parseLine (state: MapType, parsedInput: ParsedInput)  (line: string) =
    match line with
    | line when line.StartsWith("seed-to-soil") ->
        (SeedToSoil, parsedInput)
    | line when line.StartsWith("soil-to-fertilizer") ->
        (SoilToFertilizer, parsedInput)
    | line when line.StartsWith("fertilizer-to-water") ->
        (FertilizerToWater, parsedInput)
    | line when line.StartsWith("water-to-light") ->
        (WaterToLight, parsedInput)
    | line when line.StartsWith("light-to-temperature") ->
        (LightToTemperature, parsedInput)
    | line when line.StartsWith("temperature-to-humidity") ->
        (TemperatureToHumidity, parsedInput)
    | line when line.StartsWith("humidity-to-location") ->
        (HumidityToLocation, parsedInput)
    | "" -> (state, parsedInput)
    | _ ->
        match state with
        | SeedToSoil ->
            (state, {parsedInput with SeedToSoil = Array.append parsedInput.SeedToSoil [| line |> toMap |] })
        | SoilToFertilizer ->
            (state, {parsedInput with SoilToFertilizer = Array.append parsedInput.SoilToFertilizer [| line |> toMap |] })
        | FertilizerToWater ->
            (state, {parsedInput with FertilizerToWater = Array.append parsedInput.FertilizerToWater [| line |> toMap |] })
        | WaterToLight ->
            (state, {parsedInput with WaterToLight = Array.append parsedInput.WaterToLight [| line |> toMap |] })
        | LightToTemperature ->
            (state, {parsedInput with LightToTemperature = Array.append parsedInput.LightToTemperature [| line |> toMap |] })
        | TemperatureToHumidity ->
            (state, {parsedInput with TemperatureToHumidity = Array.append parsedInput.TemperatureToHumidity [| line |> toMap |] })
        | HumidityToLocation ->
            (state, {parsedInput with HumidityToLocation = Array.append parsedInput.HumidityToLocation [| line |> toMap |] })

let parseInput (input: string array) parseSeeds =
    let seeds = parseSeeds input[0]
    printfn "test3"
    let initialParsedInput = {
        Seeds = seeds
        SeedToSoil = Array.empty 
        SoilToFertilizer = Array.empty
        FertilizerToWater = Array.empty
        WaterToLight = Array.empty
        LightToTemperature = Array.empty
        TemperatureToHumidity = Array.empty
        HumidityToLocation = Array.empty 
    }
    let (_, parsedInput) = input[1..] |> (Array.fold parseLine (SeedToSoil, initialParsedInput))
    parsedInput
    
let isWithinRange (map: Map_) (number: int64) =
    let min = map.SourceStart
    let max = map.SourceStart + map.Range
    if number < min || number > max then false
    else true
    
let getNumber (maps: Map_ array) (currentNumber: int64) =
    let mutable found = false
    let mutable newNumber = currentNumber
    for map in maps do
        if (found) then ()
        else
            let isWithinRange = isWithinRange map currentNumber
            if isWithinRange then
                let diff = map.DestinationStart - map.SourceStart
                newNumber <- currentNumber + diff
                found <- true
            else
                ()
    newNumber

let getLocation (parsedInput: ParsedInput) (seed: int64) =
    let soil = getNumber parsedInput.SeedToSoil seed
    let fertilizer = getNumber parsedInput.SoilToFertilizer soil
    let water = getNumber parsedInput.FertilizerToWater fertilizer
    let light = getNumber parsedInput.WaterToLight water
    let temperature = getNumber parsedInput.LightToTemperature light
    let humidity = getNumber parsedInput.TemperatureToHumidity temperature
    let location = getNumber parsedInput.HumidityToLocation humidity
    location

   
let getLocationWithCache (input: ParsedInput)  (cache: Dictionary<int64, int64>) (seed: int64) =
    printfn "calculating seed %A" <| seed
    let mutable value = 0L
    let isInCache = cache.TryGetValue(seed, &value)
    if (isInCache) then
        printfn "cache hit"
        value
    else
        let location = getLocation input seed
        printfn "adding to cache %A" <| location
        cache.Add(seed, location)
        location
    
// isWithinRange {DestinationStart =  50; SourceStart = 98; Range = 2 } 79 |> printfn "%A"
// isWithinRange {DestinationStart =  52; SourceStart = 50; Range = 48 } 79 |> printfn "%A"
//
// getNumber parsedInput.SeedToSoil 79 |> printfn "%A"
// getNumber parsedInput.SeedToSoil 14 |> printfn "%A"
// getNumber parsedInput.SeedToSoil 55 |> printfn "%A"
// getNumber parsedInput.SeedToSoil 13 |> printfn "%A"

// let input_part1 = parseInput lines parseSeeds_part1
// let allLocations = input_part1.Seeds |> Array.map (getLocation input_part1)
// allLocations |> Array.min |> printfn "%A"
//
// getLocation parsedInput 79 |> printfn "%A"
// getLocation parsedInput 14 |> printfn "%A"
// getLocation parsedInput 55 |> printfn "%A"
// getLocation parsedInput 13 |> printfn "%A"
//
// getNumber parsedInput.FertilizerToWater 53 |> printfn "%A"

// parseSeeds_part2 "seeds: 79 14 55 13" |> Seq.length |> printfn "%A"

printfn "test2"
let input = parseInput lines parseSeeds_part2
printfn "test"
let cache = new Dictionary<int64, int64>()
input.Seeds
    |> Array.map (getLocationWithCache input cache)
    |> Array.min
    |> printfn "%A"