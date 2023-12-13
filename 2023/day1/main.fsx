open System

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let lines = readLines $"{__SOURCE_DIRECTORY__}/input.txt"
    
let getSolution (line: string) =
    let digits = line |> Seq.filter Char.IsDigit
    let first = digits |> Seq.head
    let last = digits |> Seq.rev |> Seq.head
    $"{first}{last}" |> int
    
let part1Result =
    lines
    |> Array.map getSolution
    |> Array.sum
                 
printfn "%A" part1Result
if part1Result <> 54605 then failwith "wrong answer" else printfn "%A" "part1 ok"

let getNumbers (line: string) =
    line |> Seq.indexed |> Seq.fold (fun (leftNumber, rightNumber) (currentIndex, currentItem) ->
        let isDigit = Char.IsDigit(currentItem)
        if (isDigit) then
            let digit = currentItem |> string |> int
            if leftNumber = -1 then
                (digit, digit)
            else
                (leftNumber, digit)
        else
            let number = 
                if currentIndex = 2 then
                    if line[(currentIndex-2)..currentIndex] = "two" then 2
                    elif line[(currentIndex-2)..currentIndex] = "one" then 1
                    elif line[(currentIndex-2)..currentIndex] = "six" then 6
                    else -1
                elif currentIndex = 3 then
                    if line[(currentIndex-2)..currentIndex] = "two" then 2
                    elif line[(currentIndex-2)..currentIndex] = "one" then 1
                    elif line[(currentIndex-2)..currentIndex] = "six" then 6
                    elif line[(currentIndex-3)..currentIndex] = "four" then 4
                    elif line[(currentIndex-3)..currentIndex] = "five" then 5
                    elif line[(currentIndex-3)..currentIndex] = "nine" then 9
                    else -1
                elif currentIndex >= 4 then
                    if line[(currentIndex-2)..currentIndex] = "two" then 2
                    elif line[(currentIndex-2)..currentIndex] = "one" then 1
                    elif line[(currentIndex-2)..currentIndex] = "six" then 6
                    elif line[(currentIndex-3)..currentIndex] = "four" then 4
                    elif line[(currentIndex-3)..currentIndex] = "five" then 5
                    elif line[(currentIndex-3)..currentIndex] = "nine" then 9
                    elif line[(currentIndex-4)..currentIndex] = "three" then 3
                    elif line[(currentIndex-4)..currentIndex] = "seven" then 7
                    elif line[(currentIndex-4)..currentIndex] = "eight" then 8
                    else -1
                else -1
            if number <> -1 then
                if (leftNumber = -1) then
                    (number, number)
                else
                    (leftNumber, number)
            else
                (leftNumber, rightNumber)
    ) (-1, -1)

let toNumber (n1, n2) = 
    n1 * 10 + n2

let getDigitAt (line: List<char>) (index1, index2) =
    let n1 = line |> List.item index1 |> string |> int
    let n2 = line |> List.item index2 |> string |> int
    toNumber (n1, n2)
    
let getSolution2 (line: string) =
  line |> getNumbers |> toNumber
    
let part2Result = lines
                 |> Array.indexed
                 |> Array.map (fun (index, item) -> (getSolution2 item))
                 |> Array.sum
                 
printfn "%A" part2Result
if part2Result <> 55429 then failwith "wrong answer" else "part2 ok" |> printfn "%A" 
