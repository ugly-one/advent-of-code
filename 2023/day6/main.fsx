

type Race = {
    Time: int
    Distance: int
}


// Time:      7  15   30
// Distance:  9  40  200
let testRace1 = {Time = 7; Distance = 9 }
let testRace2 = {Time = 15; Distance = 40 }
let testRace3 = {Time = 30; Distance = 200 }

// Time:        59     79     65     75
// Distance:   597   1234   1032   1328
let race1 = {Time = 59; Distance = 597 }
let race2 = {Time = 79; Distance = 1234 }
let race3 = {Time = 65; Distance = 1032 }
let race4 = {Time = 75; Distance = 1328 }

let races = [|
    race1; race2; race3; race4
|]

let testRaces = [|
    testRace1; testRace2; testRace3
|]

let calculateDistance race holdingButtonTime = 
    let speed = holdingButtonTime
    let sailingTime = race.Time - holdingButtonTime
    let distance = speed * sailingTime
    distance

let calculateOptionsWIthLongerDistance race = 
    let mutable optionsWithLongerDistance = 0
    for holdingButtonTime in 0..race.Time do
        let distance = calculateDistance race holdingButtonTime
        printfn "holding button: %A - distance %A" holdingButtonTime distance
        if distance > race.Distance then
            optionsWithLongerDistance <- optionsWithLongerDistance + 1
            printfn "won!"
        else
            printfn "not good enough"
            ()
    printfn "%A" optionsWithLongerDistance
    optionsWithLongerDistance
    
    
testRaces
|> Seq.fold (fun s race -> s * (calculateOptionsWIthLongerDistance race)) 1
|> printfn "%A"

races
|> Seq.fold (fun s race -> s * (calculateOptionsWIthLongerDistance race)) 1
|> printfn "%A"

// calculateOptionsWIthLongerDistance race2 |> printfn "%A"