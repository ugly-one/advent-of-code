module Day16

// 28*20 + 25*13 + 21*21 + 13*22 + 9*3 + 6*2 

// most preassure relased if each valve has access to all other valves 
// 28*22 + 26*21 + 24*20 + 22*13 + 20*3 + 18*2
open System.Text.RegularExpressions
open System.Collections.Generic

type Valve = {
    Id: string
    mutable Rate: int
    mutable Connections: Valve[]
}

let getOrCreateValve (valves: HashSet<Valve>) id = 
    let existingValve = valves |> Seq.tryFind (fun valve -> valve.Id = id)
    match existingValve with 
    | Some valve -> valve
    | None -> 
        let valve = {Id = id; Rate = 0; Connections = Array.empty}
        valves.Add(valve) |> ignore
        valve

let getStartValve valves = 
    valves |> Seq.find (fun valve -> valve.Id = "AA")

let parseValves input = 
    let valves = new HashSet<Valve>()
    for line in input do 
        let _match = Regex.Match(line, "Valve ([A-Z]+) has flow rate=([0-9]+); tunnel[s]? lead[s]? to valve[s]? (.*)")
        let valveId = _match.Groups[1].Value
        let valveRate = _match.Groups[2].Value |> int
        let connections = 
            _match.Groups[3].Value.Split(',') 
            |> Array.map (fun i -> i.Trim() |> getOrCreateValve valves)

        let valve = getOrCreateValve valves valveId
        valve.Rate <- valveRate
        valve.Connections <- connections
    valves

let contains valve valves =
    valves |> Seq.tryFind (fun v -> v.Id = valve.Id) |> Option.isSome

let rec findPath startValve targetValve (visitedValves: Valve[]) = 
    if contains targetValve startValve.Connections 
    then Array.append visitedValves [| targetValve |] 
    else
        let mutable possiblePaths = Array.empty
        for connection in startValve.Connections do 
            if contains connection visitedValves then () 
            else if startValve = connection then () 
            else 
                let path = findPath connection targetValve (Array.append visitedValves [| connection |])
                if path.Length <> 0
                then possiblePaths <- Array.append possiblePaths [| path |]  
                else () 
        
        let mutable shortestPathLength = 999
        let mutable shortestPath = Array.empty
        for path in possiblePaths do 
            if path.Length < shortestPathLength
            then 
                shortestPathLength <- path.Length
                shortestPath <- path
            else ()
        shortestPath

let rec findAllPaths valve targetValves : Valve[][] = 
    let mutable allPossibleCombinations = Array.init 0 (fun i -> Array.empty)
    for targetValve in targetValves do 
        let path = findPath valve targetValve [| |]
        // assume there is always a path
        let remainingTargetValves = Seq.filter (fun v -> v.Id <> targetValve.Id) targetValves
        let subPaths = findAllPaths targetValve remainingTargetValves
        if subPaths.Length = 0
        then 
            allPossibleCombinations <- Array.append allPossibleCombinations  [| path |]
        else 
            let fullPaths = Array.map (fun subPath -> Array.append path subPath) subPaths
            allPossibleCombinations <- Array.append allPossibleCombinations fullPaths
    allPossibleCombinations 

let part1 (input: string[]) = 
    // parse
    let valves = parseValves input 
    let startValve = valves |> getStartValve
    let nonZeroValves = valves |> Seq.filter (fun valve -> valve.Rate <> 0)
    let targetValves = nonZeroValves

    let a = findAllPaths startValve targetValves

    a.Length |> ignore
    // algoritm 
    // for each valve
    // repeat this:
    // try to get to another non zero valve by walking through 0 valves
    // record the path when 30 minutes elapses - calculate the total preasure

    0

let run () =
    let result = part1 (inputReader.readLines "Day16/testInput.txt" |> Array.ofSeq)
    if (result <> 1651) then failwithf $"part1 test input failed/ {result}" else ()