open System.Collections.Generic

let printSeq seq =
    seq |> Seq.map (fun item -> printfn "%A" item)
    |> Seq.length

let printDic (dic: IDictionary<int,int>) =
    dic |> Seq.map (fun item -> printfn "%A -> %A" item.Key item.Value)
    |> Seq.length