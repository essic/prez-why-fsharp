namespace ClockOrDie.Core

open System

module Shell =        
    open Domain
    open Services
    open Effects

    let saveActivity (db:IHandleDatabaseOperations) name description tags =
        task {
            let! activitiesResult = db.GetAllActivities() 
            match activitiesResult with
            | Ok existingActivities ->
                match createOrUpdateActivity DateTime.Now (existingActivities |> Set.ofSeq) name description tags with
                | ActivityCreationOrUpdateFailure e -> return (e |> Seq.map (sprintf "%A") |> BusinessErr |> Error)
                | ActivityCreationSuccess newActivity ->return! db.CreateActivity newActivity 
                | ActivityUpdateSuccess existingActivity -> return! db.UpdateActivity existingActivity 
            | Error _ -> return ( "An error occured in database" |> Seq.singleton |> TechnicalErr |> Error)
        }

