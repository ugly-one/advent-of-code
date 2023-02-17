module Day4

open System.Security.Cryptography
open System.Text

let secret = "ckczppom"

let calculate (secret: string) number = 
    let input =  secret + number
    let md5 = MD5.Create ()
    let hash = md5.ComputeHash ((System.Text.Encoding.ASCII.GetBytes input))
    let sb = new StringBuilder();
    for i in 0 .. 1 .. hash.Length - 1 do
        sb.Append (hash[i].ToString("x2")) |> ignore
    let result = sb.ToString ()
    let first5Characters = result[0..5]
    first5Characters |> Seq.fold (fun allZeros char -> allZeros && (char = '0')) true


let run () = 
    for number in 1 .. 1 .. 99999999 do 
        if (number |> string) |> calculate secret then failwithf "%d" number else ()