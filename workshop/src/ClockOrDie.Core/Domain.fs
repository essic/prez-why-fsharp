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

        let createOrUpdateActivity 
            (existingActivities:Set<Activity>) 
            (name:string) 
            (description:string) 
            (tags:string seq) : ActivityOperationsResult =
                notYetImplementedFailure()