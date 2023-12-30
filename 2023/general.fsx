open System.Collections.Generic

let print item =
    printfn "%A" item
    item

let printSeq seq =
    seq |> Seq.map (printfn "%A") |> Seq.length |> ignore
    seq
    

let printDic (dic: IDictionary<int,int>) =
    dic |> Seq.map (fun item -> printfn "%A -> %A" item.Key item.Value)
    |> Seq.length
    
let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq