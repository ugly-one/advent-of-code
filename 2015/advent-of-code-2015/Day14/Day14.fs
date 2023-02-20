module Day14

open System.Collections.Generic
open System.Text.RegularExpressions

type Reindeer = {
    Name: string
    Speed: int
    FlyTime: int
    RestTime: int
    mutable DistanceSoFar: int
    mutable State: int // 1 flying. 0 resting
    mutable RemainingTimeForCurrentState: int
}

let parse line =
    let match_ = Regex.Match(line, "([A-Za-z]+) can fly ([0-9]+) km/s for ([0-9]+) seconds, but then must rest for ([0-9]+) seconds.")
    let name = match_.Groups[1] |> string
    let speed = match_.Groups[2] |> string |> int
    let flyTime = match_.Groups[3] |> string |> int
    let restTime = match_.Groups[4] |> string |> int
    {Name = name; Speed = speed; FlyTime = flyTime; RestTime = restTime; DistanceSoFar = 0; State = 1; RemainingTimeForCurrentState = flyTime}
    
let run () =
    let input = [|
        "Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds."
        "Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
    |]
    let reindeerCollection = input
                             |> Seq.map parse
                             |> Array.ofSeq
    for second in 1 .. 1000 do
        for reindeer in reindeerCollection do
            // TODO update reindeer 
            
    ()
