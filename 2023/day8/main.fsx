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
    
// network |> parseNodes |> moveUntilEnd instructions |> General.print

let checkNodesEndsWithZ (nodes: Node seq) =
    nodes |> Seq.fold (fun state node -> state && (node.Name[2] = 'Z')) true

let checkAnyNodesEndsWithZ (nodes: Node seq) =
    nodes |> Seq.fold (fun state node -> state || (node.Name[2] = 'Z')) false

let findStartNodes (network: Network) =
    let nodeNames = network.Keys |> Seq.filter (fun node -> node[2] = 'A')
    let nodes = new List<Node>()
    for nodeName in nodeNames do
        nodes.Add(network[nodeName])
    nodes
    
let moveUntilEnd_part2 (instructions: string) (network: Network) =
    let mutable currentNodes: Node seq = findStartNodes network
    let startNodes = currentNodes |> Array.ofSeq
    startNodes |> General.printSeq
    let mutable currentInstructionIndex = 0
    let mutable steps = 0L
    while not (checkNodesEndsWithZ currentNodes) do
        let instruction =
            if currentInstructionIndex = instructions.Length then
                currentInstructionIndex <- 1
                instructions[0]
            else
                currentInstructionIndex <- currentInstructionIndex + 1
                instructions[currentInstructionIndex - 1]
        currentNodes <- (Seq.map (fun node-> move instruction network node)) currentNodes |> Array.ofSeq
        
        // "######## currentNodes: " |> General.print
        // currentNodes |> General.printSeq
        // "######## currentNodes: " |> General.print
        steps <- steps + 1L
        currentNodes |> Seq.mapi (fun index node ->
                if node.Name[2] = 'Z' then printfn "node (%A) reached (%A) after %A steps " startNodes[index] node (steps) else ()
                if Seq.contains node startNodes then printfn "node (%A) reached itself after %A steps " startNodes[index] (steps) else ()
            ) |> Seq.length
        
        // if steps % 1000L = 0 then printfn "%A" steps else ()
    steps

network |> parseNodes |> moveUntilEnd_part2 instructions |> General.print

// node    ({ Name = "HMA" Left = "TVC" Right = "RBX" })
// reached ({ Name = "QLZ" Left = "RBX" Right = "TVC" })
// after 13771 steps (47 rounds)

// node    ({ Name = "CXA" Left = "XLT" Right = "VRF" })
// reached ({ Name = "NVZ" Left = "VRF" Right = "XLT" })
// after 17287 steps (59 rounds)

// node    ({ Name = "NPA" Left = "TPC" Right = "SNK" })
// reached ({ Name = "DHZ" Left = "SNK" Right = "TPC" })
// after 19631 steps (67 rounds)

// node    ({ Name = "VHA" Left = "KLF" Right = "HXJ" })
// reached ({ Name = "KQZ" Left = "HXJ" Right = "KLF" })
// after 20803 steps (71 rounds)

// node    ({ Name = "GQA" Left = "NQS" Right = "CNH" })
// reached ({ Name = "BJZ" Left = "CNH" Right = "NQS" })
// after 21389 steps (73 rounds)

// node    ({ Name = "AAA" Left = "XBB" Right = "FGX" })
// reached ({ Name = "ZZZ" Left = "FGX" Right = "XBB" })
// after 23147 steps (79 rounds)

// 1 round = 293 instructions = 1 line of instructions
// all those numbers of steps are divisible by 293

let rounds = 47L * 59L * 67L * 71L * 73L * 79L 
let steps = rounds * 293L
steps |> printfn "%A"