module Day2 
open Day2_input

let calculate input =
    let folder (h,d) item = 
        match item with 
        | ("forward", x) -> (h + x, d)
        | ("down", x) -> (h, d + x)
        | ("up", x) -> (h, d - x)
        | _ -> (h,d)

    let (h, d) = Array.fold folder (0,0) input
    h * d

let calculate2 input = 
    let folder (h,d,a) item = 
        match item with 
        | ("forward", x) -> (h + x, d + a*x, a)
        | ("down", x) -> (h, d, a + x)
        | ("up", x) -> (h, d, a - x)
        | _ -> (h,d,a)

    let (h, d, a) = Array.fold folder (0,0,0) input
    h * d

let run1 = 
    calculate input |> printfn "%d" 

let run2 = 
    calculate2 input |> printfn "%d"