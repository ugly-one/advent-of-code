open System

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let getIndexes (line: seq<char>) =
    line |> Seq.indexed |> Seq.fold (fun (leftIndex, rightIndex) (index, item) ->
        let isDigit = Char.IsDigit(item)
        if isDigit && leftIndex = -1 then
            (index, index)
        elif isDigit then
            (leftIndex, index)
        else
            (leftIndex, rightIndex)
    ) (-1, -1)

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

let getSolution (line: string) =
    line |> getIndexes |> getDigitAt (line |> Seq.toList)
    
let getSolution2 (line: string) =
  line |> getNumbers |> toNumber
    
    
let lines = readLines "input.txt"
    
let sum = lines
                 |> Array.indexed
                 |> Array.map (fun (index, item) ->
                     // printfn "%A" (index, item)
                     (getSolution item))
                 |> Array.sum
                 

"pqr3stu8vwx" |> getSolution |> printfn "%A"
"a1b2c3d4e5f" |> getSolution |> printfn "%A"
"1abc2" |> getSolution |> printfn "%A"
"treb7uchet" |> getSolution |> printfn "%A"

printfn "%A" sum

"two1nine" |> getSolution2 |> printfn "%A"

let sum2 = lines
                 |> Array.indexed
                 |> Array.map (fun (index, item) ->
                     // printfn "%A" (index, item)
                     (getSolution2 item))
                 |> Array.sum
printfn "%A" sum2