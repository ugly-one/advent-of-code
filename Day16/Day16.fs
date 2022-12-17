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

let rec findPath startValve targetValve (visitedValves: Valve[]) remainingTime = 
    if remainingTime = 0 
    then Array.empty 
    else 
        if contains targetValve startValve.Connections 
        then Array.append visitedValves [| targetValve |] 
        else
            let mutable possiblePaths = Array.empty
            for connection in startValve.Connections do 
                if contains connection visitedValves  || startValve = connection
                then 
                    // we were already at this valve
                    () 
                else 
                    let path = findPath connection targetValve (Array.append visitedValves [| connection |]) (remainingTime - 1)
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

let printPath valves = 
    for valve in valves do 
        printf $"{valve.Id} -> "

    printfn ""

type PressureRelease = {
    RemainingTime: int
    Rate: int
    Valve: Valve
    PathToTarget: Valve array
}

let rec findAllPaths valve targetValves remainingTime : (Valve array * PressureRelease array) array = 
    let mutable allPossibleCombinations = Array.init 0 (fun i -> (Array.empty, Array.empty))

    if remainingTime = 0 
    then allPossibleCombinations 
    else 
        for targetValve in targetValves do 
            let path = findPath valve targetValve [| |] remainingTime
            let remainingTime = remainingTime - path.Length - 1
            if remainingTime < 0 then 
                // cannot make this path
                () 
            else 
                let remainingTargetValves = Seq.filter (fun v -> v.Id <> targetValve.Id) targetValves
                let subPaths = findAllPaths targetValve remainingTargetValves remainingTime
                let bla =  {RemainingTime = remainingTime; Rate = targetValve.Rate; Valve = targetValve; PathToTarget = path}
                if subPaths.Length = 0
                then 
                    allPossibleCombinations <- Array.append allPossibleCombinations  [| (path, [| bla |] ) |]
                else 
                    let fullPaths = Array.map (fun (subPath, preassure) -> ((Array.append path subPath), Array.append preassure [| bla |] )) subPaths
                    allPossibleCombinations <- Array.append allPossibleCombinations fullPaths
        allPossibleCombinations 

let sumTotalRelease releases = 
    Array.fold (fun total release -> total + release.RemainingTime * release.Rate) 0 releases

let part1 (input: string[]) = 
    // parse
    printfn "parsing..."
    let valves = parseValves input 
    let startValve = valves |> getStartValve
    let nonZeroValves = valves |> Seq.filter (fun valve -> valve.Rate <> 0)
    let targetValves = nonZeroValves

    printfn "finding all possible paths..."
    let allPossibleCombinations = findAllPaths startValve targetValves 30
    //let debugCombination = Array.filter (fun ((path : Valve[]), _) -> if path[0].Id = "DD" && path[1].Id = "CC" && path[2].Id = "BB"  && path[3].Id = "AA" && path[path.Length-1].Id = "CC" && path[path.Length-2].Id = "DD" && path[path.Length-3].Id = "EE"  && path[path.Length-4].Id = "FF" then true else false) allPossibleCombinations

    printfn "Finding max release...."

    let c = Seq.map (fun (path, preasure) -> sumTotalRelease preasure) allPossibleCombinations |> Seq.max
    c

let run () =
    let result = part1 (inputReader.readLines "Day16/testInput.txt" |> Array.ofSeq)
    if (result <> 1651) then failwithf $"part1 test input failed/ {result}" else printfn "OK!"

    let result = part1 (inputReader.readLines "Day16/input.txt" |> Array.ofSeq)
    if (result <> 1651) then failwithf $"part1 normal input failed/ {result}" else printfn "OK!"