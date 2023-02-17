module Day3

let input = System.IO.File.ReadAllLines "Day3/input.txt"

let move (x, y) char = 
    match char with 
    | '^' -> (x, y-1)
    | '<' -> (x-1, y)
    | '>' -> (x+1, y)
    | 'v' -> (x, y+1)

let fold (currentPos, visitedHouses) char = 
    let newPos = move currentPos char
    (newPos, visitedHouses |> Set.add newPos)

let visitedHouses = Set.empty |> Set.add (0,0)

let run () = 
    //part1
    //input[0] |> Seq.fold fold ((0,0), visitedHouses) |> snd |> Set.count |> printfn "%d"
    //part2
    let inputMarkedWithOwner = input[0] |> Seq.mapi (fun index item -> if index % 2 = 0 then (item, 0) else (item, 1))
    let santaInput = inputMarkedWithOwner |> Seq.filter (fun (item, ownerId) -> ownerId = 0) |> Seq.map (fun (item, _) -> item)
    let robotInput = inputMarkedWithOwner |> Seq.filter (fun (item, ownerId) -> ownerId = 1) |> Seq.map (fun (item, _) -> item)

    let santasHouses = santaInput |> Seq.fold fold ((0,0), visitedHouses) |> snd
    let robotsHouses = robotInput |> Seq.fold fold ((0,0), visitedHouses) |> snd

    let allHouses = santasHouses |> Seq.fold (fun houses santaHouse -> houses |> Set.add santaHouse) robotsHouses
    allHouses |> Set.count |> printfn "%d"
        
