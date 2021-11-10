namespace ClockOrDie.Core

module Shell =        
    open Domain
    open Services
    open Effects

    let saveActivity (db:IHandleDatabaseOperations) name description tags =
        task {
            let! activitiesResult = db.GetAllActivities() 
            match activitiesResult with
            | Ok existingActivities ->
                match createOrUpdateActivity (existingActivities |> Set.ofSeq) name description tags with
                | ActivityErr e -> return (e |> Seq.map (sprintf "%A") |> BusinessErr |> Error)
                | CreateActivity newActivity ->return! db.CreateActivity newActivity 
                | UpdateActivity existingActivity -> return! db.UpdateActivity existingActivity 
            | Error _ -> return ( "An error occured in database" |> Seq.singleton |> TechnicalErr |> Error)
        }

