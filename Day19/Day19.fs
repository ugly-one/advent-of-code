module Day19
open System.Collections.Generic
open System.Diagnostics

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
type Blueprint = IDictionary<Resource, Map<Resource, int>>
type Action = 
    | Wait
    | BuildRobot of Resource

let print (storage: Storage) = 
    for kv in storage do 
        printfn $"{kv.Key}: {kv.Value}"

let initialResources = Map [ (Resource.Ore, 0); (Resource.Clay, 0); (Resource.Obsidian, 0); (Resource.Geode, 0)]
let initialRobotsColletion = Map [ (Robot.Ore, 1); (Robot.Clay, 0); (Robot.Obsidian, 0); (Robot.Geode, 0)]
let possibleActions = [ Wait; BuildRobot Ore; BuildRobot Clay; BuildRobot Obsidian; BuildRobot Geode]

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

let rec executeMinutes (resources: Resources) (robots: Robots) (blueprint: Blueprint) minute highestCount = 
    if minute = 19 then 
        if resources[Clay] > highestCount then resources[Clay] else highestCount
    else 
        let mutable newHighestCount = highestCount
        for action in possibleActions do 
            match action with 
            | Wait -> 
                let newResources = produceAllResources resources robots
                newHighestCount <- executeMinutes newResources robots blueprint (minute + 1) highestCount
            | BuildRobot robotType -> 
                let (success, newResources, newRobots) = tryBuildRobot resources robots blueprint robotType
                if success 
                then 
                    // produce resources without the new robot
                    let newResources = produceAllResources newResources robots
                    newHighestCount <- executeMinutes newResources newRobots blueprint (minute + 1) highestCount
                else 
                    ()
        newHighestCount

let run () = 
    let input = inputReader.readLines "Day19/testInput.txt" |> Array.ofSeq

    let blueprint : Blueprint = dict [ 
        (Robot.Ore,  Map [(Resource.Ore, 4); (Resource.Clay, 0); (Resource.Obsidian, 0); (Resource.Geode, 0)]);
        (Robot.Clay, Map [(Resource.Ore, 2); (Resource.Clay, 0); (Resource.Obsidian, 0); (Resource.Geode, 0)]);
        (Robot.Obsidian, Map [(Resource.Ore, 3); (Resource.Clay, 14); (Resource.Obsidian, 0); (Resource.Geode, 0)]);
        (Robot.Geode, Map [(Resource.Ore, 2); (Resource.Clay, 0); (Resource.Obsidian, 7); (Resource.Geode, 0)]);
    ]

    let resources = initialResources
    let robots = initialRobotsColletion

    let largestNumberOfGeodes = executeMinutes resources robots blueprint 1 0

    if largestNumberOfGeodes <> 9 then failwithf $"Wrong answer {largestNumberOfGeodes}"