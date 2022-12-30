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

let rec findDistanceRec (target:Valve) (destination:Valve) (visitedValves: Valve array) (calculatedDistances: Dictionary<(string * string), int>) = 
    
    let mutable index = 0
    let mutable foundPath = false
    let mutable keepLooping = true
    let mutable shortestDistance = None
    while (keepLooping) do 

        let connection = target.Connections[index]
        if target.Id = connection.Id then () // don't want to go back from where we came from
        else if contains connection visitedValves then () // don't want to go through the same valve twice
        else if connection.Id = destination.Id then 
            shortestDistance <- 1 |> Some
            foundPath <- true
        else 
            let key1 = (connection.Id, destination.Id)
            let key2 = (destination.Id, connection.Id)

            let found1, value1 = calculatedDistances.TryGetValue key1
            let found2, value2 = calculatedDistances.TryGetValue key2

            if found1
            then 
                if isShorter value1 shortestDistance 
                then shortestDistance <- Some (value1 + 1)
                else ()
            else if found2
            then 
                if isShorter value2 shortestDistance 
                then shortestDistance <- Some (value2 + 1) 
                else ()
            else 
                let distanceFromConnectionToDestination = findDistanceRec connection destination (addToArray visitedValves target) calculatedDistances
                match distanceFromConnectionToDestination with 
                    | None -> ()
                    | Some distance -> 
                        let newDistance = distance + 1
                        if isShorter newDistance shortestDistance 
                        then shortestDistance <- Some newDistance 
                        else ()
            ()

        index <- index + 1
        if index = target.Connections.Length then keepLooping <- false
        else if foundPath then keepLooping <- false 
        else ()

    if shortestDistance.IsSome then 
        //printfn $"{target.Id} -> {destination.Id} = {shortestDistance.Value}"
        calculatedDistances[(target.Id, destination.Id)] <- shortestDistance.Value
        ()
    else
        //printfn $"{target.Id} -> {destination.Id} = No route"
        ()
    
    shortestDistance

let findDistance target destination calculatedDistances = 
    let distance = findDistanceRec target destination Array.empty calculatedDistances
    distance

type Result = {
    mutable Pressure: int
}

let calculatePressure valves calculatedDistances = 
    let mutable timeLeft = 30
    let mutable pressure = 0
    for index in 0 .. 1 .. Array.length valves - 2 do 
        let currentValve = valves[index]
        let nextValve = valves[index + 1]
        let Somedistance = findDistance currentValve nextValve calculatedDistances
        let distance = Somedistance.Value
        timeLeft <- timeLeft - distance - 1 
        pressure <- pressure + nextValve.Rate * timeLeft
    pressure

let rec calculateReleasedPressure startValve nonZeroValves calculatedDistances timeLeft pressure maxPressure = 
    let mutable a = maxPressure

    for nextValve in nonZeroValves do 
        let distance = findDistance startValve nextValve calculatedDistances
        let canWeMakeIt = timeLeft - distance.Value - 1 >= 0
        if canWeMakeIt
        then 
            let newTimeLeft = timeLeft - distance.Value - 1
            let newPressure = pressure + nextValve.Rate * newTimeLeft
            let nonZeroValvesWithoutNextValve = nonZeroValves |> Array.filter (fun v -> v.Id <> nextValve.Id)
            let maxPressure = if newPressure > maxPressure then newPressure else maxPressure
            let newMaxPressure = calculateReleasedPressure nextValve nonZeroValvesWithoutNextValve calculatedDistances newTimeLeft newPressure maxPressure
            if newMaxPressure > a then a <- newMaxPressure else ()
        else
            ()
    a

let part1 (input: string[]) = 
    let valves = Day16.parseValves input
    let nonZeroValves = valves |> Seq.filter (fun valve -> valve.Rate <> 0) |> Array.ofSeq
    let startValve = getStartValve valves
    let calculatedDistances = new Dictionary<(string * string), int>()
    let timeLeft = 30
    let maxPressure = calculateReleasedPressure startValve nonZeroValves calculatedDistances timeLeft 0 0
    printfn "%d" maxPressure
    0

let part1TestInput () =
    part1 (inputReader.readLines "Day16/testInput.txt" |> Array.ofSeq)


let part1RealInput () =
    part1 (inputReader.readLines "Day16/input.txt" |> Array.ofSeq)