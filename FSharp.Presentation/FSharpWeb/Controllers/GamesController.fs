namespace FSharpWeb.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Configuration
open System.Web.Mvc
open System.Web.Mvc.Ajax
open FSharpWeb.DataLayer

type GamesController() =
    inherit Controller()

    member internal this.IndexView (games : GameDAL.Game list) = this.View games

    member this.Index () = 
        async {
            let! games = GameDAL.getAllGamesAsync ()
            return games |> this.IndexView
        }
        |> Async.StartAsTask
        

