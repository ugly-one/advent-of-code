module Day7

open System.Text.RegularExpressions

type Argument = 
   | Number of int32
   | Wire of string

type TwoArgOperation = {
   Argument1: Argument
   Argument2: Argument
   Operation: string
}

type Operation = 
   | NotOperation of string
   | TwoArgOperation of TwoArgOperation
   | AssignOperaton of Argument

type Instruction = {
   ResultWire: string
   Operation: Operation
}

let readLines file = System.IO.File.ReadLines(file) |> Array.ofSeq

let and_ (a:int32) (b: int32) = 
   a &&& b

let or_ (a:int32) (b: int32) = 
   a ||| b

let left_ (a:int32) (b: int32) = 
   a <<< b

let right_ (a:int32) (b: int32) = 
   a >>> b

let not_ (a:int32) = 
   ~~~ a

let parseLine (line: string)  = 
   let twoArgumentsOperation = Regex.Match(line, "([a-z0-9]+) ([A-Z]+) ([a-z0-9]+) -> ([a-z]+)")
   let oneArgumentsOperation = Regex.Match(line, "NOT ([a-z]+) -> ([a-z]+)")
   let assignOperation = Regex.Match(line, "([a-z0-9]+) -> ([a-z]+)")

   let parseArgument (arg: string) = 
      let (isNumber, number) = System.Int32.TryParse arg
      if isNumber then Number number else Wire arg

   if twoArgumentsOperation.Success then 

      let argument1 = twoArgumentsOperation.Groups[1] |> string
      let argument2 = twoArgumentsOperation.Groups[3] |> string
      let argument1 = parseArgument argument1
      let argument2 = parseArgument argument2
      let operation = twoArgumentsOperation.Groups[2] |> string
      let resultWire = twoArgumentsOperation.Groups[4] |> string
      let operation = TwoArgOperation { Argument1 = argument1; Argument2 = argument2; Operation = operation;}
      {ResultWire = resultWire; Operation = operation}
   elif oneArgumentsOperation.Success then
      let argument = oneArgumentsOperation.Groups[1] |> string
      let resultWire = oneArgumentsOperation.Groups[2] |> string
      {ResultWire = resultWire; Operation = NotOperation argument}
   elif assignOperation.Success then 
      let leftOperator = assignOperation.Groups[1] |> string
      let leftOperator = parseArgument leftOperator
      let resultWire = assignOperation.Groups[2] |> string
      {ResultWire = resultWire; Operation = AssignOperaton leftOperator}
   else 
      failwith "WHAT"
      
let rec getValue (wire: Instruction) (allInstructions: Map<string,Instruction>) =
   
   let (result, newAll) = 
      match wire.Operation with 
      |  AssignOperaton wire -> 
         match wire with 
         | Number n -> (n, allInstructions)
         | Wire wire -> 
            let instruction = allInstructions |> Map.find wire
            getValue instruction allInstructions
      | NotOperation operation -> 
         let instruction = allInstructions |> Map.find operation
         let (result, newAll) = getValue instruction allInstructions
         (not_ result, newAll)
      | TwoArgOperation operation -> 
         let (firstValue, newAll) = 
            match operation.Argument1 with
            | Number n -> (n, allInstructions)
            | Wire wire -> 
               let instruction = allInstructions |> Map.find wire
               let (result, newAll) = getValue instruction allInstructions
               (result, newAll)
         let (secondValue, newAll) = 
            match operation.Argument2 with
            | Number n -> (n, newAll)
            | Wire wire -> 
               let instruction = newAll |> Map.find wire
               getValue instruction newAll

         let r = 
            match operation.Operation with 
            | "AND" -> and_ firstValue secondValue
            | "OR" -> or_ firstValue secondValue
            | "RSHIFT" -> right_ firstValue secondValue 
            | "LSHIFT" -> left_ firstValue secondValue 
            | _ -> failwith "HE?!"
         (r, newAll)

   let change optionWire = 
      match optionWire with 
      | None -> None
      | Some wire -> Some {ResultWire = wire.ResultWire; Operation = AssignOperaton (Number result) }

   let newAll = newAll |> Map.change wire.ResultWire change
   (result, newAll)
   

let run () =
   let input = readLines "Day7/input.txt"

   let instructions = 
      input 
      |> Array.map (fun line -> 
                        let a = parseLine line
                        (a.ResultWire, a)) 
      |> Map.ofArray

   let aWire = instructions |> Map.find "a"

   
   let (result, newAll) = getValue aWire instructions
   printfn "%d" result
   result
