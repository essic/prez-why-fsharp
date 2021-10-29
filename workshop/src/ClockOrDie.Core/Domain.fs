namespace ClockOrDie.Core

module Domain =
    
    type AppError = 
        | BusinessErr of string seq
        | TechnicalErr of string seq

    type [<Struct>] Activity = 
        { IdActivity: int option
          Name: string
          Tags: string array
          Description: string }

    type ActivityOperationsError =
    | ActivityNameCannotBeNullOrEmpty
    | DescriptionCannotBeNullOrEmpty
    | DuplicatedTagsDetected

    type ActivityOperationsResult =
    | Create of Activity
    | Update of Activity
    | Err of ActivityOperationsError list

    module Services =

        let saveActivity 
            (existingActivities:Set<Activity>) 
            (name:string) 
            (description:string) 
            (tags:string seq) : ActivityOperationsResult =
                notYetImplementedFailure()