#load @"../../.paket/load/net6.0/Core/core.group.fsx"
#load @"Library.fs"
#load @"Domain.fs"

open System
open ClockOrDie.Core
open ClockOrDie.Core.Domain
open ClockOrDie.Core.Domain.Services

let ``Should say greetings`` () =
    Say.hello() = "Hello world !"

``Should say greetings``() = true

let ``Should create new activity when none exists`` () =
    match createOrUpdateActivity Set.empty "fake name" "description !" ["tag1"; "tag2"; "tag3"] with
    | CreateActivity activity ->
        activity.IdActivity = None &&
        activity.Name = "fake name" &&
        activity.Description = "description !" &&
        activity.Tags = [| "tag1"; "tag2"; "tag3" |] &&
        activity.CreatedAt < DateTime.Now &&
        activity.ModifiedAt = None
    | r ->  printfn $"%A{r}"
            false

// run expression below for test
``Should create new activity when none exists`` () = true


let ``Should create new activity when none exists and remove useless spaces`` () =
    match createOrUpdateActivity Set.empty "  fake name" "   description !   " ["tag1   "; "tag2"; "tag3"] with
    | CreateActivity activity ->
        activity.IdActivity = None &&
        activity.Name = "fake name" &&
        activity.Description = "description !" &&
        activity.Tags = [| "tag1"; "tag2"; "tag3" |]
    | r ->  printfn $"%A{r}"
            false

``Should create new activity when none exists and remove useless spaces`` () = true

let ``Should update existing activity`` () =
    let activityName = "Whatever"
    let existingActivities =
        { IdActivity = Some 12
          Name = activityName
          Description = "Whatever"
          Tags = [|"I"; "love"; "FSharp"|]
          CreatedAt = DateTime(2020,01,01)
          ModifiedAt = None } |> Set.singleton
    let activityName = $"{activityName}   "
    match createOrUpdateActivity existingActivities activityName "New description" Seq.empty with
    | UpdateActivity item ->
        item.Description = "New description" &&
        item.Name = "Whatever" &&
        item.Tags = Array.empty &&
        item.CreatedAt = DateTime(2020,01,01) &&
        item.ModifiedAt.IsSome && item.ModifiedAt.Value > item.CreatedAt
    | r -> printfn $"%A{r}"
           false

``Should update existing activity`` () = true

let ``Should update existing activity regardless of name case`` () =
    let activityName = "Whatever"
    let existingActivities =
        { IdActivity = Some 12
          Name = activityName
          Description = "Whatever"
          Tags = [|"I"; "love"; "FSharp"|]
          CreatedAt = DateTime(2020,01,01)
          ModifiedAt = None } |> Set.singleton
    match createOrUpdateActivity existingActivities "WHATEVER" "New description" Seq.empty with
    | UpdateActivity item ->
        item.Description = "New description" &&
        item.Name = "Whatever" &&
        item.Tags = Array.empty &&
        item.CreatedAt = DateTime(2020,01,01) &&
        item.ModifiedAt.IsSome
    | r -> printfn $"%A{r}"
           false

``Should update existing activity regardless of name case`` () = true

let ``Should fail when activity name is null or empty`` () =
    let res = [null; ""; "   "]
              |> List.map (fun name -> createOrUpdateActivity Set.empty name "description!" ["tag1"; "tag2"; "tag3"])

    match res with
    | [ActivityErr [ActivityNameCannotBeNullOrEmpty]
       ActivityErr [ActivityNameCannotBeNullOrEmpty]
       ActivityErr [ActivityNameCannotBeNullOrEmpty]] -> true
    | r ->  printfn $"%A{r}"
            false

// ``Should fail when activity name is null or empty`` () = true

let ``Should fail when description is null or empty`` () =
    let res = [null; ""; "   "]
              |> List.map (fun desc -> createOrUpdateActivity Set.empty "Whatever" desc ["tag1"; "tag2"; "tag3"])

    match res with
    | [ActivityErr [DescriptionCannotBeNullOrEmpty]
       ActivityErr [DescriptionCannotBeNullOrEmpty]
       ActivityErr [DescriptionCannotBeNullOrEmpty]] -> true
    | r ->  printfn $"%A{r}"
            false

// ``Should fail when description is null or empty`` () = true


let ``Should fail when tags have null or empty values`` () =
    let res = [null; ""; "   "]
              |> List.map (fun tag -> createOrUpdateActivity Set.empty "Whatever" "Wherever" [tag; "tag2"; "tag3"])

    match res with
    | [ActivityErr [TagsCannotHaveNullOrEmptyValues]
       ActivityErr [TagsCannotHaveNullOrEmptyValues]
       ActivityErr [TagsCannotHaveNullOrEmptyValues]] -> true
    | r ->  printfn $"%A{r}"
            false

//``Should fail when tags have null or empty values`` () = true

let ``Should fail when duplicated tags are present`` () =
    match createOrUpdateActivity Set.empty "Whatever" "Description!" ["tag1"; "tag1"; "tag2"; "tag3"; "tag2"] with
    | ActivityErr [DuplicatedTagsDetected] -> true
    | r -> printfn $"%A{r}"
           false

// ``Should fail when duplicated tags are present`` () = true

let ``Should return all failures !``() =
    let res = createOrUpdateActivity Set.empty null "" ["  "; "tag2"; "tag2"; ""; null]
    let expected =
        [ ActivityNameCannotBeNullOrEmpty
          DescriptionCannotBeNullOrEmpty
          DuplicatedTagsDetected
          TagsCannotHaveNullOrEmptyValues ] |> Set.ofList
    match res with
    | ActivityErr xs when (xs |> Set.ofSeq) = expected && xs.Length = expected.Count -> true
    | r ->
        printfn $"%A{r}"
        false

//``Should return all failures !``() = true



//SHOWCASE if there's time !: An example of how to add a use case !