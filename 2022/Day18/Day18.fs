module Day18

open System.Collections.Generic

let getSurfaceArea3Dimentions (cubeX,cubeY,cubeZ) =
    [|
        (cubeX, cubeY, cubeZ + 1);
        (cubeX, cubeY, cubeZ - 1);
        (cubeX, cubeY + 1, cubeZ);
        (cubeX, cubeY - 1, cubeZ);
        (cubeX + 1, cubeY, cubeZ);
        (cubeX - 1, cubeY, cubeZ);
    |]

let getSurfaceArea2Dimentions (cubeX,cubeY) =
    [|
        (cubeX, cubeY + 1);
        (cubeX, cubeY - 1);
        (cubeX + 1, cubeY);
        (cubeX - 1, cubeY);
    |]

let getSurfaceArea1Dimention (cubeX) =
    [|
        (cubeX + 1);
        (cubeX - 1);
    |]

let addCube getSurfaceArea totalSurfaceArea allPoints newPoint  = 
    let newPointSurfaceArea = getSurfaceArea newPoint |> Array.ofSeq

    if totalSurfaceArea |> Seq.contains newPoint then 
        // step 1, remove all points from totalSurfaceArea equal to point
        let newTotalSurfaceArea = totalSurfaceArea |> Array.filter (fun areaPoint -> newPoint <> areaPoint)
        // step 2, remove all points from cubeSurfaceArea that are found in allPoints
        let newPointSurfaceAreaWithoutCollisions = newPointSurfaceArea |> Array.filter (fun pointFromNewPointSurface -> Array.contains pointFromNewPointSurface allPoints |> not)
        (Array.append newTotalSurfaceArea newPointSurfaceAreaWithoutCollisions, Array.append allPoints [| newPoint |])
    else 
        (Array.append totalSurfaceArea newPointSurfaceArea, Array.append allPoints [| newPoint |])


let run getSurfaceArea input = 
    let addCube = addCube getSurfaceArea
    input |> Seq.fold (fun (surfaceArea, allPoints) cube -> addCube surfaceArea allPoints cube) (Array.empty, Array.empty)

let runTestCases () = 
    let run = run getSurfaceArea3Dimentions
    let cubes = 
        [|
            (1,1,1);
            (2,1,1)
        |]
    let result = run cubes |> fst
    if result.Length <> 10 then printfn $"Test case 1 Wrong answer {result.Length}" else printfn "Test case 1 OK"

    let cubes = 
        [|
            (1,1,1);
            (3,1,1);
            (2,1,1)
        |]
    let result = run cubes |> fst
    if result.Length <> 14 then printfn $"Test case 2 Wrong answer {result.Length}" else printfn "Test case 2 OK"

    let result = run Day18_input.testInput |> fst
    if result.Length <> 64 then printfn $"Test case 3 Wrong answer {result.Length}" else printfn "Test case 3 OK"

    let cubes = Day18_input.input
    let result = run cubes |> fst
    if result.Length <> 3550 then printfn $"Test case 4 Wrong answer {result.Length}" else printfn "Test case 4 OK"

let rec hasWayOut getSurfaceArea (points: 'a array) obstacles border (visitedPoints: HashSet<'a>) (cache: Dictionary<'a, bool>) (continuation: bool -> unit) printPoint = 
    match points |> List.ofArray with 
    | [] -> continuation false
    | point::points -> 
        let isInCache, resultInCache = cache.TryGetValue point
        if isInCache then 
            if resultInCache then 
                continuation true 
            else 
                let points = points |> Seq.filter (fun item -> visitedPoints.Contains item |> not) |> Array.ofSeq
                visitedPoints.Add(point) |> ignore
                hasWayOut getSurfaceArea (points) obstacles border visitedPoints cache continuation printPoint
        if Seq.contains point border then 
            continuation true
        else if Seq.contains point obstacles then 
            let points = points |> Seq.filter (fun item -> visitedPoints.Contains item |> not) |> Array.ofSeq
            visitedPoints.Add(point) |> ignore
            hasWayOut getSurfaceArea (points) obstacles border visitedPoints cache continuation printPoint
        else 
            visitedPoints.Add(point) |> ignore
            let neighborPoints = getSurfaceArea point
            let allPointsToCheck = Seq.append neighborPoints points |> Seq.filter (fun item -> visitedPoints.Contains item |> not) |> Array.ofSeq
            hasWayOut getSurfaceArea allPointsToCheck obstacles border visitedPoints cache continuation printPoint

type Test = 
    {
        mutable Value: bool
    }

let findWayOut getSurfaceArea printPoint surface points border = 
    let mutable index = 0
    let cache = new Dictionary<'a, bool>()

    seq{ 
        for point in surface do 
            let result = {Value = false}
            let visitedPoints = new HashSet<'a>()
            hasWayOut getSurfaceArea [|point|] points border visitedPoints cache (fun a -> result.Value <- (result.Value || a)) printPoint
            cache.TryAdd(point, result.Value) |> ignore
            (index |> decimal) * 100m / ((Seq.length surface) |> decimal) |> printfn "%f"
            index <- index + 1
            yield result
    }

let generate2DimentionalBorder min max = 
    seq {
        for item in min .. 1 .. max do 
            yield (min, item)
            yield (max, item)
            yield (item, min)
            yield (item, max)
    }

let generate3DimentionalBorder min max = 
    seq {
        for x in min .. 1 .. max do 
            for y in min .. 1 .. max do 
                yield (x, y, min)
                yield (x, y, max)
        for x in min .. 1 .. max do 
            for z in min .. 1 .. max do 
                yield (x, min, z)
                yield (x, max, z)
        for y in min .. 1 .. max do 
            for z in min .. 1 .. max do 
                yield (min, y, z)
                yield (max, y, z)
    }

let print1 x = 
    printf "%d" x

let print2 (x, y) = 
    printf "(%d,%d)" x y

let print3 (x, y, z) = 
    printf "(%d,%d,%d)" x y z

let check expected result = 
    if result <> expected then printfn $"Wrong answer {result}. Expected: {expected}" else printfn "OK"

let runPart2TestCases () = 
    printfn "part 2"
    printfn "one dimention - test case 1. "
    let cubes = 
        [|
            (1);
            (3)
        |]
    let run1Dimention = run getSurfaceArea1Dimention
    let (surface, points) = run1Dimention cubes
    let result = findWayOut getSurfaceArea1Dimention print1 surface points [0;4] |> Seq.filter (fun i -> i.Value) |> Seq.length
    check 2 result

    printfn "one dimention - test case 2. "
    let cubes = 
        [|
            (1);
            (3);
            (7)
        |]
    let (surface, points) = run1Dimention cubes
    let result = findWayOut getSurfaceArea1Dimention print1 surface points [0;9] |> Seq.filter (fun i -> i.Value) |> Seq.length
    check 2 result

let runPart2TestCases2 () = 
    printfn "two dimentions - test case 1. "
    let run2Dimentions = run getSurfaceArea2Dimentions

    let cubes = 
        [|
            (1,1);
            (3,3);
            (3,2);
        |]
    let (surface, points) = run2Dimentions cubes
    let border = generate2DimentionalBorder 0 4
    let result = findWayOut getSurfaceArea2Dimentions print2 surface points border |> Seq.filter (fun i -> i.Value) |> Seq.length
    check 10 result

    printfn "two dimentions - test case 2. "
    let cubes = 
        [|
            (2,2);
            (2,3);
            (2,4);
            (4,2);
            (4,3);
            (4,4);
            (3,2);
            (3,4);
        |]
    let (surface, points) = run2Dimentions cubes
    let border = generate2DimentionalBorder 0 6
    let result = findWayOut getSurfaceArea2Dimentions print2 surface points border |> Seq.filter (fun i -> i.Value) |> Seq.length
    let expected = 12
    check expected result

let runPart2TestCases3 () = 
    printfn "three dimentions - test input. "
    let cubes = Day18_input.testInput
    let run3Dimentions = run getSurfaceArea3Dimentions
    let a cube = getSurfaceArea3Dimentions cube
    let (surface, points) = run3Dimentions cubes
    let border = generate3DimentionalBorder 0 7
    let result = findWayOut a print3 surface points border |> Seq.filter (fun i -> i.Value) |> Seq.length
    check 58 result

    // TODO this is very slooooow - maybe I should use the cache (if it works :D) that is available
    let cubes = Day18_input.input
    let run3Dimentions = run getSurfaceArea3Dimentions
    let a cube = getSurfaceArea3Dimentions cube
    let (surface, points) = run3Dimentions cubes
    let border = generate3DimentionalBorder -1 20
    let result = findWayOut a print3 surface points border |> Seq.filter (fun i -> i.Value) |> Seq.length
    check 2028 result
