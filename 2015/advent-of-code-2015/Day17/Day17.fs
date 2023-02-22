module Day17

open System.Collections.Generic
open System.Text.RegularExpressions

type Container = {
    Id: int
    Size: int
}
let run () =
    let input = [| 20; 15; 10; 5; 5 |]
    let total = 25
    let input = InputReader.readLines "Day17/input.txt" |> Seq.map (fun line -> line |> int)
    let total = 150
    
    let containers = 
        input
        |> Seq.fold (fun (index, acc) c -> (index + 1, acc |> Array.append [| {Id = index; Size = c}  |] )) (0, Array.empty)
        |> snd
        
    
    let rec fill (emptyContainers: Container[]) (filledContainers: Container[]) liters =
        let filledLiters = filledContainers |> Array.sumBy (fun c -> c.Size)
        if filledLiters = liters then
            [| filledContainers |> Array.sortBy (fun c -> c.Id) |]
        elif filledLiters < liters then
            let results = new List<Container[]>()
            for emptyContainer in emptyContainers do
                let filledContainers = filledContainers |> Array.append [| emptyContainer |]
                let emptyContainerIndex = emptyContainers |> Array.findIndex (fun c -> c = emptyContainer)
                let emptyContainers = emptyContainers |> Array.removeAt emptyContainerIndex
                let result = fill emptyContainers filledContainers liters
                results.AddRange result
            results |> Array.ofSeq
        else Array.empty
    ()
    
    let print (containers: Container[]) =
        for container in containers do
            printf "[%d] %d," container.Id container.Size
        printfn ""
        containers
        
    // TODO this is very slow.
    // Plus, I was lucky enough to print all sets (while getting answer to part 1) and they are order by the amount of containers
    // so it was easy to calculate manually the answer for part 2
    fill containers Array.empty total |> Set.ofArray |> Seq.map print |> Seq.length |> printfn "%d"