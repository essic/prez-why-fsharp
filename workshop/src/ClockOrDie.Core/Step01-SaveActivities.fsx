#load @"../../.paket/load/net6.0/Core/core.group.fsx"
#load @"Library.fs"
#load @"Domain.fs"

// open System
// open ClockOrDie.Core
// open ClockOrDie.Core.Domain
// open ClockOrDie.Core.Domain.Services

// ``Should fail when duplicated tags are present`` () = true

// let ``Should return all failures !``() =
//     let res = createOrUpdateActivity Set.empty null "" ["  "; "tag2"; "tag2"; ""; null]
//     let expected =
//         [ ActivityNameCannotBeNullOrEmpty
//           DescriptionCannotBeNullOrEmpty
//           DuplicatedTagsDetected
//           TagsCannotHaveNullOrEmptyValues ] |> Set.ofList
//     match res with
//     | ActivityErr xs when (xs |> Set.ofSeq) = expected && xs.Length = expected.Count -> true
//     | r ->
//         printfn $"%A{r}"
//         false



//SHOWCASE if there's time !: An example of how to add a use case !