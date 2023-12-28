#load "../general.fsx"

open System.Collections.Generic

let lines = General.readLines "testInput.txt"

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
    
let getValue (hand: Hand) =
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
    
let lines2 = lines |> Seq.map parseLine
lines2
|> Seq.map (fun line -> {Line = line; Strength = (getValue line.Hand)})
|> Seq.sortBy (fun l -> l.Strength)
|> Seq.map (printfn "%A")
|> Seq.length
// group cards by strength and give ranks within each group