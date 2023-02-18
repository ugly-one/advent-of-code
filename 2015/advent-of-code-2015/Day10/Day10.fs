module Day10

let lookAndSay (input: string) = 
   let mutable result = ""
   let mutable previousChar = input[0]
   let mutable currentGroupCount = 1
   let mutable index = 1
   while index < input.Length do 
      let char = input[index]
      if char = previousChar then 
         currentGroupCount <- currentGroupCount + 1
      else 
         result <- result + (currentGroupCount |> string)
         result <- result + (previousChar |> string)
         currentGroupCount <- 1
         previousChar <- char
      index <- index + 1
   result <- result + (currentGroupCount |> string)
   result <- result + (previousChar |> string)
   result
let run () =
   "1" |> lookAndSay |> printfn "%s"
   "111221" |> lookAndSay |> printfn "%s"

   let mutable input = "1113122113"
   for i in 1 .. 40 do 
      input <- lookAndSay input
      printfn "%d done" i
   input.Length |> printfn "%d"
