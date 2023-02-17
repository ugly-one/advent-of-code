module Day2

let input = System.IO.File.ReadAllLines "Day2/input.txt"

let getArea (line: string) = 
   let items =  line.Split 'x' |> Array.map int
   let l = items[0]
   let w = items[1]
   let h = items[2]
   let areas = [| l*w; w*h; h*l |]
   let smallestArea = Array.min areas
   let areas = Array.map (fun area -> area * 2) areas
   (Array.sum areas) + smallestArea

let getAreaFoRibbon (line: string) = 
   let items =  line.Split 'x' |> Array.map int
   let biggestArea = Array.max items
   let biggestItemIndes = items |> Array.findIndex (fun item -> item = biggestArea)
   let itemsForPresent = items |> Array.removeAt biggestItemIndes
   let areaForPresent = itemsForPresent[0] + itemsForPresent[0] + itemsForPresent[1] + itemsForPresent[1]
   let areaForBow = items[0] * items[1] * items[2]
   areaForPresent + areaForBow


let run () = input |> Array.map getAreaFoRibbon |> Array.sum |> printfn "%d"