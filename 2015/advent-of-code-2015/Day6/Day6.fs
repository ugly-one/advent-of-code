module Day6

open System.Text.RegularExpressions

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let getIndexes (x1,y1) (x2, y2) width = 
   if y1 = y2 then
      let start = x1 + y1*width
      let end_ = x2 + y1*width
      seq {start..1..end_}
   else 
      let a = seq {
         for row in y1 .. 1 .. y2 do 
            let start = x1 + row*width
            let end_ = x2 + row*width
            yield seq {start..1..end_}
      }
      Seq.collect (fun s -> s) a


let print numbers = 
   for n in numbers do 
      printf "%d," n
   printfn ""

let parse input = 
   //turn off 660,55 through 986,197
   let match_off = Regex.Match (input, "turn off ([0-9]+),([0-9]+) through ([0-9]+),([0-9]+)")
   let match_on = Regex.Match (input, "turn on ([0-9]+),([0-9]+) through ([0-9]+),([0-9]+)")
   let match_toggle = Regex.Match (input, "toggle ([0-9]+),([0-9]+) through ([0-9]+),([0-9]+)")

   let get (match_: Match) x = 
      (x, (match_.Groups[1].Value |> int, match_.Groups[2].Value |>int), (match_.Groups[3].Value |> int, match_.Groups[4].Value |> int))

   if match_off.Success then 
      get match_off 0
   elif match_on.Success then 
      get match_on 1
   else 
      get match_toggle 2

let run_ input width = 
   let grid = Array.create (width * width) 0
   for action in input do 
      let (action, leftTop, bottomRight) = action
      let lightsToChange = getIndexes leftTop bottomRight width |> Array.ofSeq
      for light in lightsToChange do 
         match (action, grid[light]) with 
         | 1,_ -> grid[light] <- grid[light] + 1
         | 0,_ -> grid[light] <- if grid[light] > 0 then grid[light] - 1 else 0
         | 2,_ -> grid[light] <- grid[light] + 2
   grid

let run () =

   getIndexes (0,0) (4,0) 10 |> print
   getIndexes (0,1) (4,1) 10 |> print
   getIndexes (0,0) (4,2) 10 |> print
   getIndexes (1,0) (1,0) 10 |> print
   getIndexes (4,4) (5,5) 10 |> print
   getIndexes (4,4) (4,6) 10 |> print

   let testInput = [
      (2, (0,0), (9,9)) // all on
      (0, (5,5), (6,6)) // 96 on
      // (2, (0,0), (9,9)) // 4 on
      ]
   run_ testInput 10 |> Array.sum |> printfn "%d"


   let input = readLines "Day6/input.txt" |> Seq.map parse |> Array.ofSeq
   run_ input 1_000 |> Array.sum |> printfn "%d"
   ()