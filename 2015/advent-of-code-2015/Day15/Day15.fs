module Day15

open System.Collections.Generic
open System.Text.RegularExpressions

type Ingredient = {
    Name: string
    Properties: int[]
}
let run () =
    let input = [|
        "Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8"
        "Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"
    |]

    let parse line =
        let _match = Regex.Match(line, "([A-Za-z]+): capacity ([-0-9]+), durability ([-0-9]+), flavor ([-0-9]+), texture ([-0-9]+), calories ([-0-9]+)")
        let properties = [|
            _match.Groups[2] |> string |> int
            _match.Groups[3] |> string |> int
            _match.Groups[4] |> string |> int
            _match.Groups[5] |> string |> int
            _match.Groups[6] |> string |> int
        |]
        {Name = _match.Groups[1] |> string; Properties = properties }
        
    let ingredients = InputReader.readLines "Day15/input.txt" |> Array.map parse
    
    let calculateScore (recipe: (Ingredient * int)[]) =
        let makeNonNegative number =
            if number < 0 then 0 else number
        let capacity = recipe |> Array.fold (fun acc (ing, spoons) -> acc + ing.Properties[0] * spoons) 0 |> makeNonNegative
        let durability = recipe |> Array.fold (fun acc (ing, spoons) -> acc + ing.Properties[1] * spoons) 0 |> makeNonNegative
        let flavor = recipe |> Array.fold (fun acc (ing, spoons) -> acc + ing.Properties[2] * spoons) 0 |> makeNonNegative
        let texture = recipe |> Array.fold (fun acc (ing, spoons) -> acc + ing.Properties[3] * spoons) 0 |> makeNonNegative
        let result = capacity * durability * flavor * texture
        result    
    
    let calculateCalories (recipe: (Ingredient * int)[]) =
        recipe |> Array.fold (fun acc (ing, spoons) -> acc + ing.Properties[4] * spoons) 0
        
    let print (recipe: (Ingredient * int)[]) =
        for (ingredient, spoons) in recipe do
            printf "%s %d, " ingredient.Name spoons
        printfn ""
        
    let rec getScore (availableIngredients: Ingredient[]) (usedIngredients: (Ingredient * int)[]) (availableSpoons: int) (availableCalories: int) (bestScore: option<int>) =
        if availableIngredients.Length = 0 then
            let score = usedIngredients |> calculateScore
            let calories = usedIngredients |> calculateCalories
            if calories = 500 then 
            // print usedIngredients
                match bestScore with
                | None -> Some score
                | Some bestScore -> if score > bestScore then Some score else Some bestScore
            else None
        else
            let mutable bestScore = bestScore
            for ingredient in availableIngredients do
                let remainingIngredients = availableIngredients |> Array.filter (fun i -> i <> ingredient)
                let minimumNumberOfSpoons = if remainingIngredients.Length = 0 then availableSpoons else 0
                for spoons in minimumNumberOfSpoons .. availableSpoons do
                    let usedIngredients = usedIngredients |> Array.append [| (ingredient, spoons)  |]
                    let availableSpoons = availableSpoons - spoons
                    let calories = calculateCalories usedIngredients
                    let score = getScore remainingIngredients usedIngredients availableSpoons availableCalories bestScore
                    match (bestScore, score) with
                    | None, None -> ()
                    | Some bestScore, None -> ()
                    | None, Some score -> bestScore <- Some score
                    | Some bestScore_, Some score -> if score > bestScore_ then bestScore <- Some score else ()
            bestScore
    
    getScore ingredients Array.empty 100 500 None |> (fun score -> printfn $"%d{score.Value}")
