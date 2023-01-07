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

let rec hasWayOut getSurfaceArea point points border (visitedPoints: HashSet<'a>) (cache: Dictionary<'a, bool>) = 
    let (success, result) = cache.TryGetValue point
    if success then result 
    else if Seq.contains point visitedPoints then 
        false
    else if Seq.contains point border then 
        visitedPoints.Add(point) |> ignore // this is done so the cache will have the result for this point
        true
    else if Seq.contains point points then 
        false
    else 
        visitedPoints.Add(point) |> ignore
        let neighborPoints = getSurfaceArea point
        neighborPoints |> Seq.fold (fun state neighbor -> state || (hasWayOut getSurfaceArea neighbor points border visitedPoints cache)) false 

let findWayOut getSurfaceArea printPoint surface points border = 
    let mutable index = 0
    let cache = new Dictionary<'a, bool>()
    seq{ 
        for point in surface do 
            printf $"checking... "
            printPoint point
            let visitedPoints = new HashSet<'a>()
            let hasWayOut = hasWayOut getSurfaceArea point points border visitedPoints cache
            for visitedPoint in visitedPoints do 
                cache.Add(visitedPoint, hasWayOut)
            (index |> decimal) * 100m / ((Seq.length surface) |> decimal) |> printfn "%f"
            index <- index + 1
            yield hasWayOut
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
        for item in min .. 1 .. max do 
            yield (min, max, item)
            yield (max, min, item)
            yield (min, item, max)
            yield (max, item, min)
            yield (item, min, max)
            yield (item, max, min)
    }

let print1 x = 
    printfn "%d" x

let print2 (x, y) = 
    printfn "(%d,%d)" x y

let print3 (x, y, z) = 
    printfn "(%d,%d,%d)" x y z

let runPart2TestCases () = 
    printfn "part 2"
    printf "one dimention - test case 1. "
    let cubes = 
        [|
            (1);
            (3)
        |]
    let run1Dimention = run getSurfaceArea1Dimention
    let (surface, points) = run1Dimention cubes
    let result = findWayOut getSurfaceArea1Dimention print1 surface points [0;4]  |> Seq.filter (fun item -> item)
    let expected = 2
    if Seq.length result <> expected then printfn $"Wrong answer {Seq.length result}. Expected: {expected}" else printfn "OK"

    printf "one dimention - test case 2. "
    let cubes = 
        [|
            (1);
            (3);
            (7)
        |]
    let (surface, points) = run1Dimention cubes
    let result = findWayOut getSurfaceArea1Dimention print1 surface points [-4;10]  |> Seq.filter (fun item -> item)
    let expected = 2
    if Seq.length result <> expected then printfn $"Wrong answer {Seq.length result}. Expected: {expected}" else printfn "OK"

    printf "two dimentions - test case 1. "
    let run2Dimentions = run getSurfaceArea2Dimentions

    let cubes = 
        [|
            (1,1);
            (3,3);
            (3,2);
        |]
    let (surface, points) = run2Dimentions cubes
    let border = generate2DimentionalBorder 0 4
    let result = findWayOut getSurfaceArea2Dimentions print2 surface points border |> Seq.filter (fun item -> item)
    let expected = 10
    if Seq.length result <> expected then printfn $"Wrong answer {Seq.length result}. Expected: {expected}" else printfn "OK"

    printf "two dimentions - test case 2. "
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
    let result = findWayOut getSurfaceArea2Dimentions print2 surface points border |> Seq.filter (fun item -> item)
    let expected = 12
    if Seq.length result <> expected then printfn $"Wrong answer {Seq.length result}. Expected: {expected}" else printfn "OK"

    printfn "three dimentions - test input. "
    let cubes = Day18_input.testInput
    let run3Dimentions = run getSurfaceArea3Dimentions

    let (surface, points) = run3Dimentions cubes
    let border = generate3DimentionalBorder 1 6
    let result = findWayOut getSurfaceArea3Dimentions print3 surface points border |> Seq.filter (fun item -> item)
    let expected = 58
    if Seq.length result <> expected then printfn $"Wrong answer {Seq.length result}. Expected: {expected}" else printfn "OK"
