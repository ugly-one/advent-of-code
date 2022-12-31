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

type Path = {
    Valves: Valve[]
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

let rec findPath startValve targetValve (visitedValves: Valve[]) remainingTime : Path = 
    if remainingTime = 0 
    then {Valves = Array.empty} 
    else 
        if contains targetValve startValve.Connections 
        then {Valves = Array.append visitedValves [| targetValve |]} 
        else
            let mutable possiblePaths = Array.empty
            for connection in startValve.Connections do 
                if contains connection visitedValves  || startValve = connection
                then 
                    // we were already at this valve
                    () 
                else 
                    let path = findPath connection targetValve (Array.append visitedValves [| connection |]) (remainingTime - 1)
                    if path.Valves.Length <> 0
                    then possiblePaths <- Array.append possiblePaths [| path |]  
                    else () 
        
            let mutable shortestPathLength = 999
            let mutable shortestPath = {Valves = Array.empty}
            for path in possiblePaths do 
                if path.Valves.Length < shortestPathLength
                then 
                    shortestPathLength <- path.Valves.Length
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
    PathToTarget: Path
}

let appendPath path1 path2 = 
    {Valves = Array.append path1.Valves path2.Valves}

let rec findAllPaths valve targetValves remainingTime : List<(Path * PressureRelease array)> = 
    let mutable allPossibleCombinations = new List<(Path * PressureRelease array)>()

    if remainingTime = 0 
    then allPossibleCombinations 
    else 
        for targetValve in targetValves do 
            let path = findPath valve targetValve [| |] remainingTime
            if path.Valves.Length = 0 then ()
            else 
                let remainingTime = remainingTime - path.Valves.Length - 1
                if remainingTime <= 0 then 
                    // cannot make this path
                    () 
                else 
                    let remainingTargetValves = Seq.filter (fun v -> v.Id <> targetValve.Id) targetValves
                    let subPaths = findAllPaths targetValve remainingTargetValves remainingTime
                    let pressureRelease =  {RemainingTime = remainingTime; Rate = targetValve.Rate; Valve = targetValve; PathToTarget = path}
                    if subPaths.Count = 0
                    then 
                        allPossibleCombinations.Add((path, [| pressureRelease |] ))
                       // printfn $"Adding path with length {path.Valves.Length}"
                    else 
                        let fullPaths = Seq.map (fun (subPath, preassure) -> (appendPath path subPath, Array.append preassure [| pressureRelease |] )) subPaths
                        allPossibleCombinations.AddRange(fullPaths)
                        //for (fullPath, preassureRelease) in fullPaths do 
                         //   printfn $"Adding path with length {fullPath.Valves.Length}"
                          //  ()
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

let rec generateAllPossibleSubCollections (collection: Valve array) subCollectionCount = 
    if subCollectionCount = 0 
    then Array.empty
    else 
        let result = new List<Valve array>()
        let mutable index = 0
        for item in collection do 
            let collectionWithoutItem = Array.filter (fun i -> i.Id <> item.Id) collection
            let remainingSubCollection = generateAllPossibleSubCollections collectionWithoutItem (subCollectionCount - 1)

            if (remainingSubCollection.Length = 0)
            then 
                result.Add([| item |])
            else 
                let subResult = Seq.map (fun sub -> Array.append sub [| item |] ) remainingSubCollection
                result.AddRange(subResult)
            ()
        result.ToArray()

let getRemainingPart collection subCollection = 
    Array.filter (fun item -> contains item subCollection |> not) collection

let splitTargetValves valves = 
    let result = new List<(Valve array * Valve array)>()
    for count in (Seq.length valves / 2) .. (Seq.length valves / 2) do 
        printfn $"SPLITTING WITH COUNT {count}"
        let splitPart1 = generateAllPossibleSubCollections valves count
        let splitPart2 = Array.map (fun split -> (split, getRemainingPart valves split)) splitPart1
        result.AddRange(splitPart2)
    result

let splitTargetValves_faster valves = 
    let partSize = (valves |> Array.length ) / 2
    let result = new List<(Valve array * Valve array)>()
    let indexes = seq { 0 .. partSize - 1 } |> Array.ofSeq
    let part = valves[.. partSize - 1]
    let part2 = getRemainingPart valves part
    result.Add((part, part2))
    for index in 0 .. partSize - 1 do 
        let mutable newIndex = indexes[index]
        for count in 0 .. partSize - 1 do 
            newIndex <- newIndex + 1
            let temp_result = Array.copy part
            while (Array.contains newIndex indexes) do 
                newIndex <- newIndex + 1
            if newIndex < Array.length valves then 
                temp_result[index] <- valves[newIndex] 
                let part2 = getRemainingPart valves temp_result
                result.Add((temp_result, part2))
            else ()

    result


let part2 (input: string[]) = 
    // parse
    printfn "parsing..."
    let valves = parseValves input 
    let startValve = valves |> getStartValve
    let nonZeroValves = valves |> Seq.filter (fun valve -> valve.Rate <> 0)
    let targetValves = nonZeroValves |> Array.ofSeq

    let mutable ourMaxPressure = 0

    let splitTargetValves = splitTargetValves targetValves

    for (mine, elephants) in splitTargetValves do 
        printfn $"finding all possible paths... I have {Seq.length mine} valves, elephant has {Seq.length elephants}"
        let allPossibleMineCombinations = findAllPaths startValve mine 26
        let allPossibleElephantsCombinations = findAllPaths startValve elephants 26

        //let debugCombination = Array.filter (fun ((path : Valve[]), _) -> if path[0].Id = "DD" && path[1].Id = "CC" && path[2].Id = "BB"  && path[3].Id = "AA" && path[path.Length-1].Id = "CC" && path[path.Length-2].Id = "DD" && path[path.Length-3].Id = "EE"  && path[path.Length-4].Id = "FF" then true else false) allPossibleCombinations
        printfn "Finding max release...."
        let mineMaxPressure = Seq.map (fun (path, preasure) -> sumTotalRelease preasure) allPossibleMineCombinations |> Seq.max
        let elephantMaxPressure = Seq.map (fun (path, preasure) -> sumTotalRelease preasure) allPossibleElephantsCombinations |> Seq.max
        let ourPressure = mineMaxPressure + elephantMaxPressure
        if ourPressure > ourMaxPressure then ourMaxPressure <- ourPressure else ()
    ourMaxPressure


let run () =
    //let result = part1 (inputReader.readLines "Day16/testInput.txt" |> Array.ofSeq)
    //if (result <> 1651) then failwithf $"part1 test input failed/ {result}" else printfn "OK!"

    //let result = part1 (inputReader.readLines "Day16/input.txt" |> Array.ofSeq)
    //if (result <> 1986) then failwithf $"part1 normal input failed/ {result}" else printfn "OK!"

    //let result = part2 (inputReader.readLines "Day16/testInput.txt" |> Array.ofSeq)
    //if (result <> 1707) then failwithf $"part1 test input failed/ {result}" else printfn "OK!"

    let result = part2 (inputReader.readLines "Day16/input.txt" |> Array.ofSeq)
    if (result <> 0) then failwithf $"part1 normal input failed/ {result}" else printfn "OK!"