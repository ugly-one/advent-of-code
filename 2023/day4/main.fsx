let part1Example =
    """Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
    Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
    Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
    Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
    Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
    Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"""
        .Split("\n")
        |> Array.map (fun s -> s.Trim())
        |> List.ofSeq

let cleanUp (strings: string[]) =
    strings |> Array.filter (fun s -> s <> "") |> Seq.map (fun s -> s.Trim() |> int)
    
let tryAddToWinning (winningNumbers: seq<int>) (winningCount: int) (myNumber: int) =
    if Seq.contains myNumber winningNumbers then winningCount + 1 else winningCount 
    
let parseNumbers (line: string) =
    let numbers = line.Split(':')[1]
    let winningAndMyNumbers = numbers.Split('|')
    let winningNumbers = winningAndMyNumbers[0].Split(' ') |> cleanUp
    let myNumbers = winningAndMyNumbers[1].Split(' ') |> cleanUp
    let myWinningNumbersCount = myNumbers |> Seq.fold (tryAddToWinning winningNumbers) 0
    let power = myWinningNumbersCount - 1
    pown 2 power
    
let solve1 lines =
    lines |> Seq.map parseNumbers |> Seq.sum 
"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53" |> parseNumbers |> printfn "%A"
part1Example |> solve1 |> printfn "%A"
let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq
readLines "input.txt" |> solve1 |> printfn "%A"