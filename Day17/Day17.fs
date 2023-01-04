﻿module Day17

open System.Collections.Generic
open System.Diagnostics;

type Position = {
    X: int
    Y: int64
}

let isWithinChamber leftPosition rightPosition = 
    if leftPosition.X < 0 then false 
    else if rightPosition.X > 6 then false
    else true

let isRock0WithinChamber (rock: Position array) = 
    isWithinChamber rock[0] rock[3]

let rock0 bottomLeftPos = 
    let rock = 
        [|
            bottomLeftPos;
            {X = bottomLeftPos.X + 1; Y = bottomLeftPos.Y};
            {X = bottomLeftPos.X + 2; Y = bottomLeftPos.Y}; 
            {X = bottomLeftPos.X + 3; Y = bottomLeftPos.Y}; 
        |]

    (rock, isRock0WithinChamber)

let isRock1WithinChamber (rock: Position array) = 
    isWithinChamber rock[1] rock[3]

let rock1 bottomLeftPos = 
    let rock = 
        [|
            {X = bottomLeftPos.X + 1; Y = bottomLeftPos.Y};
            {X = bottomLeftPos.X;     Y = bottomLeftPos.Y + 1L};
            {X = bottomLeftPos.X + 1; Y = bottomLeftPos.Y + 1L};
            {X = bottomLeftPos.X + 2; Y = bottomLeftPos.Y + 1L};
            {X = bottomLeftPos.X + 1; Y = bottomLeftPos.Y + 2L};
        |]
    (rock, isRock1WithinChamber)
    
let isRock2WithinChamber (rock: Position array) = 
    isWithinChamber rock[0] rock[3]

let rock2 bottomLeftPos = 
    let rock = 
        [|
            bottomLeftPos
            {X = bottomLeftPos.X + 1; Y = bottomLeftPos.Y};
            {X = bottomLeftPos.X + 2; Y = bottomLeftPos.Y};
            {X = bottomLeftPos.X + 2; Y = bottomLeftPos.Y + 1L};
            {X = bottomLeftPos.X + 2; Y = bottomLeftPos.Y + 2L};
        |]
    (rock, isRock2WithinChamber)


let isRock3WithinChamber (rock: Position array) = 
    isWithinChamber rock[0] rock[0]

let rock3 bottomLeftPos = 
    let rock = 
        [|
            bottomLeftPos
            {X = bottomLeftPos.X; Y = bottomLeftPos.Y + 1L};
            {X = bottomLeftPos.X; Y = bottomLeftPos.Y + 2L};
            {X = bottomLeftPos.X; Y = bottomLeftPos.Y + 3L};
        |]
    (rock, isRock3WithinChamber)


let isRock4WithinChamber (rock: Position array) = 
    isWithinChamber rock[0] rock[1]

let rock4 bottomLeftPos = 
    let rock = 
        [|
            bottomLeftPos
            {X = bottomLeftPos.X + 1; Y = bottomLeftPos.Y};
            {X = bottomLeftPos.X + 1; Y = bottomLeftPos.Y + 1L};
            {X = bottomLeftPos.X;     Y = bottomLeftPos.Y + 1L};
        |]
    (rock, isRock4WithinChamber)

let getHighestY positions = 
    Seq.fold (fun highest position -> if position.Y > highest then position.Y else highest) 0L positions

//let getLowestPositions rock = 
//    rock |> Array.fold (fun (result: Option<Position> array) (position: Position) -> 
//        match result[position.X] with 
//        | None -> 
//            result[position.X] <- Some position
//            result
//        | Some resPos -> 
//            if position.Y < resPos.Y 
//            then 
//                result[position.X] <- Some position 
//                result 
//            else result) (Array.create 7 None)

//let getHighestPositions rock = 
//    rock |> Array.fold (fun (result: Option<Position> array) (position: Position) -> 
//        match result[position.X] with 
//        | None -> 
//            result[position.X] <- Some position
//            result
//        | Some resPos -> 
//            if position.Y > resPos.Y 
//            then 
//                result[position.X] <- Some position 
//                result 
//            else result) (Array.create 7 None)

let checkCollision rock (ground: Dictionary<int64,HashSet<Position>>) = 
    let mutable collide = false
    for position in rock do 
        if ground.ContainsKey(position.Y) then 
            let groundPart = ground[position.Y]
            if groundPart |> Seq.contains position then collide <- true else ()
        else ()
    collide

let addToGround (ground : Dictionary<int64,HashSet<Position>>) rock highestY = 
    let mutable newHighestY = highestY
    for position in rock do 
        if position.Y > newHighestY then newHighestY <- position.Y else ()
        if ground.ContainsKey(position.Y) then 
            ground[position.Y].Add(position) |> ignore
        else 
            let hashSet = new HashSet<Position>()
            hashSet.Add(position) |> ignore
            ground[position.Y] <- hashSet
    newHighestY

let getRock rockIndex position = 
    match rockIndex with 
    | 0 -> rock0 position
    | 1 -> rock1 position
    | 2 -> rock2 position
    | 3 -> rock3 position
    | 4 -> rock4 position
    | _ -> rock0 position

let withinChamber rock = 
    rock |> Array.fold (fun withinChamber position -> if position.X >= 0 && position.X <= 6 then withinChamber else false) true

let moveRockByWind rock ground windDirection isWithinChamber = 
    let rockAfterMove = 
        match windDirection with 
        | '>' -> rock |> Array.map (fun position -> {position with X = position.X + 1})
        | '<' -> rock |> Array.map (fun position -> {position with X = position.X - 1})
        | _ -> failwithf "unknown direction"

    if isWithinChamber rockAfterMove then
        if checkCollision rockAfterMove ground |> not then rockAfterMove else rock
    else rock

let moveDown rock ground = 
    let rockAfterMove = 
        Array.map (fun position -> {position with Y = position.Y - 1L}) rock

    let collide = checkCollision rockAfterMove ground
    if collide then (rock, false) else (rockAfterMove, true)

let print (ground: Position array) (rock: Position array) = 
    printfn "------------------------------"
    let highestY = getHighestY rock
    for y in highestY .. -1L .. -1L do 
        for x in -1 .. 1 .. 7 do
            let pos = {X = x; Y = y} 
            if pos.X = -1 && (pos.Y = -1 || pos.Y = 7) then printf "+"
            else if pos.X = -1 || pos.X = 7 then printf "|"
            else if pos.Y = -1 then printf "-"
            else
                if Array.contains pos rock 
                then printf "@" 
                else
                    if Array.contains pos ground 
                    then printf "#" 
                    else printf "."
        printfn ""

    printfn "------------------------------"

    ()

let private run (jetPattern: string) (totalRocksCount: int64) = 
    let mutable jetIndex = 0
    let mutable rockToPick = 0;
    let rockTypesCount = 5

    let ground = new Dictionary<int64, HashSet<Position>>()
    let positions = 
                [| 
                    {X = 0; Y = 0}; 
                    {X = 1; Y = 0;}
                    {X = 2; Y = 0;}
                    {X = 3; Y = 0;}
                    {X = 4; Y = 0;}
                    {X = 5; Y = 0;}
                    {X = 6; Y = 0;}
                |]
    ground.Add(0, new HashSet<Position>(positions))

    let mutable rocksCount = 1L
    let mutable highestY = 0L
    let mutable rockPosition = {X = 2; Y = highestY + 4L}
    let (_rock, _isWithinChamber) = getRock rockToPick rockPosition
    let mutable rock = _rock
    let mutable isWithinChamber = _isWithinChamber

    //let stopWatch = new Stopwatch()
    //stopWatch.Start()
    while rocksCount <= totalRocksCount do 
        
        //print ground rock
        let wind = jetPattern[jetIndex]
        //stopWatch.Restart()
        rock <- moveRockByWind rock ground wind isWithinChamber
        //printfn "move by wind %d" stopWatch.ElapsedTicks
        //stopWatch.Restart()
        let (newRock, moved) = moveDown rock ground
        //printfn "move down %d" stopWatch.ElapsedTicks
        
        if not moved then 
            //stopWatch.Restart()
            highestY <- addToGround ground rock highestY
            
            //printfn "Adding to ground %d" stopWatch.ElapsedTicks

            // take a new rock
            //stopWatch.Restart()

            rockToPick <- (rockToPick + 1) % rockTypesCount 
            rockPosition <- {X = 2; Y = highestY + 4L}
            let (_rock, _isWithinChamber) = getRock rockToPick rockPosition
            rock <- _rock
            isWithinChamber <- _isWithinChamber
            rocksCount <- rocksCount + 1L

            //printfn "Other %d" stopWatch.ElapsedTicks

        else 
            rock <- newRock
        
            
        jetIndex <- (jetIndex + 1) % jetPattern.Length

    highestY


let run_part1_input () = 
    let jetPattern = "><<<>>>><<<>>>><>><<><<>>>><<<<>><<>><<<>>>><<<>>>><<<<>>><>>><<>>><<>>>><<>>>><>><>><<>>>><><<<<>>>><><>>><>>>><>>>><<>>>><><<<>>><<<<>><<<><<<<>>>><<>>><<<>>><><<>>><<<<>>><<<<>>><<<<>>>><<<><<<<>><>><<<>>>><><<<<><><><<>>><><>>><>>>><<<<><>>>><<>><<>>>><<>><<<>>>><<>>>><<>><<<>>>><<<<>>><<><<<<>>><<<>><><<<<>><<>><><<><<><>>>><<><<<>><<>>>><<<><<<>>><<<<>>><<<>>>><<<<><<>><<>>><>><<<>>><<<>>><<>><>><<>>>><<>>><<<<>><<<<>>><>>><>>>><<<>>><<<><>><<<>><<<>>>><<<><<<<><<>><>><<<>><>>><<>>><<<><<><><>>><<<<><<<<><<><<><<<<>><<<>>><<<>><<>><<<>><>>><<<>>><<>><<<<><<<<>><<<><>><<>><><<>><<<>><<>>><<<<><><>><<>>>><<<<><<<>><<<><<<<>>>><<<<><<<>>><<><<<<><<<<>>><<><<<>>>><>>>><<>>><><<>><<<<>><>>><<<>>>><<><<<><<<>><<>>><<<>><<<>>>><<<<>>>><>><><>>><<<>>><<<>>>><<<><<<<>>>><<><<<<>>>><<<<>>><<<>><><<><<<<><><<>>>><<<<>><>>><<<>><<<<><>>><><<>>><>>><<>>>><>><<>>><>>>><><<>>><<<>>>><<<>>><>>><<<>>>><<>>>><<<<>>><>><<<><<><>>>><<<>>><<>><<<<>>><<><<<<>>><<<>>><<<>>><>>><<><<<>><<<<>>>><>>><<<>>>><<>>><<<<><<<>><<<><<<<><<>>>><<>><>>><<>>>><<<<>>>><<<>>><<>>><<<>><<><>>>><<>>>><<>>>><>>><<>>>><<>><<><<>><<<<>><<<><<>>>><<>><<<<>>>><<<<>>>><<<>><>>><<<>>><>>>><<<<><<<>><>>><<<<><<<<><<<>><>>>><>><<<<><<>>>><<<<>>><<<<>><<<>>>><<>>>><>><<>>>><<<<>>><<>>><>>><<<<>><>>><<<<>>><<>>>><<>>><<><>>><<<<>>><<<><<<<>>>><<>>><><>>><<>>>><<>>>><<>><><>>>><<<<>>>><>><>>>><<<>>>><>>>><<>>><<<<>>><<<>><><<<><<><<<<>>><<><<>><<<<>><><><<<>>>><<<<>><><<<>>>><><<<>>>><>>>><<<<>><<<><<>>><<<<>>><<>>>><<<<>>><>><<<><<>><<>>>><>>>><<>>><<>>>><<<>><>><<<>>><<<<>>>><<>><<<<>>><<<<>><<>>><<<>>>><>>><<<<>>>><<>>>><<>>><><<<><>><<>>>><<<>>>><<<>><>><<<>><<><<<>>>><<><<>><<<<><<>><<<><><<<<>>><<>>>><<><<<><<<<>>><<<>>><<>><><<<><<>><<>><<<><>>><<<<>>>><<>>><><<<<>>><<>><<<>>>><><<<<><<<>>><>>><<<<><<<<>>><<<<>><<<<><<><<>><<<><<<>><<<><><<>>><>><<<<>>><<>><<>><<<>>>><<<<>><<<<><>>>><<><>><<<<><<<>>><<<<><<><<<>>><<<><><>>>><<>>>><>><<<<><<<>><<<><>>><<>><<<><<<<>>><>>><<<>><<<<><>><<<><<<>>><>>>><>>><<><<>>><<>><<><<<<>>>><<>><<>>>><>>>><<><<<><<<<>>>><<<>>><<<>>><<<<>><<<>>>><>>><<><<<<>>>><<<>>>><<><<<<>>><<<>><><<<<>>>><<<<>>><>>><<>>><>>>><<<<>>><<<>><<>><<<>><<<>>><>>><><<>><<<>><<><<<>><><<>>><<><<>>><<><<<<>>>><>>>><<>>>><<<<>>><<<<><>><<><<<>><>><<>><<>>><<<>><<>><>>><<<<><<<><<<<><>>>><<><<>><<>>><<<>><<<<>>><<<<><<<>>>><<><<>><<<<><<<<>><>>><<><<<>><<<>><<<>>><><<><><<<<>>>><<<>><<<><<<<>>>><<<>>>><<<<>>>><<<<>><<<>>>><>><<<><><<<><<<<><><<<<><<<<><<<<><<<<>>>><<>>><<<<>>><><<<>>><<<>><<<<>>><<<>><>>>><<<<>>>><<<>><<<>>>><<<>><>>><>>>><<<<>><<<>>><<>><<<<>>><<<<>>>><>>><<<<><>>>><<<>>>><<<>><<<>>><<><<>><<<<>>><<<<>><<<>>><<>><<>>><<<>><<>>>><<<>><<<>>><<>>><<<<>>><>>>><><<<<>>><>>>><<<>>><>><<<<>>><<<>><<<>>>><<>>>><<><<<<>>>><<<<>>>><<>>>><<<><<>>>><<><<>>>><>>><<<<><<>>>><>>>><<<<><<<>>>><<<>><>>><<<<>>><<><<<<>><>>><<<<>>>><><<<>>><<<<>><>><>><<<<>>><<<<>>>><<>>>><<<>>>><>>><<<>>><<<<>><<<><<<>><<<><>><<<>>>><<<><>>>><>><<<<>><>>><><<<<>>>><<>>><<<<>>>><<<>>><<<>><<<>>><><>><<<>><<>>><<<<><<><<<>>>><<<<><<><<<<>>><<>>>><>><<>>><<<<>>><<>>><<>>><<<<>>><>><<<>>><<>>>><<<>><<<<>><<>>>><<<><>>>><<>>><<><<<<><>>><>>><<<>>><>>>><<<<>>>><<<<>>><<<>>>><>>>><<>>>><<<>>><<<<><<>>><<>>>><<<>>>><<<<>><<<<>><<><>><<<<>><>>>><<><<<>>><>><>>><<<<>><<<><<<>>><<<<><>>><<>>>><>>><<<<>>><>><<>><>>><<>>>><<>><<<<>><<>>><<<>>>><<<><<<><<>>><<<<>>>><<>><<<><<>><<>><<<<>>><<<<>>><<<<>>><>>><><<><<<<><<<<>>><<<>>>><<<>>><>>>><<<<>><<><>>>><<<<><<<<><<<<>>><<<<>>>><<<<>>><<<<><<<<>><<<<>><>><<<<>><<<>>><<>><<<>><<<<>>><<<>>><<>>><<<>><<><<<>>><<><<<<>>>><<<<>>><<<>><>><>>>><<>>>><<<<>>><<><<<<>><<<><<<<>>>><><<<<>>>><<<<>><<>>>><>>>><<>>><<<>>>><<<<>>>><<>><<>>><<<<>>><<><><>>><>>>><<<<>>><>>>><<<<>><<<<><<<>><<<<>><<>>><><<><<>><<<<>>>><<<>><<>><<<>>>><>>><<<>>>><><>>>><<<>>>><<><<<<><<<<>>>><>>>><<<>>><>>>><<<<>><>><>><<<<><<>>><<<<>>>><<><<<>>>><<>><<>>><<<><<<><<<<><><<>>><<>><<<>>>><<<<><>>><><><><<<>>>><<<><<<<><>><><<<<><<<>>>><<><>><<>>><<<<>>>><<>><<<>><<<<>><<<<><<>>><>><<<<>>>><>>><><>><>><<>>><>><<<>>><<<<>>>><<><<<<>>>><<<<>>>><><<>>>><<>>>><>><<>><<<<>>>><>><<<<>>><<>><<<>>><<<<><<><<>>>><><>>>><<<<>>><>><<<<>><<<>><<<>>><<<<>>><<>><<<>>><<<<>>><<<<><<>>>><><<>>>><<<><<<<>>>><>><<<<>><<<<>>><<>><<<<>>>><<>>><>>><<<>>><<<<>>><<<<><<<>><>>>><><<><<><<><<>>><<<>>>><<<<>><<<<>>>><<>>>><>>><>>><>><<><<<>>>><>>><<<>><>>>><>>><>>>><<<<>><>>><<<<>>>><><<<><<<>>><<<>>>><<<<>><>><<<<>>><<<<><<>><<><<><<>>><<>>>><<<<>>>><<><<><<<>><<<>><<>>><<<><><>>>><>>><<<<>>><<>><<>>><<<<>>><<<><<>><<<>><<<<>>>><>>>><<><>>>><<<<>>>><<<<>>><<>>><<>>><><>><><<<<><<>>>><<><>><<<><<<<>><<>><<>>><><>>><<><<<<>>>><>>>><><<<><<<>><<<>>><>>>><<<<>><<>>>><>>>><>><<>>><<<><<>>>><>><>><><<<><>><<>>><<<><>>><<<>>>><<<><<>>><<<>><<<>><<>><<<<><<<<>><>><<<>>>><><<<>>>><<<<>>>><<<<>>><<<><<<<>>>><<><<<>>><<<>>><>>>><>>><<<<><<<<>>><><<>>>><<<><<>><<<<>><<>>>><<><<<<><<><<>>>><<<>>><>>><>>>><<<>>>><<<>><><<<>><<<>><<><<>>><><<<<>>>><>>><>>><<<><<>>>><<<>>>><<>><>>>><<<<>>><><><<<<><<<<>>>><<<>><>>>><>>><<<<>>><<<<>>><><<>>><<>>><<<<><<>>><>><<<>>><<<><<<<>>>><><<<><<><><<<>>><<>>><<<<><<<>>><<<><>><<><<>>><<<<>>>><>>>><<><<<>><>>>><<><<>>>><<<<><>>><>>>><<>>>><<<<>>><<<<>>><<><<><<<<>><<>>><<<<>><<<<>><<<>>>><<<<>>><<>>>><<<>>><<<><<<<>>><<<>>><>><<<>><<>>>><>>>><<<<>>><>>>><<<>>>><<<<>>>><<<>><<>><<<<><<<<><<<<><<<>>><<><<<<>>>><<><>><<<<>>><<>><><<<>>><<<><>>><<<<><<>>>><<<>>>><><><<<>><<<<>>><<>><<>>><<<>><<<>><<>>>><<<><><<><<<>><><>><<<>><<>>>><<><<<<>><<><<<<>><>>><<>>>><<>><>>>><<<>>><<>>><>>>><<>>><<<>>><><<>><>>><<<<>>>><<<>>><<<<>>>><<>>><<<<><<<>>>><>><<<><>>><<>><<<>>><<<><<<<>><<>>><<>><><<>><<<>><<<>>>><<<<>><><<<<><>>>><<>>>><<><<>><<>><<<><<>>>><<<>><<>>><<<>><>>>><<<>><<<<>><<>><<<<>>>><><<>>><<><>>><<<>>>><<>>><>>><<<<><<<>>><<>><<>><<>><<<>>>><<<>><<>>><<<<>>>><><<<>>>><<<<>><>>><<>><<<<><<<>>>><>><>><<<>><>>><<<>>><<<<>><<>>>><><<<>>>><<<>><<<>>>><<>>>><<>><<<>>>><>>>><>>><>>>><><>>>><<<<>><<<>>>><>><<<>>><<<><<<<><<<><<<><<<<>>>><<>>>><><<<>><<>>>><<><<<<>>>><>>><><<<<>>><<<>><<<<>><<<<>>>><<<>>><>><<<<>>>><<>><<><<>>><<<<>>>><<<<><<>><<<>><<<<>><<>>><><<>>><>>><<<<>>><<><<>>><<<<>><><<<>><<<<><<>><<><<<>><<<<>>><<<<>>><<<<><<>><>>><<>>><>>>><>><<<<>><<<<>><<<>><>>>><>><<<>>>><<<<>>><<<>>>><<><><><<<<>>>><<<>>><<<<>>>><<<<>><<>><<<>>>><<>><<><<>><<<<>>><<<>>><<<>>>><<<><<><>>><<<>>><<<>><<<><<>><<<<>><>><<<>><<<<>>><><<<><>><><<<<>>>><<>>>><<<>>><<<<>>><<<<><>><<>>>><>>><<<>>>><><<>>><<<>>>><<<>>>><<<<>>>><<<<>>><<<<>>>><<><<<<><<<>><>>>><<<<><><<>>><<>>>><<<<>><<<<>>><<<><><><>><<<>>><<<<><>><<<>>>><<<>><>><<<<>>><<><>>>><><<<><>><<<>>><<<<>><<>>><>>><<>><<>>><>>><<<><><<<<><<<><<><<<><<>><>>><>><<<<><<<>><>><<><<>>>><>>><>>>><>>>><<<<>>><>>>><<>>><<>>>><<<>><<<<>><<><<>><<<<>><<><<<>>><>><<>><<<<>><>><><<<>>>><<<>>><<>>>><>><<<<>><>>>><<>>><<>>><<<>><<<>>><<<<><<><<>><<<<>>>><<<<>><<>><>>>><<<<>>><<<<>>><<<><<<<>>>><<<<>><<<<>><>>><<<<>>><<<><<<<>>><<<<>><<<<>>>><<<<>>>><<<<>>>><<><>>>><<>>><<><>><<>>>><<><>><<>>>><<>>>><<>>>><<<<>><><>>>><<>>><<>>>><<<>>>><<<<>><<>><<>><<<><<<>>>><<<><>>><<<<>><<<<>>>><<<<>>>><<<><<<<><<<<>>>><>>>><>>>><<<<>><><<<><<<<>>><<><<<>><<<>>>><<<<>>>><<<>><<>>>><<>>>><<<<>><<<<>>><<<>>><<<>><<<>><>>><<<>>>><<<<>>>><<<>>>><>>><<<<>>>><<<<>>><<<>>><>><<<>><<<>><<<<>>><<<>>>><<><<><<<>>><<>>>><<<>>><<<>>><<>><<<<>>>><<><<><<>>><>>>><<>>>><<>>>><<<<>>>><<<<><<<<>>><>>>><<><>>><><<<><<<>><>><>>><<<>><<<<>>>><<<><><>>>><<>>><<>>>><<<>>>><><>>>><<<>><<<><<<>>>><<>>>><<>>>><<><<>>><>><<<>>><<<<>><<<<><><<<<>>>><<><>><<<>>>><><<<<>><>>>><<<>>><<<>><>><<<>><<><<<>>><<<>>><<><<>>><<<><<<>>>><<>>>><><<<<><<<<>>><><<<>>><<>>><<>><>><>>><<>>><<<>>>><<><<>>>><<<<>><<<><>><<<<><<<><><<>><<<<><<<>><<<<>>><<<<><<<<>>>><<<>><><>><<><<>>>><<><<<<>><<<>>><<<><<<>>>><<>><<<>>><>>><<>><<><<<<><>><<<<>><>><<<<><<<<><<<><>>>><<>>>><>><<>>>><<>><<>>><>>><<<>>>><<>>>><>><>>><<><<<>>><<><<><><<<<><>><<<>>><<>><<<>>><<<<>>><<<<>>><<<><><<<>>>><<<>>>><>><<>>>><>><>>>><<<>><<<<>>><<>>><<<>>><<<<>>><<<<><<>><><<<<>>>><<>><<<<>><<<><>>><<><>><>>>><<>><<<<>><<<>>><<<>>><<<<><<<<>><<<<>>>><<<>>><<><<<><<<<>><><<>><<<<>>><<><>>><<<<>>><<>><<>>>><<<<>>>><<<<>>>><>>>><<>>>><>>>><<><<><>>>><<><<<<>>>><<<><<<>>>><<>>>><<<<>>>><<<>>>><<<<>>><>>><<<><<<>><<>><<<<>><<>>><<>>><<>>><<<<><>>>><<>><<><<<<>>><<<<>>><<<<><<<<>>><<<>>>><>>>><<>>>><>>>><<>><<>>>><<<><<<<><>><<<<><<>>>><>>>><<<<>>><<>><<<<>>>><<>>><<>>>><<<<>>><<<<>>><<<<>><<<<>>>><<><<<<>><<<<>>>><<><><<<<><<<>>><<<<><<<>>><<<>><<<><<>>>><<<<>><<<<><<<<><<<<>>>><<><<>>>><<>><<<>>><<<>>>><<<>><<<>>><<>><>><<<<><<<<>><<<<><<>>>><<<<>><<<<>>>><<><<<<>>>><<><<<<><<>>><<><<<>>><<><<<<>>><<>><<<<><<<<><<<<><<><<<<>>>><<<<><<>>>><<<<>>>><>><<<<>>><>>><<<>>>><>>>><>>>><<<><>>><>><<<<>>>><<<<>><<<>>>><<<<>><<<<>>>><<<<>>>><><<<<>>>><<<>><<>>>><>>>><>>>><><<>>>><<><><<<><>>>><>>>><<<><<<>>>><<<<>>>><<<>>><<<<><<<><<>>>><<>>>><<>>><<<>><>><<<><<<<><<>>>><<<>><<>><>>>><>>>><<>>>><<<>>><>>><>>><<>><<><<<>><<>><<<<>>>><<>><<>>>><<<>><<<<>>><<<><<<<><<<<>>><>><<>><><<>><<<>><<<<>>>><<<><<>><>><<<><<>><<<<>>><<<>><<<<><<>>>><<>><<<<><<>>>><<<><>>><<><<<<>><<<<>><<>><<<<>>>><<><<<<>>><<<><<<>><<>>><<<<>>>><<><>>><<<>>><<>>><<<<>>><<<>><<<<>>><<<>>>><>>>><<>><<<<>><<>>>><<<<>>>><<>>>><<<<>><<>>>><>>>><>>><<<<>>><<<<><<<>>><<>><<<<>>><>><<<<><<<<><>>>><<<<>>>><>>>><<<<>>><<>><<<>>><<<>>><<<>>>><>>>><<<>>><<<<><<>>><<<<>>><<<<><<<<>>>><>><>>>><>>><>><<<<>>>><<>><<<>>>><<>><<>><<<<>><<<>>><<>>>><<<><<<>>><<<<>><<<<><>>>><<<<>><<<<><><<>>><<<>>>><<<<>>>><<<>>>><<>>>><<>><>><>>><<>><<<<>>>><<<>><<>><<<>>>><<<>><<<>><<<<>>><<<><<>><<<<>>>><<<<>><>>><<<>><<><<<>>><<<>>>><>>>><<<>>>><<<>><<>>>><>>>><><<<<>><<>><><>><<<<>>>><<>><<<<>>><<>><>>><<<>>><<<><<<>>><<<<>>><<>>>><<<>><<>><<>>><<<>>><>><<>><>><<<>>>><<<<>><>>><<>>><<<<>><<>><>>>><>><<<>>><<>>>><>>><><<>>>><<><<<<>>>><<<>>>><<<<><<>>><<<<><><<>>><>><>>>><<>><<<>>><<>>><<>>><<<<>><>>>><<<<>><<<<><<>>>><<<><<<<>>>><<>><<>>>><>>><<<><<<>>><<<<>>><<>>><<<<>><<<><<<<><<<>><<<<>><<>>><<<<><<<<><>>>><<<<>><<><>><<>><<>>>><><<<>>><<><>>>><<<><<<>><<<><<<>><<<<>>><<<>>>><<<>>><<>><<<"
    let result = run jetPattern 2022L
    if result <> 3235L then failwith $"wrong answer: {result}" else printfn "CORRECT ANSWER"

let run_part1_test () = 
    let jetPattern = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    let result = run jetPattern 2022L
    if result <> 3068L then failwith $"wrong answer: {result}" else printfn "CORRECT ANSWER"

let run_part2_test () = 
    let jetPattern = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    let result = run jetPattern 1000000000000L
    if result <> 1514285714288L then failwith $"wrong answer: {result}" else printfn "CORRECT ANSWER"

let run_part1 () = 
    run_part1_test ()
    run_part1_input ()

