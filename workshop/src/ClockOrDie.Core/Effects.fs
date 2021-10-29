namespace ClockOrDie.Core

module Effects =
    open Domain
    open System.Threading.Tasks

    type IHandleDatabaseOperations =
        abstract member GetAllActivities:
            Unit -> Task<Result<Activity seq, AppError>>

        abstract member CreateActivity :
            item:Activity -> Task<Result<Activity,AppError>>

        abstract member UpdateActivity :
            item:Activity -> Task<Result<Activity,AppError>>