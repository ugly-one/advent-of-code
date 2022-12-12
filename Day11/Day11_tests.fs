module Day11_tests

open inputReader

let run () = 
    let testInput = readLines "Day11/testInput.txt" |> Array.ofSeq
    let input = readLines "Day11/input.txt" |> Array.ofSeq

    let testResult = Day11.run1 testInput
    if testResult = 10605UL then printfn "DONE" else failwith $"wrong result {testResult}" 

    let result = Day11.run1 input
    if result = 57838UL then printfn "DONE" else failwith $"wrong result {result}" 

    let testPart2 rounds expected1 expected2 = 
        let testResult_sub2 = Day11.run2 testInput rounds
        if testResult_sub2 = expected1 * expected2 then printfn "DONE" else failwith $"wrong result for {rounds} rounds: {testResult_sub2}" 

    testPart2 20 103UL 99UL
    testPart2 1000 5204UL 5192UL
    testPart2 2000 10419UL 10391UL
    testPart2 3000 15638UL 15593UL
    testPart2 4000 20858UL 20797UL
    testPart2 8000 41728UL 41606UL
    testPart2 9000 46945UL 46807UL