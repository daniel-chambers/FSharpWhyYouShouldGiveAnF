namespace FSharpWeb.DataLayer

open Microsoft.WindowsAzure.Storage
open DigitallyCreated.FSharp.Azure.TableStorage

module GameDAL =
    type Game = 
        { Name: string
          Platform: string
          Developer : string
          HasMultiplayer: bool }
          
         interface IEntityIdentifiable with
            member g.GetIdentifier() = 
                { PartitionKey = g.Developer; RowKey = g.Name + "-" + g.Platform }

    let private cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount
    let private tableClient = cloudStorageAccount.CreateCloudTableClient()
    let private inGameTableAsBatchAsync = inTableAsBatchAsync tableClient "Games"
    let private fromGameTableAsync = fromTableAsync tableClient "Games"

    let getAllGamesAsync () =
        async {
            let! games = 
                Query.all<Game> 
                |> fromGameTableAsync 
            
            return games 
            |> Seq.map fst
            |> Seq.toList
        }

    let insertGamesAsync () =
        async {
            let games = [
                { Name = "Halo 5"; 
                  Platform = "Xbox One"; 
                  Developer = "343 Industries"; 
                  HasMultiplayer = true }
                { Name = "Halo 4"
                  Platform = "Xbox 360"
                  Developer = "343 Industries"
                  HasMultiplayer = true }
                { Name = "Half-Life 2"
                  Platform = "PC"
                  Developer = "Valve Software"
                  HasMultiplayer = false }
                { Name = "Portal"
                  Platform = "PC"
                  Developer = "Valve Software"
                  HasMultiplayer = false }
                { Name = "Portal 2"
                  Platform = "PC"
                  Developer = "Valve Software"
                  HasMultiplayer = true }
                { Name = "Tomb Raider"
                  Platform = "PC"
                  Developer = "Crystal Dynamics"
                  HasMultiplayer = true }
            ]

            do! games 
                |> Seq.map InsertOrReplace
                |> autobatch 
                |> Seq.map inGameTableAsBatchAsync
                |> Async.Parallel
                |> Async.Ignore
        }
        