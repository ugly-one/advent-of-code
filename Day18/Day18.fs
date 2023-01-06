module Day18

let getSurfaceArea (cubeX,cubeY,cubeZ) =
    [|
        (cubeX, cubeY, cubeZ + 1);
        (cubeX, cubeY, cubeZ - 1);
        (cubeX, cubeY + 1, cubeZ);
        (cubeX, cubeY - 1, cubeZ);
        (cubeX + 1, cubeY, cubeZ);
        (cubeX - 1, cubeY, cubeZ);
    |]

let addCube totalSurfaceArea allPoints newPoint = 
    let newPointSurfaceArea = getSurfaceArea newPoint |> Array.ofSeq

    if totalSurfaceArea |> Seq.contains newPoint then 
        // step 1, remove all points from totalSurfaceArea equal to point
        let newTotalSurfaceArea = totalSurfaceArea |> Array.filter (fun areaPoint -> newPoint <> areaPoint)
        // step 2, remove all points from cubeSurfaceArea that are found in allPoints
        let newPointSurfaceAreaWithoutCollisions = newPointSurfaceArea |> Array.filter (fun pointFromNewPointSurface -> Array.contains pointFromNewPointSurface allPoints |> not)
        (Array.append newTotalSurfaceArea newPointSurfaceAreaWithoutCollisions, Array.append allPoints [| newPoint |])
    else 
        (Array.append totalSurfaceArea newPointSurfaceArea, Array.append allPoints [| newPoint |])


let run input = 
    input |> Seq.fold (fun (surfaceArea, allPoints) cube -> addCube surfaceArea allPoints cube) (Array.empty, Array.empty)


let runTestCases () = 
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

    let cubes = 
        [|
            (2,2,2);
            (1,2,2);
            (3,2,2);
            (2,1,2);
            (2,3,2);
            (2,2,1);
            (2,2,3);
            (2,2,4);
            (2,2,6);
            (1,2,5);
            (3,2,5);
            (2,1,5);
            (2,3,5);
        |]
    let result = run cubes |> fst
    if result.Length <> 64 then printfn $"Test case 3 Wrong answer {result.Length}" else printfn "Test case 3 OK"

    let cubes = Day18_input.input
    let result = run cubes |> fst
    if result.Length <> 3550 then printfn $"Test case 4 Wrong answer {result.Length}" else printfn "Test case 4 OK"