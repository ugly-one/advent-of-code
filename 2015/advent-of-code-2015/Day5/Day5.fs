module Day5

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq
let input = readLines "Day5/input.txt"

let checkRule1 (item:string) = 
    let mutable previousPreviousWindow = item[0..1]

    let mutable previousWindow = item[1..2]
    let mutable windows = Set.empty |> Set.add previousWindow |> Set.add previousPreviousWindow
    let mutable windowsAppearingTwice = Set.empty

    for i in 2 .. 1 .. (Seq.length item - 2) do
        let currentWindow = item[i..i+1]
        if Set.contains currentWindow windows && previousWindow <> currentWindow then
            windowsAppearingTwice <- Set.add currentWindow windowsAppearingTwice
        elif previousPreviousWindow = currentWindow then
            windowsAppearingTwice <- Set.add currentWindow windowsAppearingTwice
        
        else ()

        windows <- Set.add currentWindow windows
        previousPreviousWindow <- previousWindow
        previousWindow <- currentWindow

    if Set.count windowsAppearingTwice > 0 then true else false

let checkRule2 (item:string) = 
    let mutable result = false
    for i in 0 .. 1 .. (Seq.length item - 3) do
        let window = item[i..(i+2)]
        if window[0] = window[2] then result <- true else ()
    result

let isNice (item:string) = 
    checkRule1 item && checkRule2 item

let run () = 
    "qjhvhtzxzqqjkmpb" |> isNice |> printfn "%b"
    "xxyxx" |> isNice |> printfn "%b"
    "xxxxx" |> isNice |> printfn "%b"
    "aabcdefgaa" |> checkRule1 |> printfn "%b"
    "xuxu" |> checkRule1 |> printfn "%b"

    "abcdefeghi" |> checkRule2 |> printfn "%b"

    "aba" |> checkRule1 |> printfn "%b"
    "aaa" |> checkRule1 |> printfn "%b"


    "ieodomkazucvgmuy" |> isNice |> printfn "%b"
    "uurcxstgmygtbstg" |> isNice |> printfn "%b"
    input |> Array.map isNice |> Array.filter (fun r -> r) |> Array.length |> printf "%d"