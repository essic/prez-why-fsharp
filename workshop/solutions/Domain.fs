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
            
            let validateAndCleanEntries () =
                let isNameValid = String.IsNullOrWhiteSpace(name) |> not
                let isDescriptionValid = String.IsNullOrWhiteSpace(description) |> not
                let areAllTagsValid =
                    tags |> Seq.forall (fun x -> String.IsNullOrWhiteSpace(x) |> not) = true
                let cleanTags = tags |> Seq.filter (fun x -> x <> null) |> Seq.map (_.Trim())                    
                let areThereNoDuplicatedTags =
                    if areAllTagsValid then
                        let sourceNb = tags |> Seq.length
                        let cleanTagsNb = (cleanTags |> Seq.map(_.ToLower()) |> Set.ofSeq).Count
                        sourceNb = cleanTagsNb
                    else true
                
                if isNameValid && isDescriptionValid && areAllTagsValid && areThereNoDuplicatedTags then 
                    Ok (name.Trim(), description.Trim(), cleanTags |> Array.ofSeq) 
                else
                    [ if isNameValid then [] else [ActivityNameCannotBeNullOrEmpty]
                      if isDescriptionValid then [] else [DescriptionCannotBeNullOrEmpty]
                      if areAllTagsValid then [] else [TagsCannotHaveNullOrEmptyValues]
                      if areThereNoDuplicatedTags then [] else [DuplicatedTagsDetected]]
                    |> List.concat |> ActivityCreationOrUpdateFailure |> Error
                    
            match validateAndCleanEntries() with
            | Error err -> err
            | Ok (cleanName,cleanDescription,cleanTags) ->
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
