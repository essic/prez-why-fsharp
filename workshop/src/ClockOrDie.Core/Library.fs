namespace ClockOrDie.Core

[<AutoOpen>]
module Utils =
    let notYetImplementedFailure () =
        failwith "Core app needs implementation, get the F# magic !"

module Say =
    let hello () : string =
        "Hello world !"