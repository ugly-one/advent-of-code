module Day4

open System.Text.RegularExpressions

type Number = {
    Value: int
    mutable Marked: bool
}

type Board = {
    numbers : Number[]
}

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let parseMoves input =
    let matches = Regex.Matches(input, "[0-9]+")
    seq { for _match in matches do yield _match.Value |> int } |> Array.ofSeq

let parseBoards (input: array<string>) = 
    seq {

        for line in 0 .. 6 .. input.Length do 
            let boardInput = input[line.. line+4]
            let board = boardInput |> Array.fold (fun board line -> Array.append board (parseMoves line)) [| |]
            let numbers = board |> Array.map (fun n -> {Value = n; Marked = false})
            yield {numbers = numbers}
    } |> Array.ofSeq


let mark board number =
    let mutable i = 0
    while i < board.numbers.Length do
        if (board.numbers[i].Value = number) then
            board.numbers[i].Marked <- true
            i <- board.numbers.Length // stop the loop
        else i <- i + 1
   
let checkRows board =
    let mutable isBingo = false
    for i in 0 .. 5 .. (Array.length board.numbers) - 1 do 
        let row = board.numbers[i..(i + 4)]
        let isRowMarked = row |> Array.fold(fun isBingo number -> number.Marked && isBingo) true
        isBingo <- isBingo || isRowMarked
    isBingo

let checkColumns board =
    let mutable isBingo = false
    let numbers = board.numbers
    for i in 0 .. 4 do 
        let column = [| numbers[i]; numbers[i+5]; numbers[i+10]; numbers[i+15]; numbers[i+20]|]
        let isColumnsMarked = column |> Array.fold(fun isBingo number -> number.Marked && isBingo) true
        isBingo <- isBingo || isColumnsMarked
    isBingo

let checkBoard board =
    let bingoInRows = checkRows board
    let bingoInColumns = checkColumns board
    if bingoInColumns || bingoInRows then true else false
    
let run_1 input = 

    let moves = input |> Array.head
    let moves = parseMoves moves
    let boards = parseBoards (input |> Array.skip 2)

    for move in moves do 
        let mutable boardIndex = 0
        for board in boards do 
            mark board move
            if checkBoard board then 
                let sum = board.numbers |> Array.fold (fun sum number -> if number.Marked then sum else sum + number.Value ) 0
                let score = sum * move
                failwithf "Board index: %d OnMove: %d. Sum: %d sum. Score: %d" boardIndex move sum score
            else 
                ()
            boardIndex <- boardIndex + 1

let run_2 input = 

    let moves = input |> Array.head
    let moves = parseMoves moves
    let boards = parseBoards (input |> Array.skip 2) |> List.ofArray
    let mutable finishedBoards = [ ]
    for move in moves do 
        let mutable boardIndex = 0
        for board in boards do 
            mark board move
            if checkBoard board then 
                if List.contains boardIndex finishedBoards then ()
                else 
                    finishedBoards <- List.append finishedBoards [boardIndex]
                if (List.length finishedBoards) = (List.length boards) then
                    let sum = board.numbers |> Array.fold (fun sum number -> if number.Marked then sum else sum + number.Value ) 0
                    let score = sum * move
                    failwithf "board number: %d sum %d. move %d. score %d" boardIndex sum move score
                else ()
            else 
                ()
            boardIndex <- boardIndex + 1

let run () =
    // let input = readLines "Day4/testInput.txt"
    // run_1 input
    let input = readLines "Day4/input.txt"
    // run_1 input
    run_2 input
