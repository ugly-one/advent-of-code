open System.Collections.Generic
#load "../general.fsx"

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
 
let parse (line: string) =
    let numbers = line.Split(':')[1]
    let winningAndMyNumbers = numbers.Split('|')
    let winningNumbers = winningAndMyNumbers[0].Split(' ') |> cleanUp
    let myNumbers = winningAndMyNumbers[1].Split(' ') |> cleanUp
    (winningNumbers, myNumbers)
    
let getMyWinningNumbers (line: string) =
    let (myNumbers, winningNumbers) = parse line
    let myWinningNumbers = myNumbers |> Seq.where (fun number -> Seq.contains number winningNumbers)
    myWinningNumbers
    
let getMyWinningNumbersCount (line: string) =
    let myWinningNumbers = getMyWinningNumbers line
    let myWinningNumbersCount = myWinningNumbers |> Seq.length
    myWinningNumbersCount
    
let calculatePoints count = 
    let power = count - 1
    pown 2 power
    
let solve1 lines =
    lines |> Seq.map (fun line -> line |> getMyWinningNumbersCount |> calculatePoints ) |> Seq.sum
    
// "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53" |> getMyWinningNumbersCount |> calculatePoints |> printfn "%A"
part1Example |> solve1 |> printfn "part 1 example: %A"
let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq
readLines "input.txt" |> solve1 |> printfn "part 1 main: %A"

type CardToProcess = {
    Card: string
    mutable Count: int
}

let increaseCounts (cards: CardToProcess list) cardCount value =

    let cardsToIncrease = List.take cardCount cards
    for cardToIncrease in cardsToIncrease do 
        cardToIncrease.Count <- cardToIncrease.Count + value
    ()

let solve2 lines = 

    let cardsToProcess = lines |> Seq.map (fun line -> {Card = line; Count = 1}) |> List.ofSeq
    let mutable score = 0
    let rec processCards (cards: CardToProcess list) =
    
        if cards |> Seq.length = 0 then ()
        else
            let firstCard = cards |> Seq.head
            let firstCardWinningNumbers = firstCard.Card |> getMyWinningNumbersCount
            score <- firstCard.Count + score
            let otherCards = List.skip 1 cards
            increaseCounts otherCards firstCardWinningNumbers firstCard.Count
            processCards otherCards

    processCards cardsToProcess
    printfn "%A" score

part1Example |> solve2
readLines "input.txt" |> solve2