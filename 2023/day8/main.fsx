#load "../general.fsx"

open System.Collections.Generic

let lines = General.readLines "input.txt"
let instructions = lines[0]
let network = lines[2..]

type Node =
    {
        Name: string
        Left: string
        Right: string
    }
    
let parseNode (line: string) =
    let chunks = line.Split('=')
    let nodeName = chunks[0].TrimEnd()
    let childNodes = chunks[1].TrimStart()
    let leftRight = childNodes.Split(',')
    let left = leftRight[0].TrimStart('(').Trim()
    let right = leftRight[1].TrimEnd(')').Trim()
    {Name = nodeName; Left = left; Right = right }
    
    
let parseNodes (lines: string array) =
    let network = new Dictionary<string, Node>()
    for line in lines do
        let node = line |> parseNode
        network.Add(node.Name, node)
    network
    
type Network = Dictionary<string, Node>

let move (instruction: char) (network: Network) (currentPosition: Node) =
    let newNodeName =
        match instruction with
        | 'L' -> currentPosition.Left
        | 'R' -> currentPosition.Right
        | _ -> failwithf "???"
    network[newNodeName]
    
let moveUntilEnd(instructions: string) (network: Network) =
    let mutable currentPosition = network["AAA"]
    let mutable currentInstructionIndex = 0
    let mutable steps = 0
    while currentPosition.Name <> "ZZZ" do
        let instruction =
            if currentInstructionIndex = instructions.Length then
                currentInstructionIndex <- 1
                instructions[0]
            else
                currentInstructionIndex <- currentInstructionIndex + 1
                instructions[currentInstructionIndex - 1]
        
        currentPosition <- move instruction network currentPosition
        steps <- steps + 1
        
    steps
    
network |> parseNodes |> moveUntilEnd instructions |> General.print