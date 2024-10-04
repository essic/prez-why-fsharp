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
            let cleanTags = tags |> Seq.map (_.Trim()) |> Seq.toArray
            
            let validateAndCleanNameAndDescription () =
                let isNameValid = String.IsNullOrWhiteSpace(name) |> not
                let isDescriptionValid = String.IsNullOrWhiteSpace(description) |> not
                
                if isNameValid && isDescriptionValid then
                    Ok (name.Trim(), description.Trim())
                else
                    [ if isNameValid then [] else [ActivityNameCannotBeNullOrEmpty]
                      if isDescriptionValid then [] else [DescriptionCannotBeNullOrEmpty] ]
                    |> List.concat |> ActivityCreationOrUpdateFailure |> Error
                    
            match validateAndCleanNameAndDescription() with
            | Error err -> err
            | Ok (cleanName,cleanDescription) ->
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
