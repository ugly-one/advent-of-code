module Day1

open Day1_input

let calculate input =
    let folder item (result, maybeNextItem) = 
        match maybeNextItem with 
            | Some(nextItem) -> if nextItem > item then (result + 1, Some(item)) else (result, Some(item))
            | None -> (result, Some(item))

    let (a, _) = Array.foldBack folder input (0, None)
    a

let calculateWindow input = 
    let folder (item: int) (result, w1, w2) = 
        if (Array.length w1 = 0) then 
            let newW1 = Array.append [|item|] w1
            (result, newW1, w2)
        elif (Array.length w1 = 1 || Array.length w1 = 2) then 
            let newW1 = Array.append [|item|] w1
            let newW2 = Array.append [|item|] w2
            (result, newW1, newW2)
        else 
            let newW2 = Array.append [|item|] w2
            // take 2 first items from newW2 and place it as w1
            if (Array.sum w1 > Array.sum newW2) then (result + 1, newW2, newW2[..1]) else (result, newW2, newW2[..1])
         
    let (a, _, _) = Array.foldBack folder input (0, [||], [||])
    a

let run1 = 
    calculate input

let run2 =
    calculateWindow input