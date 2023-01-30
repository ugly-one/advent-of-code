module Day22_part2

open Day22

type WallId = int

type WallInfo = {
    Size: int
    Id: WallId
    LeftTopPosition: Position
}

type Wall = {
    Info: WallInfo
    Left: (WallId * Direction)
    Right: (WallId * Direction)
    Top: (WallId * Direction)
    Bottom: (WallId * Direction)
}

type Net = {
    Map: Map
    Walls: Wall[]
}

let findWall wallId walls = 
    walls |> Array.find (fun wall -> wall.Info.Id = wallId)

let simpleMove (pos: Position) (direction: Direction) = 
    let (x, y) = 
        match direction with 
        | Up -> (pos.X, pos.Y - 1)
        | Down -> (pos.X, pos.Y + 1)
        | Left -> (pos.X - 1, pos.Y)
        | Right -> (pos.X + 1, pos.Y)
    {X = x; Y = y}    

let isWithinWall (pos: Position) (wall: Wall) = 
    let leftToPosition = wall.Info.LeftTopPosition
    if pos.X < leftToPosition.X || pos.X > leftToPosition.X + wall.Info.Size - 1 then false 
    elif pos.Y < leftToPosition.Y || pos.Y > leftToPosition.Y + wall.Info.Size - 1 then false
    else true

let getIndex (pos: Position) (wall: Wall) (edge: Direction) = 
    match edge with 
    | Right -> pos.Y - wall.Info.LeftTopPosition.Y
    | Left -> pos.Y - wall.Info.LeftTopPosition.Y 
    | Up -> pos.X - wall.Info.LeftTopPosition.X
    | Down -> pos.X - wall.Info.LeftTopPosition.X

let getPosFromStart (index: int) (wall: Wall) (edge: Direction) = 
    let (x, y) = 
        match edge with 
        | Right -> (wall.Info.LeftTopPosition.X + wall.Info.Size - 1, wall.Info.LeftTopPosition.Y + index)
        | Left -> (wall.Info.LeftTopPosition.X, wall.Info.LeftTopPosition.Y + index)
        | Up -> (wall.Info.LeftTopPosition.X + index, wall.Info.LeftTopPosition.Y)
        | Down -> (wall.Info.LeftTopPosition.X + index, wall.Info.LeftTopPosition.Y + wall.Info.Size - 1)
    {X = x; Y = y}

let getPosFromEnd (index: int) (wall: Wall) (edge: Direction) = 
    let (x, y) = 
        match edge with 
        | Right -> (wall.Info.LeftTopPosition.X + wall.Info.Size - 1, wall.Info.LeftTopPosition.Y + wall.Info.Size - 1 - index)
        | Left -> (wall.Info.LeftTopPosition.X, wall.Info.LeftTopPosition.Y + wall.Info.Size  - 1 - index)
        | Up -> (wall.Info.LeftTopPosition.X + wall.Info.Size - 1 - index, wall.Info.LeftTopPosition.Y)
        | Down -> (wall.Info.LeftTopPosition.X + wall.Info.Size - 1 - index, wall.Info.LeftTopPosition.Y + wall.Info.Size - 1)
    {X = x; Y = y}

let move (pos: Position) (direction: Direction) (currentWall: Wall) (allWalls: Wall[])= 
    let positionAfterSimpleMove = simpleMove pos direction
    let withinWall = isWithinWall positionAfterSimpleMove currentWall
    if withinWall 
    then (positionAfterSimpleMove, direction, currentWall)
    else 
        let indexOnCurrentWallEdge = getIndex pos currentWall direction
        let (pos, direction, nextWall) = 
            match direction with 
            | Up -> 
                let (nextWallId, wallDirection) = currentWall.Top
                let nextWall = findWall nextWallId allWalls
                match wallDirection with 
                    | Up -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Down -> (getPosFromStart indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Left -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Right -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
            | Right -> 
                let (nextWallId, wallDirection) = currentWall.Right
                let nextWall = findWall nextWallId allWalls
                match wallDirection with 
                    | Up -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Down -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Left -> (getPosFromStart indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Right -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
            | Left -> 
                let (nextWallId, wallDirection) = currentWall.Right
                let nextWall = findWall nextWallId allWalls
                match wallDirection with 
                    | Up -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Down -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Left -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Right -> (getPosFromStart indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
            | Down -> 
                let (nextWallId, wallDirection) = currentWall.Right
                let nextWall = findWall nextWallId allWalls
                match wallDirection with 
                    | Up -> (getPosFromStart indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Down -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Left -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
                    | Right -> (getPosFromEnd indexOnCurrentWallEdge nextWall wallDirection, wallDirection, nextWall)
        (pos, direction, nextWall)

let walk (pos: Position) (currentWall: Wall) (allWalls: Wall[]) (direction: Direction) (steps: int) (map: Map) = 
    let mutable pos = pos
    let mutable direction = direction
    let mutable wall = currentWall
    for step in 1 .. 1 .. steps do 
        let (newPosition, newDirection, nextWall) = move pos direction wall allWalls
        match map.Content[newPosition.Y][newPosition.X] with 
            | '.' -> 
                pos <- newPosition
                direction <- newDirection
                wall <- nextWall
            | '#' -> () 
            | _ -> failwith "how did we jump out of the cube?!"
    (pos, direction, wall)

let tests map = 
    
    let cubeWallSize = 4
    let mutable wallId = 1
    let walls = seq {
        for rowIndex in 0 .. cubeWallSize .. map.Height - 1 do 
            let row = map.Content[rowIndex]
            for columnIndex in 0 .. cubeWallSize .. row.Length - 1 do 
                let item = row[columnIndex]
                match item with 
                | ' ' -> yield None
                | _ -> 
                    yield Some {Id = wallId; Size = cubeWallSize; LeftTopPosition = {X = columnIndex ; Y = rowIndex}}
                    wallId <- wallId + 1
    }

    let walls = walls |> Array.ofSeq

    let getWall id = 
        walls 
        |> Seq.filter (fun wall -> wall.IsSome) 
        |> Seq.map (fun wall -> wall.Value)
        |> Seq.find (fun wall -> wall.Id = id) 

    let wall1 = {
        Info = getWall 1; 
        Left = (3, Up); 
        Top = (2, Up); 
        Right = (6, Right); 
        Bottom = (4, Up)}

    let wall2 = {
        Info = getWall 2; 
        Left = (6, Down); 
        Top = (1, Up); 
        Right = (3, Left); 
        Bottom = (5, Down)}

    let wall3 = {
        Info = getWall 3; 
        Left = (2, Right); 
        Top = (1, Right); 
        Right = (4, Left); 
        Bottom = (5, Right)}

    let wall4 = {
        Info = getWall 4; 
        Left = (3, Right); 
        Top = (1, Down); 
        Right = (6, Up); 
        Bottom = (5, Up)}

    let wall5 = {
        Info = getWall 5; 
        Left = (3, Down); 
        Top = (4, Down); 
        Right = (6, Left); 
        Bottom = (2, Down)}
    
    let wall6 = {
        Info = getWall 6; 
        Left = (5, Right); 
        Top = (4, Right); 
        Right = (1, Right); 
        Bottom = (2, Left)}

    [| wall1; wall2; wall3; wall4; wall5; wall6; |]

let testWalkWithinWall () = 
    let (map, _, _) = parseOnTestInput ()
    let walls = tests map
    let wall1 = findWall 1 walls
    let (position, _, _) = walk {X = 11; Y = 3} wall1 walls Left 1 map
    if position <> {X = 10; Y = 3} then failwithf "you can't even go left within a wall. %d-%d" position.X position.Y else printfn "going left within a wall OK"


let testWalkToAnotherWall () = 
    let (map, _, _) = parseOnTestInput ()
    let walls = tests map
    let currentWall = findWall 4 walls
    let (position, _, _) = walk {X = 11; Y = 5} currentWall walls Right 1 map
    if position <> {X = 14; Y = 8} then failwithf "you can't move to another wall. %d-%d" position.X position.Y else printfn "going to another wall OK"

let testInput () = 
    let (map, startingPosition, path) = parseOnTestInput ()
    let walls = tests map
    let currentWall = findWall 1 walls
    let mutable direction = Right
    let mutable position = {X = startingPosition; Y = 0}
    let mutable wall = currentWall
    for movement in path do 
        match movement with 
        | Walk steps -> 
            let (newPosition, newDirection, newWall) = walk position wall walls direction steps map
            position <- newPosition
            direction <- newDirection
            wall <- newWall
        | Rotate rotation -> direction <- rotate rotation direction
    let result = 1000 * (position.Y + 1) + 4 * (position.X + 1) + (directionValue direction)
    if result <> 5031 then failwithf "failed test input - final calculation. %d" result else printfn "test input OK"

let part2 () = 
    testWalkWithinWall ()
    testWalkToAnotherWall ()
    testInput ()