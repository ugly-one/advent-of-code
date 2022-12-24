module Day16_attempt2
open Day16
open System.Collections.Generic

let addToArray array item = 
    Array.append array [| item |]


let isShorter distance maybeDistance = 
    match maybeDistance with 
        | None -> true
        | Some distance2 -> 
            if distance < distance2 then true else false

let rec findDistanceRec (target:Valve) destination (visitedValves: Valve array) (calculatedDistances: Dictionary<(string * string), int>) = 
    
    let mutable index = 0
    let mutable foundPath = false
    let mutable keepLooping = true
    let mutable shortestDistance = None
    while (keepLooping) do 

        let connection = target.Connections[index]
        if connection.Id = destination.Id then 
            shortestDistance <- Array.length visitedValves + 1 |> Some
            foundPath <- true
        else if contains connection visitedValves then () // don't want to go through the same valve twice
        else if target.Id = connection.Id then () // don't want to go back from where we came from
        else 
            let result = 999
            if calculatedDistances.TryGetValue((connection.Id, destination.Id), ref result) 
            then if isShorter result shortestDistance then shortestDistance <- Some result else ()
            else if calculatedDistances.TryGetValue((destination.Id, connection.Id), ref result) 
            then if isShorter result shortestDistance then shortestDistance <- Some result else ()
            else 
                let distanceFromConnectionToDestination = findDistanceRec connection destination (addToArray visitedValves target) calculatedDistances
                match distanceFromConnectionToDestination with 
                    | None -> ()
                    | Some distance -> 
                        calculatedDistances.Add((connection.Id, destination.Id), distance)
                        if isShorter distance shortestDistance then shortestDistance <- Some distance else ()
            ()

        index <- index + 1
        if index = target.Connections.Length then keepLooping <- false
        else if foundPath then keepLooping <- false 
        else ()
    shortestDistance

let findDistance target destination calculatedDistances = 
    let distance = findDistanceRec target destination Array.empty calculatedDistances
    distance


let part1 (input: string[]) = 
    let valves = Day16.parseValves input
    let nonZeroValves = valves |> Seq.filter (fun valve -> valve.Rate <> 0) |> Array.ofSeq

    let calculatedDistances = new Dictionary<(string * string), int>()
    for index in 0..nonZeroValves.Length-1 do 
        let valve = nonZeroValves[index]

        for subIndex in index+1..nonZeroValves.Length-1 do
            let nextValve = nonZeroValves[subIndex]
            let distance = findDistance valve nextValve calculatedDistances
            ()
            //printfn $"Distance from {valve.Id} to {nextValve.Id} = {distance}"
        ()
    0

let part1TestInput () =
    part1 (inputReader.readLines "Day16/testInput.txt" |> Array.ofSeq)


let part1RealInput () =
    part1 (inputReader.readLines "Day16/input.txt" |> Array.ofSeq)