#load "../general.fsx"

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let lines = readLines "testInput.txt"

type MapType =
    | SeedToSoil
    | SoilToFertilizer
    | FertilizerToWater
    | WaterToLight
    | LightToTemperature
    | TemperatureToHumidity
    | HumidityToLocation

type Map_ = { DestinationStart: int ; SourceStart: int; Range: int }

type ParsedInput = {
    Seeds: int[]
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
    numbersString |> Array.map int

let arrayToMap (numbers: int array) =
    { DestinationStart = numbers[0]; SourceStart = numbers[1]; Range = numbers[2] }

let toMap (line: string) =
    line |> toArray |> arrayToMap

let parseLine (state: MapType, parsedInput: ParsedInput) (line: string) =
    match line with
    | line when line.StartsWith("seeds:") ->
        let numbersString = line.Split(" ")[1..]
        let numbers = numbersString |> Array.map int
        (state, { parsedInput with Seeds = numbers })
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

let parseInput (input: string array) =
    let initialParsedInput = {
        Seeds = Array.empty
        SeedToSoil = Array.empty 
        SoilToFertilizer = Array.empty
        FertilizerToWater = Array.empty
        WaterToLight = Array.empty
        LightToTemperature = Array.empty
        TemperatureToHumidity = Array.empty
        HumidityToLocation = Array.empty 
    }
    let (_, parsedInput) = input |> (Array.fold parseLine (SeedToSoil, initialParsedInput))
    parsedInput
    
let parsedInput = parseInput lines

let isWithinRange (map: Map_) (number: int) =
    let min = map.SourceStart
    let max = map.SourceStart + map.Range
    if number < min || number > max then false
    else true
    
let getNumber (maps: Map_ array) (currentNumber: int) =
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
    

let getLocation (parsedInput: ParsedInput) (seed: int) =
    let soil = getNumber parsedInput.SeedToSoil seed
    // soil |> printfn "soil: %A"
    let fertilizer = getNumber parsedInput.SoilToFertilizer soil
    // fertilizer |> printfn "fertilizer: %A"
    let water = getNumber parsedInput.FertilizerToWater fertilizer
    // water |> printfn "water: %A"
    let light = getNumber parsedInput.WaterToLight water
    // light |> printfn "light: %A"
    let temperature = getNumber parsedInput.LightToTemperature light
    // temperature |> printfn "temperature %A"
    let humidity = getNumber parsedInput.TemperatureToHumidity temperature
    // humidity |> printfn "humidity: %A"
    let location = getNumber parsedInput.HumidityToLocation humidity
    // location |> printfn "location: %A"
    location

// isWithinRange {DestinationStart =  50; SourceStart = 98; Range = 2 } 79 |> printfn "%A"
// isWithinRange {DestinationStart =  52; SourceStart = 50; Range = 48 } 79 |> printfn "%A"
//
// getNumber parsedInput.SeedToSoil 79 |> printfn "%A"
// getNumber parsedInput.SeedToSoil 14 |> printfn "%A"
// getNumber parsedInput.SeedToSoil 55 |> printfn "%A"
// getNumber parsedInput.SeedToSoil 13 |> printfn "%A"

let allLocations = parsedInput.Seeds |> Array.map (getLocation parsedInput)

allLocations |> Array.min |> printfn "%A"
//
// getLocation parsedInput 79 |> printfn "%A"
// getLocation parsedInput 14 |> printfn "%A"
// getLocation parsedInput 55 |> printfn "%A"
// getLocation parsedInput 13 |> printfn "%A"
// //
// parsedInput.FertilizerToWater |> General.printSeq
//
// getNumber parsedInput.FertilizerToWater 53 |> printfn "%A"