module Day15

open System.Text.RegularExpressions
open System.Collections.Generic
type Position = {
    x : int
    y : int
}

type Sensor = {
    Position: Position
    Range: int
}

let parse line = 
    let _match = Regex.Match(line, "Sensor at x\=([-0-9]+), y\=([-0-9]+): closest beacon is at x\=([-0-9]+), y\=([-0-9]+)")
    {x = _match.Groups[1].Value |> int; y = _match.Groups[2].Value |> int}, {x = _match.Groups[3].Value |> int; y = _match.Groups[4].Value |> int}

let calculateDistance pos1 pos2 = 
    ((pos2.x - pos1.x) |> abs) + ((pos2.y - pos1.y) |> abs)

let crossWithRow sensor distance row = 
    if (sensor.y = row) 
        then 
            seq {
                    for x in sensor.x - distance .. sensor.x + distance do
                        if x = sensor.x then () else 
                            yield {x = x; y = row}
                }
        else if (sensor.y + distance < row) || (sensor.y - distance > row) then Seq.empty
        else if (sensor.y + distance > row) then 
            let distanceUntilRow = sensor.y - row |> abs
            let remainingDistance = distance - distanceUntilRow 
            seq {
                    for x in sensor.x - remainingDistance .. sensor.x + remainingDistance do
                        yield {x = x; y = row}
                }
        else if (sensor.y - distance < row) then
            let distanceUntilRow = sensor.y - row |> abs
            let remainingDistance = distance - distanceUntilRow
            seq {
                    for x in sensor.x - remainingDistance .. sensor.x + remainingDistance do
                        yield {x = x; y = row}
                }
        else failwith "UPS"


let printPositions positions = 
    for pos in positions do 
         printfn "%d, %d" pos.x pos.y


let getSensorsBeaconDistances (input: string array) = 
    seq {
        for line in input do 
        let sensor, beacon = parse line
        let distance = calculateDistance sensor beacon
        yield (sensor, beacon, distance)
    } |> Array.ofSeq


let getAllPositionsOnRow row sensorsBeaconAndDistances = 
    let sensorsOnRow = new HashSet<Position>()
    let beaconsOnRow = new HashSet<Position>()
    let allPositionsOnRow = new HashSet<Position>()
    for (sensor, beacon, distance) in sensorsBeaconAndDistances do 
        if (sensor.y = row) then 
            sensorsOnRow.Add(sensor) |> ignore
        else ()

        if (beacon.y = row) then 
            beaconsOnRow.Add(beacon) |> ignore
        else ()

        let positionsOnRow = crossWithRow sensor distance row

        for pos in positionsOnRow do 
            allPositionsOnRow.Add(pos) |> ignore
    (allPositionsOnRow, sensorsOnRow, beaconsOnRow)

let part1 row (input: string array)  = 
    let sensorsAndBeacons = getSensorsBeaconDistances input
    let (allPositionsOnRow, sensorsOnRow, beaconsOnRow) = getAllPositionsOnRow row sensorsAndBeacons
    for beaconOnRow in beaconsOnRow do 
        allPositionsOnRow.Remove(beaconOnRow) |> ignore
    for sensorOnRow in sensorsOnRow do 
        allPositionsOnRow.Remove(sensorOnRow) |> ignore
    allPositionsOnRow.Count

let getCircumferencePoints sensor radius boundary =
    let points = new HashSet<Position>()
    for xDistance in radius .. -1 .. 0 do
        let yDistance = radius - xDistance
        if sensor.x - xDistance < 0 || sensor.x + xDistance > boundary || sensor.y - yDistance < 0 || sensor.y + yDistance > boundary
        then ()
        else
            points.Add({x = sensor.x - xDistance; y = sensor.y - yDistance}) |> ignore
            points.Add({x = sensor.x + xDistance; y = sensor.y - yDistance}) |> ignore
            points.Add({x = sensor.x + xDistance; y = sensor.y + yDistance}) |> ignore
            points.Add({x = sensor.x - xDistance; y = sensor.y + yDistance}) |> ignore
    points    

let isWithinRange point sensor = 
    let distance = calculateDistance point sensor.Position
    distance <= sensor.Range
    
let scanCircumferencePoints index sensor (sensors: Sensor[]) boundary =
    let circumferencePoints = getCircumferencePoints sensor.Position (sensor.Range + 1) boundary |> Array.ofSeq
    
    let mutable pointIndex = 0
    let mutable frequency = 0UL
    while pointIndex < circumferencePoints.Length do 
        let point = circumferencePoints[pointIndex]
        
        let mutable isWithinSensor = false
        for currentSensorIndex in 0..(sensors.Length - 1) do
            if currentSensorIndex = index then () else
                let currentSensor = sensors[currentSensorIndex]
                isWithinSensor <- isWithinSensor || (isWithinRange point currentSensor)
        if not isWithinSensor then
            frequency <- (point.x |> uint64) * 4000000UL + (point.y |> uint64)
            pointIndex <- circumferencePoints.Length
        pointIndex <- pointIndex + 1
    frequency

let part2 (size: int) (input: string array) = 
    let sensors = getSensorsBeaconDistances input |> Array.map (fun (sensor, beacon, distance) -> {Position = sensor; Range = distance})
    let mutable sensorIndex = 0
    let mutable frequency = 0UL
    while sensorIndex < sensors.Length - 1 do
        let sensor = sensors[sensorIndex]
        frequency <- scanCircumferencePoints sensorIndex sensor sensors size
        if frequency <> 0UL then sensorIndex <- sensors.Length else ()
        sensorIndex <- sensorIndex + 1
    frequency

let run () = 
    let bla = crossWithRow {x = 8; y = 7} 9 10 |> Array.ofSeq
    if bla.Length = 13 then () else failwith "bla"
    
    let bla = crossWithRow {x = 0; y = 11} 3 10 |> Array.ofSeq
    if bla.Length = 5 then () else failwith "bla"
    
    let bla = crossWithRow {x = 12; y = 14} 4 10 |> Array.ofSeq
    if bla.Length = 1 then () else failwith "bla"

    let bla = crossWithRow {x = 2; y = 0} 10 10 |> Array.ofSeq
    if bla.Length = 1 then () else failwith "bla"
    
    let circumferencePoints = getCircumferencePoints {x = 3; y = 3} 2 20
    if circumferencePoints.Count <> 8 then failwith "wrong points" else ()

    Day13.testcaseWithDescription "Day15/testInput.txt" 26 (part1 10) "part1 test"
    Day13.testcaseWithDescription "Day15/testInput.txt" 56000011UL (part2 20) "part2 test"
    
    Day13.testcaseWithDescription "Day15/input.txt" 4502208 (part1 2000000) "part1"
    Day13.testcaseWithDescription "Day15/input.txt" 13784551204480UL (part2 4000000) "part2 test"