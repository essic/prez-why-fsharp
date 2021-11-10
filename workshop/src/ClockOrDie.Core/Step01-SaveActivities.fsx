#load @"../../.paket/load/net6.0/Core/core.group.fsx"
#load @"Library.fs"
#load @"Domain.fs"

open System
open ClockOrDie.Core
open ClockOrDie.Core.Domain
open ClockOrDie.Core.Domain.Services

let ``Should say greetings`` () =
    Say.hello() = "Hello world !"

let ``Should create new activity when none exists`` () =
    match saveActivity Set.empty "fake name" "description !" ["tag1"; "tag2"; "tag3"] with
    | Create activity ->
        activity.IdActivity = None &&
        activity.Name = "fake name" &&
        activity.Description = "description !" &&
        activity.Tags = [| "tag1"; "tag2"; "tag3" |]
    | r ->  printfn "%A" r
            false

// run expresion below for test
//``Should create new activity when none exists`` () = true


let ``Should update existing activity`` () =
    let activityName = "Whatever"
    let existingActivities =
        { IdActivity = Some 12
          Name = activityName
          Description = "Whatever"
          Tags = [|"I"; "love"; "FSharp"|]
          CreatedAt = DateTime(2020,01,01)
          ModifiedAt = None } |> Set.singleton
    match saveActivity existingActivities activityName "New description" Seq.empty with
    | Update item ->
        item.Description = "New description" &&
        item.Name = "Whatever" &&
        item.Tags = Array.empty &&
        item.CreatedAt = DateTime(2020,01,01) &&
        item.ModifiedAt.IsSome
    | r -> printfn "%A" r
           false

// ``Should update existing activity`` () = true

let ``Should update existing activity regardless of name case`` () =
    let activityName = "Whatever"
    let existingActivities =
        { IdActivity = Some 12
          Name = activityName
          Description = "Whatever"
          Tags = [|"I"; "love"; "FSharp"|]
          CreatedAt = DateTime(2020,01,01)
          ModifiedAt = None } |> Set.singleton
    match saveActivity existingActivities "WHATEVER" "New description" Seq.empty with
    | Update item ->
        item.Description = "New description" &&
        item.Name = "Whatever" &&
        item.Tags = Array.empty &&
        item.CreatedAt = DateTime(2020,01,01) &&
        item.ModifiedAt.IsSome
    | r -> printfn "%A" r
           false

// ``Should update existing activity regardless of name case`` () = true

let ``Should fail when activity name is null or empty`` () =
    let res = [null; ""; "   "] |> List.map (fun name -> saveActivity Set.empty name "description!" ["tag1; tag2; tag3"])

    match res with
    | [Err [ActivityNameCannotBeNullOrEmpty]; Err [ActivityNameCannotBeNullOrEmpty]; Err [ActivityNameCannotBeNullOrEmpty]] -> true
    | r ->  printfn "%A" r
            false

// ``Should fail when activity name is null or empty`` () = true

let ``Should fail when description is null or empty`` () =
    let res = [null; ""; "   "] |> List.map (fun desc -> saveActivity Set.empty "Whatever" desc ["tag1; tag2; tag3"])

    match res with
    | [Err [DescriptionCannotBeNullOrEmpty]; Err [DescriptionCannotBeNullOrEmpty]; Err [DescriptionCannotBeNullOrEmpty]] -> true
    | r ->  printfn "%A" r
            false

// ``Should fail when description is null or empty`` () = true

let ``Should fail when duplicated tags are present`` () =
    match saveActivity Set.empty "Whatever" "Description!" ["tag1; tag1; tag2"; "tag3"; "tag2"] with
    | Err [DuplicatedTagsDetected] -> true
    | r -> printfn "%A" r
           false

// ``Should fail when duplicated tags are present`` () = true
