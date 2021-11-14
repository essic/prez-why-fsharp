namespace ClockOrDie.Core

module Domain =
    open System
    
    type AppError = 
        | BusinessErr of string seq
        | TechnicalErr of string seq

    type [<Struct>] Activity = 
        { IdActivity: int option
          Name: string
          Tags: string array
          Description: string
          CreatedAt: DateTime
          ModifiedAt: DateTime option }

    type ActivityOperationsError =
    | ActivityNameCannotBeNullOrEmpty
    | DescriptionCannotBeNullOrEmpty
    | DuplicatedTagsDetected
    | TagsCannotHaveNullOrEmptyValues

    type ActivityOperationsResult =
    | CreateActivity of Activity
    | UpdateActivity of Activity
    | ActivityErr of ActivityOperationsError list

    module Services =

        let private trim (s:string) = s.Trim();
        let private trimAll = Seq.map trim
        
        let createOrUpdateActivity 
            (existingActivities:Set<Activity>) 
            (name:string) 
            (description:string) 
            (tags:string seq) : ActivityOperationsResult =
                let name,description,tags = (name |> trim, description |> trim, tags |> trimAll )
                let isMatch activityName activity =
                    activity.Name.Equals(activityName,StringComparison.OrdinalIgnoreCase)

                match existingActivities |> Seq.tryFind (isMatch name) with
                | Some existingActivity ->
                    { existingActivity with
                        Description = description
                        Tags = tags |> Array.ofSeq 
                        ModifiedAt = Some DateTime.Now } |> UpdateActivity
                | None ->
                    { IdActivity = None 
                      Name = name
                      Description = description
                      Tags = tags |> Array.ofSeq
                      CreatedAt = DateTime.Now 
                      ModifiedAt = None } |> CreateActivity