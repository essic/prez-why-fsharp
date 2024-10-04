namespace ClockOrDie.Core

module Domain =
    open System

    type ApplicationError =
        | BusinessErr of string seq
        | TechnicalErr of string seq

    [<Struct>]
    type Activity =
        { IdActivity: int option
          Name: string
          Tags: string array
          Description: string
          CreatedAt: DateTime
          ModifiedAt: DateTime option }

    type ActivityServiceError =
        | ActivityNameCannotBeNullOrEmpty
        | DescriptionCannotBeNullOrEmpty
        | DuplicatedTagsDetected
        | TagsCannotHaveNullOrEmptyValues

    type ActivityOperationsResult =
        | ActivityCreationSuccess of Activity
        | ActivityUpdateSuccess of Activity
        | ActivityCreationOrUpdateFailure of ActivityServiceError list

    module Services =

        let createOrUpdateActivity
            (operationTime: DateTime)
            (existingActivities: Set<Activity>)
            (name: string)
            (description: string)
            (tags: string seq)
            : ActivityOperationsResult =
            let cleanDescription = description.Trim()
            let cleanTags = tags |> Seq.map (_.Trim()) |> Seq.toArray
            
            let validateAndCleanName str =
                if String.IsNullOrWhiteSpace(str) |> not then
                    Some <| str.Trim()
                else     
                    None
                    
            match validateAndCleanName name with
            | None -> ActivityCreationOrUpdateFailure [ActivityNameCannotBeNullOrEmpty]
            | Some cleanName ->
                match
                    existingActivities
                    |> Seq.map (fun x -> x.Name.ToLower(),x)
                    |> Map.ofSeq
                    |> Map.tryFind (cleanName.ToLower()) with
                | Some entry ->
                    { entry with Description = cleanDescription; Tags = cleanTags; ModifiedAt = Some operationTime }
                    |> ActivityUpdateSuccess
                | None ->
                    ActivityCreationSuccess
                        { IdActivity = None
                          Name = cleanName
                          Tags = cleanTags
                          Description = cleanDescription
                          CreatedAt = operationTime
                          ModifiedAt = None }
