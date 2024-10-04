namespace ClockOrDie.Core

module Effects =
    open Domain
    open System.Threading.Tasks

    type IHandleDatabaseOperations =
        abstract member GetAllActivities:
            Unit -> Task<Result<Activity seq, ApplicationError>>

        abstract member CreateActivity :
            item:Activity -> Task<Result<Activity,ApplicationError>>

        abstract member UpdateActivity :
            item:Activity -> Task<Result<Activity,ApplicationError>>