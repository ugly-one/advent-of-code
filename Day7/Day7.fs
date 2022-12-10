module Day7
open System.Text.RegularExpressions

type File = {
    Name: string
    Size: int
}

type Dir = {
    Name: string
    mutable SubDirs: Dir[]
    mutable Files: File[]
    mutable Parent: option<Dir>
    mutable Size: int
}

let rec calcualateSizes (dir: Dir) = 
    let filesSize= Array.fold (fun state (file:File) -> file.Size + state) 0 dir.Files
    let dirsSizes = Array.fold (fun state dir -> state + (calcualateSizes dir)) 0 dir.SubDirs
    dir.Size <- filesSize + dirsSizes
    filesSize + dirsSizes

let rec findSmallDirs (dir: Dir) = 
    let mutable smallDirs = Array.filter (fun dir -> dir.Size < 100000) dir.SubDirs
    for dir in dir.SubDirs do 
        let smallSubDirs = findSmallDirs dir
        smallDirs <- Array.append smallDirs smallSubDirs
    smallDirs

let rec findbiggest size (dir: Dir) = 
    let mutable biggestDirs = Array.filter (fun dir -> dir.Size >= size) dir.SubDirs
    for dir in biggestDirs do 
        let biggestSubDirs = findbiggest size dir
        biggestDirs <- Array.append biggestSubDirs biggestDirs
    biggestDirs

let generateDisk (input: array<string>) = 
    let root = {Name = "/"; SubDirs = Array.empty; Files = Array.empty; Parent = None; Size = 0}
    let mutable currentDir = root
    for line in input do 
        if (line[0] = '$') then 
            match line with 
            | "$ cd /" -> currentDir <- root
            | "$ cd .." -> currentDir <- currentDir.Parent.Value
            | "$ ls" -> ()
            | _ -> 
                let dirMatch = Regex.Match(line, "\$ cd ([a-z]+)")
                let name = dirMatch.Groups[1].Value
                let dir = Array.find (fun d -> d.Name = name) currentDir.SubDirs
                currentDir <- dir
        else 
            let fileMatch = Regex.Match(line, "([0-9]+) ([a-z.]+)")
            if (fileMatch.Success) then 
                let file =  { Name = fileMatch.Groups[2].Value; Size = fileMatch.Groups[1].Value |> int}
                let newFiles = Array.append currentDir.Files [|file|]
                currentDir.Files <- newFiles
            else 
                let dirMatch = Regex.Match(line, "dir (.*)")
                let dir = {Name = dirMatch.Groups[1].Value; SubDirs = Array.empty; Files = Array.empty; Parent = None; Size = 0}
                let newDirs = Array.append currentDir.SubDirs [|dir|]
                dir.Parent <- Some currentDir
                currentDir.SubDirs <- newDirs
    root 

let run1 input = 
    let root = generateDisk input
    calcualateSizes root |> ignore
    let smallDirs = findSmallDirs root
    Array.fold (fun state dir -> state + dir.Size) 0 smallDirs


let run2 input = 
    let root = generateDisk input
    calcualateSizes root |> ignore
    let sizeToDelete = root.Size - 40000000
    let biggest = findbiggest sizeToDelete root 
    let smallestOfTheBiggest = Array.fold (fun smallest dir -> if smallest.Size > dir.Size then dir else smallest) biggest[0] biggest[1..]
    smallestOfTheBiggest.Size
