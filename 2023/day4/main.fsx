open System.Collections.Generic

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

let printSeq seq =
    seq |> Seq.map (fun item -> printfn "%A" item)
    |> Seq.length

let printDic (dic: IDictionary<int,int>) =
    dic |> Seq.map (fun item -> printfn "%A -> %A" item.Key item.Value)
    |> Seq.length

let bla2 map sequence (cardId, newCards)  =
    let toAdd = Seq.skip (cardId + 1) map |> Seq.take newCards
    Seq.append sequence toAdd

let bla3 map sequence (cardId, newCards)  =
    let toAdd = Seq.skip (cardId + 1) map |> Seq.take newCards
    Seq.append sequence toAdd

let rec bla (cardsToProcess: seq<int * int>) (result: int) (map: seq<int * int>)=
    
    match cardsToProcess with
    | sequence when Seq.isEmpty sequence -> result
    | a ->
        let newScore = result + (a |> Seq.length)
        // printfn "new Score %A" newScore
        let newCardsToProcess = a |> Seq.fold (bla2 map) Seq.empty
        // printfn "new cards to process: "
        // printSeq newCardsToProcess
        bla newCardsToProcess newScore map
    
let blaaa (cardsToProcess: seq<int * int>) (map: seq<int * int>)=
    
    let mutable hola = cardsToProcess
    let mutable score = hola |> Seq.length;
    while Seq.length hola > 0 do
        
        printSeq hola
        let values = hola |> Seq.map (fun (a,b) -> b)
        let additionalScore = values |> Seq.sum
        values |> Seq.length |> printfn " amount of new cards to process %A" 
        score <- score + additionalScore
        let newCardsToProcess = hola |> Seq.fold (bla3 map) Seq.empty
        hola <- newCardsToProcess
        
    score

let solve2 (lines: string seq) =
    let cardToWinningNumbersCount =
        lines
        |> Seq.indexed
        |> Seq.map (fun (index,line) -> (index, line |> getMyWinningNumbersCount))
    
    let result = blaaa cardToWinningNumbersCount cardToWinningNumbersCount
    printfn "%A" result
  
part1Example |> solve2

// readLines "input.txt" |> solve2 |> printfn "%A"