module Day15

open System.Text.RegularExpressions
open System.Collections.Generic
// Sensor at x=2, y=18: closest beacon is at x=-2, y=15

type Position = {
    x : int
    y : int
}

let parse line = 
    let _match = Regex.Match(line, "Sensor at x\=([-0-9]+), y\=([-0-9]+): closest beacon is at x\=([-0-9]+), y\=([-0-9]+)")
    {x = _match.Groups[1].Value |> int; y = _match.Groups[2].Value |> int}, {x = _match.Groups[3].Value |> int; y = _match.Groups[4].Value |> int}

let calculateDistance pos1 pos2 = 
    ((pos2.x - pos1.x) |> abs) + ((pos2.y - pos1.y) |> abs)

let crossWithRow sensor distance row = 
    let result = if (sensor.y = row) 
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
                    else Seq.empty
    result

let printPositions positions = 
    for pos in positions do 
         printfn "%d, %d" pos.x pos.y

let part1 row (input: string array)  = 

    let allPositionsOnRow = new HashSet<Position>()
    let beaconsOnRow = new HashSet<Position>()
    let sensorsOnRow = new HashSet<Position>()
    for line in input do 
        let sensor, beacon = parse line

        if (sensor.y = row) then 
            sensorsOnRow.Add(sensor) |> ignore
        else ()

        if (beacon.y = row) then 
            beaconsOnRow.Add(beacon) |> ignore
        else ()

        let distance = calculateDistance sensor beacon
        let positionsOnRow = crossWithRow sensor distance row

        for pos in positionsOnRow do 
            allPositionsOnRow.Add(pos) |> ignore

    for beaconOnRow in beaconsOnRow do 
        allPositionsOnRow.Remove(beaconOnRow) |> ignore
    
    for sensorOnRow in sensorsOnRow do 
        allPositionsOnRow.Remove(sensorOnRow) |> ignore

    allPositionsOnRow.Count

let part2 (input: string array) = 
    0


let run () = 
    let bla = crossWithRow {x = 8; y = 7} 9 10 |> Array.ofSeq
    if bla.Length = 13 then () else failwith "bla"
    
    let bla = crossWithRow {x = 0; y = 11} 3 10 |> Array.ofSeq
    if bla.Length = 5 then () else failwith "bla"
    
    let bla = crossWithRow {x = 12; y = 14} 4 10 |> Array.ofSeq
    if bla.Length = 1 then () else failwith "bla"

    let bla = crossWithRow {x = 2; y = 0} 10 10 |> Array.ofSeq
    if bla.Length = 1 then () else failwith "bla"
    Day13.testcaseWithDescription "Day15/testInput.txt" 26 (part1 10) "part1 test"
    Day13.testcaseWithDescription "Day15/input.txt" -1 (part1 2000000) "part1"

    //Day13.testcaseWithDescription "Day15/testInput.txt" -1 part2 "part2 test"
    //Day13.testcaseWithDescription "Day15/input.txt" -1 part2 "part2"