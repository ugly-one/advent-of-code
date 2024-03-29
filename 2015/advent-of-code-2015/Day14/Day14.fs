module Day14

open System.Collections.Generic
open System.Text.RegularExpressions

type Reindeer = {
    Name: string
    Speed: int
    FlyTime: int
    RestTime: int
    mutable DistanceSoFar: int
    mutable State: bool // 1 flying. 0 resting
    mutable RemainingTimeForCurrentState: int
    mutable Score: int
}

let parse line =
    let match_ = Regex.Match(line, "([A-Za-z]+) can fly ([0-9]+) km/s for ([0-9]+) seconds, but then must rest for ([0-9]+) seconds.")
    let name = match_.Groups[1] |> string
    let speed = match_.Groups[2] |> string |> int
    let flyTime = match_.Groups[3] |> string |> int
    let restTime = match_.Groups[4] |> string |> int
    {Name = name; Speed = speed; FlyTime = flyTime; RestTime = restTime; DistanceSoFar = 0; State = true; RemainingTimeForCurrentState = flyTime; Score = 0}
    
let run () =
    let input = [|
        "Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds."
        "Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
    |]
    let input = InputReader.readLines "Day14/input.txt"
    
    let reindeerCollection = input
                             |> Seq.map parse
                             |> Array.ofSeq
    let findWinnersByDistance reindeerCollection =
        let maxDistanceReindeer = reindeerCollection |> Seq.maxBy (fun r -> r.DistanceSoFar)
        reindeerCollection |> Seq.filter (fun r -> r.DistanceSoFar = maxDistanceReindeer.DistanceSoFar)
    
    let findWinnerByScore reindeerCollection =
        reindeerCollection |> Seq.maxBy (fun r -> r.Score)
        
    for second in 1 .. 2503 do
        for reindeer in reindeerCollection do
            if reindeer.State then reindeer.DistanceSoFar <- reindeer.DistanceSoFar + reindeer.Speed else () 
            reindeer.RemainingTimeForCurrentState <- reindeer.RemainingTimeForCurrentState - 1
            if reindeer.RemainingTimeForCurrentState = 0 then
                reindeer.State <- not reindeer.State
                reindeer.RemainingTimeForCurrentState <- if reindeer.State then reindeer.FlyTime else reindeer.RestTime
            else ()                    
        let winners = findWinnersByDistance reindeerCollection
        for winner in winners do
            winner.Score <- winner.Score + 1
    
    reindeerCollection |> findWinnerByScore |> (fun r -> printfn $"%s{r.Name} %d{r.Score}")             
