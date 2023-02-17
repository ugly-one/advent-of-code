module Day19
open System.Collections.Generic
open System.Diagnostics
open System.Text.RegularExpressions

type Resource = 
    | Ore
    | Clay
    | Obsidian
    | Geode
type Cost = int * Resource 
type Robot = Resource
type Storage = Map<Resource, int>
type Resources = Storage
type Robots = Storage
type Blueprint = Map<Resource, Map<Resource, int>>
type Action = 
    | BuildRobot of Resource

let print (storage: Storage) = 
    for kv in storage do 
        printfn $"{kv.Key}: {kv.Value}"

let initialResources = Map [ (Resource.Ore, 0); (Resource.Clay, 0); (Resource.Obsidian, 0); (Resource.Geode, 0)]
let initialRobotsColletion = Map [ (Robot.Ore, 1); (Robot.Clay, 0); (Robot.Obsidian, 0); (Robot.Geode, 0)]
let possibleActions = [ BuildRobot Ore; BuildRobot Clay; BuildRobot Obsidian; BuildRobot Geode]

let produceAllResources (resources: Resources) (robots: Robots) : Resources = 
    robots |> Map.fold (fun resources robotType robotCount -> Map.add robotType (resources[robotType] + robotCount)  resources) resources

let deductCosts (resources: Resources) (blueprint: Blueprint) (robot: Robot) : Resources =
    let costs = blueprint[robot]
    costs |> Map.fold (fun resources costType cost -> Map.add costType (resources[costType] - cost) resources) resources

let tryBuildRobot (resources: Resources) (robots: Robots) (blueprint: Blueprint) (robot: Robot) : bool * Resources * Robots = 
    let resourcesAfterBuild = deductCosts resources blueprint robot
    let canBuildBeDone = resourcesAfterBuild |> Map.fold (fun canBeDone resourceType resourceAmount -> canBeDone && (resourceAmount >= 0)) true
    if canBuildBeDone then 
        let newRobots = robots |> Map.add robot (robots[robot] + 1)
        (true, resourcesAfterBuild, newRobots)
    else
        (false, resources, robots)

let rec executeMinutes (resources: Resources) (robots: Robots) (blueprint: Blueprint) (highestCosts: Storage) minute highestCount resourceToMaximixe endMinute = 
    if minute > endMinute then 
        let count = resources[resourceToMaximixe]
        if count > highestCount 
        then 
            count
        else highestCount
    else if minute = endMinute then 
        // in the last minute it makes sense only to produce resources
        let resources = produceAllResources resources robots 
        let count = resources[resourceToMaximixe]
        if count > highestCount then count else highestCount
    else
        let mutable newHighestCount = highestCount
        for action in possibleActions do 
            match action with 
            | BuildRobot robotType -> 
                // check if we have robots producing resources to build this robotType
                let costs = blueprint[robotType]
                let produceRequiredResources = costs |> Map.fold (fun makeSense resourceType cost -> if cost = 0 then makeSense else makeSense && (robots[resourceType] > 0)) true

                // check if we have already enough robots for the resource. We only need that many robots as the highest cost in the bluprint
                let highestCostInBluprint = highestCosts[robotType]
                let makeSenseTobuild = robots[robotType] < highestCostInBluprint || robotType = Geode // it always make sense to build Geode robot - sky is the limit 

                if produceRequiredResources |> not || makeSenseTobuild |> not then () else 
                    let mutable continueLooping = true
                    let mutable resources = resources
                    let mutable robots = robots
                    let mutable minutes = minute
                    while continueLooping do 
                        let (success, newResources, newRobots) = tryBuildRobot resources robots blueprint robotType
                        if (success) 
                        then 
                            resources <- produceAllResources newResources robots
                            robots <- newRobots
                            continueLooping <- false
                            let count = executeMinutes resources robots blueprint highestCosts (minutes + 1) highestCount resourceToMaximixe endMinute
                            if count > newHighestCount then newHighestCount <- count else ()
                        else 
                            resources <- produceAllResources resources robots
                            minutes <- minutes + 1
                            if minutes > endMinute 
                            then 
                                continueLooping <- false
                            else 
                                ()
                    
        newHighestCount

let getHighestCost (blueprint: Blueprint) robotType : int = 
    blueprint |> Map.fold (fun highest _ costs -> if costs[robotType] > highest then costs[robotType] else highest) 0

let execute input resourceToMaximize minutes = 
    let something = Regex.Match(input, "Blueprint ([0-9]+): Each ore robot costs ([0-9]+) ore. Each clay robot costs ([0-9]+) ore. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian.")
    let bluePrintNr = something.Groups[1].Value |> int
    let oreRobotCost = something.Groups[2].Value |> int
    let clayRobotCost = something.Groups[3].Value |> int
    let obsidianRobotOreCost = something.Groups[4].Value |> int
    let obsidianRobotClayCost = something.Groups[5].Value |> int
    let geodeRobotOreCost = something.Groups[6].Value |> int
    let geodeRobotObsidianCost = something.Groups[7].Value |> int

    let blueprint = Map [ 
        (Robot.Ore,  Map [(Resource.Ore, oreRobotCost); (Resource.Clay, 0); (Resource.Obsidian, 0); (Resource.Geode, 0)]);
        (Robot.Clay, Map [(Resource.Ore, clayRobotCost); (Resource.Clay, 0); (Resource.Obsidian, 0); (Resource.Geode, 0)]);
        (Robot.Obsidian, Map [(Resource.Ore, obsidianRobotOreCost); (Resource.Clay, obsidianRobotClayCost); (Resource.Obsidian, 0); (Resource.Geode, 0)]);
        (Robot.Geode, Map [(Resource.Ore, geodeRobotOreCost); (Resource.Clay, 0); (Resource.Obsidian, geodeRobotObsidianCost); (Resource.Geode, 0)]);
    ]

    let highestCosts = Map [ 
        (Robot.Ore, getHighestCost blueprint Robot.Ore);
        (Robot.Clay, getHighestCost blueprint Robot.Clay);
        (Robot.Obsidian, getHighestCost blueprint Robot.Obsidian);
        (Robot.Geode, getHighestCost blueprint Robot.Geode);
    ]
    
    let resources = initialResources
    let robots = initialRobotsColletion
    let stopWatch = new Stopwatch()
    stopWatch.Start()
    let largestNumber = executeMinutes resources robots blueprint highestCosts 1 0 resourceToMaximize minutes
    printfn "%d" stopWatch.ElapsedMilliseconds
    largestNumber

let run () = 
    let testInput = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian."
    let largestNumber = execute testInput Clay 5
    if largestNumber <> 2 then failwithf $"Wrong answer {largestNumber}"
    
    let largestNumber = execute testInput Clay 4
    if largestNumber <> 1 then failwithf $"Wrong answer {largestNumber}"
    
    let largestNumber = execute testInput Geode 24
    if largestNumber <> 9 then failwithf $"Wrong answer {largestNumber}"

    let testInput = "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian."
    let largestNumber = execute testInput Geode 24
    if largestNumber <> 12 then failwithf $"Wrong answer {largestNumber}"

    // part 1

    let input = inputReader.readLines "Day19/input.txt"

    let stopwatch = new Stopwatch()
    stopwatch.Start()

    let mutable result = 0
    let mutable index = 1
    for line in input do 
         let largestNumber = execute line Geode 24
         //printfn "%d %d" index largestNumber
         result <- result + index * largestNumber
         index <- index + 1
    

    printfn " Part 1: Result: %d. Time: %d" result stopwatch.ElapsedMilliseconds

    // part 2 
    let input = input |> Seq.take 3
    stopwatch.Restart()

    let mutable result = 1
    for line in input do 
         let largestNumber = execute line Geode 32
         printfn "%d" largestNumber
         result <- result * largestNumber
    
    printfn " Part 2: Result: %d. Time: %d" result stopwatch.ElapsedMilliseconds
