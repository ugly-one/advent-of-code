open System
open System.Text.RegularExpressions

type Direction = 
     | Up
     | Down
     | Left
     | Right

let rotate direction rotation = 
     match (direction, rotation) with 
     | (Up, 'L') -> Left
     | (Up, 'R') -> Right
     | (Down, 'L') -> Right
     | (Down, 'R') -> Left
     | (Right, 'L') -> Up
     | (Right, 'R') -> Down
     | (Left, 'L') -> Down
     | (Left, 'R') -> Up
     | _ -> failwith "WHAT?!"

let move_ (x,y) direction steps = 
     seq { 
          match direction with 
          | Up -> 
               for step in 1 .. steps do 
                    yield (x, y - step)
                    
          | Down -> 
               for step in 1 .. steps do 
                    yield (x, y + step)
          | Left -> 
               for step in 1 .. steps do 
                    yield (x - step, y )
               
          | Right -> 
               for step in 1 .. steps do 
                    yield (x + step, y)
               
     }

let input = "L3, R1, L4, L1, L2, R4, L3, L3, R2, R3, L5, R1, R3, L4, L1, L2, R2, R1, L4, L4, R2, L5, R3, R2, R1, L1, L2, R2, R2, L1, L1, R2, R1, L3, L5, R4, L3, R3, R3, L5, L190, L4, R4, R51, L4, R5, R5, R2, L1, L3, R1, R4, L3, R1, R3, L5, L4, R2, R5, R2, L1, L5, L1, L1, R78, L3, R2, L3, R5, L2, R2, R4, L1, L4, R1, R185, R3, L4, L1, L1, L3, R4, L4, L1, R5, L5, L1, R5, L1, R2, L5, L2, R4, R3, L2, R3, R1, L3, L5, L4, R3, L2, L4, L5, L4, R1, L1, R5, L2, R4, R2, R3, L1, L1, L4, L3, R4, L3, L5, R2, L5, L1, L1, R2, R3, L5, L3, L2, L1, L4, R4, R4, L2, R3, R1, L2, R1, L2, L2, R3, R3, L1, R4, L5, L3, R4, R4, R1, L2, L5, L3, R1, R4, L2, R5, R4, R2, L5, L3, R4, R1, L1, R5, L3, R1, R5, L2, R1, L5, L2, R2, L2, L3, R3, R3, R1"

let mutable direction = Up 
let mutable position = (0,0)

let moves = input.Split ',' |> Array.map (fun move -> move.TrimStart ())

let mutable locations = Set.empty |> Set.add position

let mutable result = 0
for move in moves do 
     let rotation = move[0]
     let width = move.Length
     let steps = move[1..width - 1]
     direction <- rotate direction rotation
     let newPositions = move_ position direction (steps |> int)
     for newPosition in newPositions do 
          position <- newPosition
          if Set.contains position locations then
               let (x,y) = position
               failwithf "%d" (Math.Abs x + Math.Abs y)
          else 
               locations <- (Set.add position locations)


printf "%d" locations.Count
// let (x,y) = position
// printfn "%d" (Math.Abs x + Math.Abs y)
