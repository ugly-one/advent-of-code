#load "../general.fsx"

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let lines = readLines "testInput.txt"

General.printSeq lines

type ParsingState =
    | Seeds
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

let parseLine (state: ParsingState, parsedInput: ParsedInput) (line: string) =
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
        | Seeds -> failwith "bla"
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
    let (_, parsedInput) = input |> (Array.fold parseLine (Seeds, initialParsedInput))
    parsedInput
    
let parsedInput = parseInput lines

parsedInput.LightToTemperature |> General.printSeq