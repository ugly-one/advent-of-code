let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

type Color = Red | Green | Blue
type Grab = Color * int
type Round = Grab list
type Game = Round list

let parseGrab (grab: string) =
    let grab = grab.Trim()
    let split = grab.Split(' ')
    let color = match split.[1].Trim() with
                | "red" -> Red
                | "green" -> Green
                | "blue" -> Blue
                | _ -> failwith ("Invalid color" + split.[1].Trim())
    let count = int (split.[0].Trim())
    (color, count)
    
let parseRound (round: string) : Round =
    let grabs = round.Split(',')
    grabs |> Array.map parseGrab |> Array.toList
    
let parseGame (game: string) : Game =
    let rounds = game.Split(';')
    rounds |> Array.map parseRound |> Array.toList
    
let parseLine (line: string) : Game = 
    let split = line.Split(':')
    let game = split.[1].Trim()
    parseGame game
    
let validateRound red_limit green_limit blue_limit round =
    let validateGrab (color, count) =
        match color with
        | Red -> count <= red_limit
        | Green -> count <= green_limit
        | Blue -> count <= blue_limit
    round |> List.fold (fun result grab ->
        result && (validateGrab grab)) true
    
let validate red_limit green_limit blue_limit (game: Game) =
    game |> List.fold (fun result round ->
        result && (validateRound red_limit green_limit blue_limit round)) true
    
"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" |> parseLine |> validate 12 13 14 |> printfn "%A"
let lines = readLines "input.txt"

let result = lines
            |> Array.indexed
            |> Array.filter (fun (index, line) ->
                let game = parseLine line
                let valid = validate 12 13 14 game
                valid)
            |> Array.map (fun (index, lines) -> index + 1)
            |> Array.sum

if result <> 2810 then
    failwith ("Wrong result: " + result.ToString())
else
    printfn "OK"

let getMaxInGrab (grab: Grab) (red, green, blue) =
    match grab with
    | (Red, count) -> (max red count, green, blue)
    | (Green, count) -> (red, max green count, blue)
    | (Blue, count) -> (red, green, max blue count)

let getMaxInRound (round: Round) (red, green, blue) =
    round |> List.fold (fun state grab ->
        getMaxInGrab grab state) (red, green, blue)
    
let calculateMin (game: Game) =
    let (red, green, blue) = game
                                |> List.fold (fun state round -> getMaxInRound round state) (0,0,0)
    red * green * blue

"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" |> parseLine |> calculateMin |> printfn "%A"

let result2 = lines
            |> Array.map (fun (line) ->
                let game = parseLine line
                calculateMin game)
            |> Array.sum
            
printfn "%A" result2

if result <> 69110 then
    failwith ("Wrong result: " + result.ToString())
else
    printfn "OK"
