#load "../general.fsx"

open System.Collections.Generic

type Hand = char array
type Line = {
    Hand: Hand
    Bid: int
    }

type LineWithStrength = {
    Line: Line
    Strength: int
}

let parseHand (string: string) : Hand =
    string |> Array.ofSeq

let parseLine (line: string) : Line =
    let chunks = line.Split " "
    let hand = chunks[0] |> parseHand
    let bid = chunks[1] |> int
    {Hand = hand; Bid = bid}
   
   
let calculateStrength_part2 (hand: Hand) =
    let dict = new Dictionary<char, int>()
    
    for char in hand do
        let mutable value = 0
        let exists = dict.TryGetValue(char, &value)
        if exists then
            dict[char] <- dict[char] + 1
        else
            dict[char] <- 1
            
    // check if we have jokers
    let mutable jokersCount = 0
    let haveJokers = dict.TryGetValue ('J', &jokersCount)
    if haveJokers then
        // find the biggest (other group) and add all jokers there
        let mutable biggestGroup = 'x'
        let mutable sizeOfBiggestGroup = -1
        for kv in dict do
            if kv.Key = 'J' then ()
            else
                if kv.Value > sizeOfBiggestGroup then
                    sizeOfBiggestGroup <- kv.Value
                    biggestGroup <- kv.Key
        
        if sizeOfBiggestGroup <> -1 then
            dict[biggestGroup] <- dict[biggestGroup] + jokersCount
            dict.Remove('J') |> ignore
        else ()
    else
        // no jokers - do nothing
        ()
                
    if dict.Count = 1 then 7
    elif dict.Count = 2 then
        if dict.Values |> Seq.contains 4 then 6
        else 5
    elif dict.Count = 3 then
        if dict.Values |> Seq.contains 3 then 4
        else 3
    elif dict.Count = 4 then 2
    else 1
    
    
let calculateStrength (hand: Hand) =
    let dict = new Dictionary<char, int>()
    
    for char in hand do
        let mutable value = 0
        let exists = dict.TryGetValue(char, &value)
        if exists then
            dict[char] <- dict[char] + 1
        else
            dict[char] <- 1
    
    if dict.Count = 1 then 7
    elif dict.Count = 2 then
        if dict.Values |> Seq.contains 4 then 6
        else 5
    elif dict.Count = 3 then
        if dict.Values |> Seq.contains 3 then 4
        else 3
    elif dict.Count = 4 then 2
    else 1

let groupByStrength (lines: seq<LineWithStrength>) =
    let result = new Dictionary<int, List<Line>>()
    for line in lines do
        let mutable value = new List<Line>()
        if result.TryGetValue(line.Strength, &value) then
            value.Add(line.Line)
        else
            let newList = List<Line>()
            newList.Add(line.Line)
            result.Add(line.Strength, newList)
    result 
    
 
let charToValue_part2 char =
    match char with
        | 'A' -> 14
        | 'K' -> 13
        | 'Q' -> 12
        | 'J' -> 1
        | 'T' -> 10
        | c -> (c |> int) - 48
    
let charToValue char =
    match char with
        | 'A' -> 14
        | 'K' -> 13
        | 'Q' -> 12
        | 'J' -> 11
        | 'T' -> 10
        | c -> (c |> int) - 48
    
let rec compareHands (hand1: Hand) (hand2: Hand) charToValue =
    if hand1.Length = 0 then 0
    else
        let valueHand1Char1 = charToValue hand1[0]
        let valueHand2Char1 = charToValue hand2[0]
        if valueHand1Char1 > valueHand2Char1 then 1
        elif valueHand1Char1 < valueHand2Char1 then -1
        else
            compareHands hand1[1..] hand2[1..] charToValue
    
let compareLines  charToValue (line1: Line) (line2: Line) =
    compareHands line1.Hand line2.Hand charToValue
    
let giveRank charToValue (hands: Line seq)  =
    hands |> Seq.sortWith (compareLines charToValue)

let giveRankForEachGroup charToValue (dict : seq<KeyValuePair<int, List<Line>>>) =
    let orderedGroups = dict |> Seq.map (fun kv -> kv.Value |> giveRank charToValue)
    let mutable rank = 1
    let result = new List<(Line*int)>()
    for group in orderedGroups do
        for line in group do
            result.Add((line, rank))
            rank <- rank + 1
    result
    
let calculateTotalWinnings (linesWithRank: List<Line * int>) =
    linesWithRank |> Seq.fold (fun sum (line, rank) -> sum + line.Bid * rank) 0
    
let lines =
    General.readLines "input.txt"
    |> Seq.map parseLine
    
// part 1
lines
    |> Seq.map (fun line -> {Line = line; Strength = (calculateStrength line.Hand)})
    |> groupByStrength
    |> Seq.sortBy (fun kv -> kv.Key)
    |> giveRankForEachGroup charToValue
    |> calculateTotalWinnings
    |> printfn "%A"
    

// part 2
lines
    |> Seq.map (fun line -> {Line = line; Strength = (calculateStrength_part2 line.Hand)})
    // |> General.print
    |> groupByStrength
    |> Seq.sortBy (fun kv -> kv.Key)
    |> giveRankForEachGroup charToValue_part2
    |> calculateTotalWinnings
    |> printfn "%A"

"T55J5"
|> Array.ofSeq
|> calculateStrength_part2
|> printfn "%A"