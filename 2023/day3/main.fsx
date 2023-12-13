open System

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let lines = readLines "input.txt"

type Position = int * int
type Length = int
type Number = int * Position * Length
type Symbol = Position * Char

let fold (symbols: Symbol[], numbers: Number[], currentNumber: string, index: int, row: int) character =
    match character with
    | '.' ->
        if currentNumber.Length > 0 then
            let numberAsInt = currentNumber |> int
            let number = Number(numberAsInt, (index - currentNumber.Length, row), currentNumber.Length)
            (symbols, Array.append numbers [| number |], "", index+1, row)
        else
            (symbols, numbers, currentNumber, index+1, row)
    | digit when Char.IsDigit(digit) ->
        let newCurrentNumber = currentNumber + (character |> string)
        (symbols, numbers, newCurrentNumber, index+1, row)
    | symbol ->
        let newSymbol = Symbol((index, row), symbol)
    
        if currentNumber.Length > 0 then
            let numberAsInt = currentNumber |> int
            let number = Number(numberAsInt, (index - currentNumber.Length, row), currentNumber.Length)
            let newNumbers = Array.append numbers [| number |]
            (Array.append symbols [| newSymbol |], newNumbers, "", index+1, row)
        else
           (Array.append symbols [| newSymbol |], numbers, currentNumber, index+1, row)
        

let parseLine (line: string) (row: int) =
    
    let emptyNumbers : Number[] = [||]
    let emptySymbols : Symbol[] = [||]
    let (symbols, numbers, lastNumber, _, _) = line |> Seq.fold fold (emptySymbols, emptyNumbers, "", 0, row)
    
    if lastNumber.Length > 0
    then
        let valueAsInt = lastNumber |> int
        let number = Number(valueAsInt, (line.Length - lastNumber.Length, row), lastNumber.Length)
        ((Array.append numbers [| number |]), symbols)
    else
        (numbers, symbols)
   
let numbersPositions = lines |> Array.indexed |> Array.map (fun (index, line) -> parseLine line index)

let (allNumbers : Number[], allSymbols : Symbol[]) =
    numbersPositions
    |> (Array.fold (fun (allNumbers, allSymbols) (numbers, symbols) -> (Array.append allNumbers numbers), (Array.append allSymbols symbols)) ([||], [||]))
    
printfn ""
printfn ""

let bla2 (position: Position) (symbols: Position) =
    position = symbols

let bla (position: Position) (symbols: Symbol[]) =
    symbols |> Array.fold (fun state (symbolPos, _) -> (bla2 position symbolPos) || state) false
    
let getPositionsToCheck (number: Number) =
    let (value, startPos, length) = number
    let y = snd startPos
    let x = fst startPos
    let leftAndRight = [|
        (x - 1, y)
        (x + length, y)
    |]
    
    let positionsWithinLine = seq { for i in (x-1)..(x + length) -> i }
    
    let upAndDown =
        positionsWithinLine
        |> Seq.map (fun i -> [| (i, y-1); (i, y + 1 )  |] )
        |> Seq.fold (fun state item -> Array.append state item ) [|  |]
        
    Array.append leftAndRight upAndDown
    

let isNearSymbols (symbols: Symbol[]) (number: Number) =
    let positionsToCheck : Position[] = getPositionsToCheck number
    positionsToCheck |> Array.fold (fun state position -> (bla position symbols) || state) false

let isNear (symbol: Symbol) (number: Number) =
    let positionsToCheck : Position[] = getPositionsToCheck number
    let (symbolPosition, _) = symbol
    positionsToCheck |> Array.fold (fun state position -> (bla2 position symbolPosition) || state) false

let isGear (numbers: Number[]) (symbol: Symbol) =
    let adjacentNumbers = numbers |> Array.filter (isNear symbol)
    if adjacentNumbers |> Array.length = 2
    then
        let (value1, _, _) = adjacentNumbers[0]
        let (value2, _, _) = adjacentNumbers[1]
        (true, value1 * value2)
        else
            (false, 0)

allNumbers
|> Array.filter (isNearSymbols allSymbols)
|> Array.sumBy(fun (value, _, _) -> value)
|> printfn "%A"

let gearSymbols = allSymbols |> Array.filter (fun (pos, symbol) -> symbol = '*')
// gearSymbols |> printfn "%A"

let realSymbols  = gearSymbols |> Array.sumBy (
    fun symbol ->
        let (result, value) = isGear allNumbers symbol
        value)

realSymbols |> printfn "%A"