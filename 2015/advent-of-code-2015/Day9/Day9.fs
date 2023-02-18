module Day9

open System.Text.RegularExpressions

let run () =
   let input = InputReader.readLines   "Day9/input.txt"

   let fold acc line = 
      let r = Regex.Match(line, "([a-zA-Z]+) to ([a-zA-Z]+) = ([0-9]+)")
      let city1 = r.Groups[1] |> string
      let city2 = r.Groups[2] |> string 
      let distance = r.Groups[3] |> string |> int
      acc 
      |> Map.add (city1, city2) distance 
      |> Map.add (city2, city1) distance

   let distances = input |> Array.fold fold Map.empty 
   // let distances = 
   //    Map.empty 
   //    |> Map.add ("London", "Dublin") 464
   //    |> Map.add ("Dublin", "London") 464
   //    |> Map.add ("London", "Belfast") 518
   //    |> Map.add ("Belfast", "London") 518
   //    |> Map.add ("Dublin", "Belfast") 141
   //    |> Map.add ("Belfast", "Dublin") 141
   let cities = distances |> Map.keys |> Seq.map (fun (c,r) -> c) |> Set.ofSeq |> Array.ofSeq

   // let cities = [| "London"; "Dublin"; "Belfast"|]

   let rec findShortestPath (distances: Map<(string*string), int>) (remainingCities: string[]) (path: string[]) totalDistance (shortestDistance: option<int>) = 
      if Array.isEmpty remainingCities then 
         totalDistance
      else 
         let previousCity = Array.tryLast path
         let mutable maybeShortest = shortestDistance
         for city in remainingCities do 
         
            let distance = 
               match previousCity with 
               | None -> 0
               | Some previousCity -> distances[(city, previousCity)]

            let remainingCities = remainingCities |> Array.filter (fun c -> c <> city)
            let path = Array.append path [| city |]
            let totalDistance = distance + totalDistance
            match maybeShortest with 
            | None -> 
               let distance = findShortestPath distances remainingCities path totalDistance maybeShortest
               maybeShortest <- Some distance
            | Some shortest -> 
               // if totalDistance >= shortest 
               // then ()
               // else 
                  let distance = findShortestPath distances remainingCities path totalDistance maybeShortest
                  if distance > shortest then maybeShortest <- Some distance else ()
         maybeShortest.Value

   let result = findShortestPath distances cities Array.empty 0 None
   result |> printfn "%d"
   ()