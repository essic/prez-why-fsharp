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

            ActivityCreationSuccess
                { IdActivity = None
                  Name = name
                  Tags = tags |> Array.ofSeq
                  Description = description
                  CreatedAt = operationTime
                  ModifiedAt = None }
