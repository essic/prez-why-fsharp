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
        
        let private runValidation (isInvalid:'T -> bool ) (failure:ActivityOperationsError) (target:'T) : ActivityOperationsError list =
            if target |> isInvalid then [failure] else []
        
        let private runDescriptionValidation s =
            runValidation String.IsNullOrWhiteSpace DescriptionCannotBeNullOrEmpty s
        
        let private runNameValidation s =
            runValidation String.IsNullOrWhiteSpace ActivityNameCannotBeNullOrEmpty s

        let private runTagsValidation s =
            s 
            |> Seq.collect (runValidation String.IsNullOrWhiteSpace TagsCannotHaveNullOrEmptyValues)
            |> List.ofSeq

        let createOrUpdateActivity 
            (existingActivities:Set<Activity>) 
            (name:string) 
            (description:string) 
            (tags:string seq) : ActivityOperationsResult =
                let allValidation =
                    [runNameValidation name
                     runDescriptionValidation description
                     runTagsValidation tags] |> List.concat
                match allValidation with
                | r when r |> List.isEmpty |> not -> ActivityErr r
                | _ ->
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