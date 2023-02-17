module Day9

open Day9_input

let input = input3
let rowSize = input.RowSize

type IsMinimum = 
    | Yes
    | NotSure
    | No

type Point = 
    {   
        Value: int
        IsMinimum: IsMinimum 
    }

let isLocalMinimum (point: Point) (neigbours: List<Point>) : IsMinimum=
    // TODO convert to computational expression?!
    let mutable a = IsMinimum.Yes;
    let neigboursWithSameValue = List.filter (fun p -> p.Value = point.Value) neigbours
    let neigboursWithLowerValue = List.filter (fun p -> p.Value < point.Value) neigbours 
    let neigboursWithHigherValue = List.filter (fun p -> p.Value > point.Value) neigbours 

    // if we have at least one neighbour with lower value - this neighbour is a potential minimum
    if (List.length neigboursWithLowerValue > 0) then IsMinimum.No 
    // we do not have anyone lower, but we have all neighbors already visited - this position is a minimum
    elif (((List.filter (fun p -> p.IsMinimum = NotSure) neigboursWithSameValue) |> List.length) = List.length neigboursWithSameValue) then IsMinimum.Yes
    elif (List.length neigboursWithSameValue > 0) then IsMinimum.NotSure
    elif (List.length neigboursWithHigherValue = List.length neigbours) then IsMinimum.Yes
    else IsMinimum.No


let AddLeftNeigbour input index neighbours=
    let newIndex = index - 1;
    if (newIndex >= 0) then Array.item newIndex input :: neighbours else neighbours

let AddRightNeigbour input index neighbours =
    let newIndex = index + 1;
    if (newIndex < (Array.length input)) then input[newIndex] :: neighbours else neighbours

let AddTopNeigbour input index neighbours =
    let newIndex = index - rowSize;
    if (newIndex >= 0) then Array.item newIndex input :: neighbours else neighbours

let AddBottomNeigbour input index neighbours =
    let newIndex = index + rowSize;
    if (newIndex < (Array.length input)) then input[newIndex] :: neighbours else neighbours

let GetNeighbours input index = 
    let empty : List<Point> = []
    empty |> (AddLeftNeigbour input index) |> (AddBottomNeigbour input index) |> (AddRightNeigbour input index) |> (AddTopNeigbour input index) 

let calculateLocalMinima input = 
    let mutable localMinima = []
    let size = Array.length input
    let sizeMinusOne = size - 1
    for index = 0 to sizeMinusOne do
        let neighbours = GetNeighbours input index
        let point = input[index]
        match isLocalMinimum point neighbours with
        | No -> localMinima <- localMinima
        | Yes -> 
            Array.set input index {point with IsMinimum = Yes}
            localMinima <- input[index] :: localMinima 
        | NotSure -> 
            Array.set input index {point with IsMinimum = NotSure}

    localMinima

let printPoint point: unit = 
    printfn "%d" point.Value
    ()

let run1 = 
    let arrayInput = Array.map (fun i -> {Value = i; IsMinimum = No}) input.Values
    let result = calculateLocalMinima arrayInput
    result |> List.iter printPoint
    let values = List.map (fun p -> p.Value) result
    printfn "%d" (List.sum values + List.length values)