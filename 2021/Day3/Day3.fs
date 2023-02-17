module Day3
open Day3_input

let bitToDecimal bits = 
    Seq.foldBack (fun item (decimal, index) ->  (if item = 1 then decimal + pown 2 index else decimal), index + 1) bits (0, 0) |> fst

let invertBit array bit = 
    Array.append array (Array.create 1  (if bit = 1 then 0 else 1))
    
let flipBits bits = 
    let empty = array.Empty()
    Seq.fold invertBit empty bits

let run = 
    let dataSet = input
    let resultSize = Array2D.length2 dataSet
    let bitsPerColumns = Array2D.length1 dataSet
    let half = bitsPerColumns / 2

    let mostCommonBits = seq {
        for i in 0..(resultSize - 1) do
            let data = dataSet[*,i]
            let sum = Array.sum data
            if (sum > half) then 1 else 0
    }

    let gamma = bitToDecimal mostCommonBits
    let epsilon = flipBits mostCommonBits |> bitToDecimal
    gamma * epsilon


let rec filterRows (dataSet: int[,]) columnNumber check = 
    let rows = Array2D.length1 dataSet
    if rows = 1 then dataSet else 
        let half = float rows / 2.0
        let sumOfBitsInColumn = Array.sum dataSet[*, columnNumber] |> float
        let winningBit = if (check sumOfBitsInColumn half) then 1 else 0
        let newRows = seq {
            for i in 0..(rows - 1) do
                let row = dataSet[i, *]
                if (row[columnNumber] = winningBit) then row
        }
        let newDataSet = Seq.toArray newRows  |> toArray2D2
        filterRows newDataSet (columnNumber + 1) check

let run2 = 
    let dataSet = input
    let oxygen_generator_rating = filterRows dataSet 0 (fun a b -> a >= b)
    let b = oxygen_generator_rating[0,*] |> bitToDecimal
    let co2_scrubber_rating = filterRows dataSet 0 (fun a b -> a < b)
    let c = co2_scrubber_rating[0,*] |> bitToDecimal

    b * c