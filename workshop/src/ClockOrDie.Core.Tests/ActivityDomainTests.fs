module ActivityDomainTests

open System
open Xunit
open Swensen.Unquote
open ClockOrDie.Core
open ClockOrDie.Core.Domain
open ClockOrDie.Core.Domain.Services

//Before we start

let removeOddNumbers (numbers: int seq) : int seq =
    let rec doIt (numbers: int list) (acc: int list) =
        match numbers with
        | [] -> acc |> List.rev
        | n::ns ->
            doIt ns <| if n % 2 = 0 then n :: acc else acc            
    
    doIt (List.ofSeq numbers) []

[<Fact>]
let ``00. Let's write a test`` () =
    //Arrange
    let expectedResult = [0; 2; 4; 6; 8; 10]
    let sut = [0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
    
    //Act
    let result = removeOddNumbers sut
    
    //Assert
    test <@ result = expectedResult @>
 

[<Fact>]
let ``01. Should say greetings`` () =
    test <@ Say.hello () = "Hello world !" @>


// Now let's get started !

[<Fact>]
let ``02. Should create new activity when none exists`` () =
    //Arrange
    let operationTime = DateTime.Now
    let subject =
        { IdActivity = None
          Name = "Fake name"
          Tags = [| "tag1"; "tag2"; "tag3" |]
          Description = "A simple fake activity"
          CreatedAt = operationTime
          ModifiedAt = None }

    //Act
    let result =
        createOrUpdateActivity operationTime Set.empty subject.Name subject.Description subject.Tags

    //Assert
    test <@ result = ActivityCreationSuccess subject @>

[<Fact>]
let ``03. Should create new activity when none exists and remove useless spaces`` () =
    //Arrange
    let operationTime = DateTime.Now
    let expectedResult =
        { IdActivity = None
          Name = "Fake name"
          Tags = [| "tag1"; "tag2"; "tag3" |]
          Description = "A simple fake activity"
          CreatedAt = operationTime
          ModifiedAt = None }
    let subject =
        {| name = "  Fake name"
           description = "   A simple fake activity   "
           tags = [ "tag1   "; "tag2"; "tag3" ] |}

    //Act
    let result =
        createOrUpdateActivity operationTime Set.empty subject.name subject.description subject.tags  

    //Assert
    test <@ result = ActivityCreationSuccess expectedResult @>

[<Fact>]
let ``04. Should update existing activity`` () =
    //Arrange
    let creationTime = DateTime.Now.AddDays(-1)
    let updateTime = DateTime.Now
    let activityName = "Whatever"

    let existingActivity =
        { IdActivity = Some 12
          Name = activityName
          Description = "Whatever"
          Tags = [| "I"; "love"; "FSharp" |]
          CreatedAt = creationTime
          ModifiedAt = None }

    let expectedActivityResult =
        { IdActivity = Some 12
          Name = activityName
          Description = "New description"
          Tags = [||]
          CreatedAt = creationTime
          ModifiedAt = updateTime |> Some }

    let activities = Set.singleton existingActivity

    //Act
    let result =
        match
            createOrUpdateActivity
                updateTime
                activities
                activityName
                expectedActivityResult.Description
                expectedActivityResult.Tags
        with
        | ActivityUpdateSuccess r -> r
        | _ -> failwith "Test failure ! Update has failed !"

    //Assert
    test <@ result = expectedActivityResult @>

[<Fact>]
let ``05. Should update existing activity regardless of name case`` () =
    //Arrange
    let creationTime = DateTime.Now.AddDays(-1)
    let updateTime = DateTime.Now    
    let activityName = "Whatever"

    let existingActivity =
        { IdActivity = Some 12
          Name = activityName
          Description = "Whatever"
          Tags = [| "I"; "love"; "FSharp" |]
          CreatedAt = creationTime
          ModifiedAt = None }

    let expectedActivityResult =
        { IdActivity = Some 12
          Name = activityName
          Description = "New description"
          Tags = [||]
          CreatedAt = creationTime
          ModifiedAt = updateTime |> Some }

    let activities = Set.singleton existingActivity

    //Act
    let result =
        match
            createOrUpdateActivity
                updateTime
                activities
                (activityName.ToUpper())
                expectedActivityResult.Description
                expectedActivityResult.Tags
        with
        | ActivityUpdateSuccess r -> r
        | _ -> failwith "Test failure ! Update has failed !"

    //Assert
    test <@ result = expectedActivityResult @>

[<Fact>]
let ``06. Should fail when activity name is null, empty or whitespaces`` () =
    //Arrange
    let operationTime = DateTime.Now
    let invalidNames = [ null; ""; "   " ]

    let expectedResults =
        [ ActivityCreationOrUpdateFailure [ ActivityNameCannotBeNullOrEmpty ]
          ActivityCreationOrUpdateFailure [ ActivityNameCannotBeNullOrEmpty ]
          ActivityCreationOrUpdateFailure [ ActivityNameCannotBeNullOrEmpty ] ]
    //Act
    let results =
        List.map (fun name -> createOrUpdateActivity operationTime Set.empty name "description!" [ "tag1"; "tag2"; "tag3" ])
        <| invalidNames

    //Assert
    test <@ results = expectedResults @>

[<Fact>]
let ``07. Should fail when description is null or empty`` () =
    //Arrange
    let operationTime = DateTime.Now
    let invalidDescriptions = [ null; ""; "   " ]

    let expectedResults =
        [ ActivityCreationOrUpdateFailure [ DescriptionCannotBeNullOrEmpty ]
          ActivityCreationOrUpdateFailure [ DescriptionCannotBeNullOrEmpty ]
          ActivityCreationOrUpdateFailure [ DescriptionCannotBeNullOrEmpty ] ]

    //Act
    let results =
        List.map (fun desc -> createOrUpdateActivity operationTime Set.empty "Whatever" desc [ "tag1"; "tag2"; "tag3" ])
        <| invalidDescriptions

    //Assert
    test <@ results = expectedResults @>

[<Fact>]
let ``08. Should fail when tags have null or empty values`` () =
    //Arrange
    let operationTime = DateTime.Now
    let invalidTags = [ null; ""; "   " ]

    let expectedResults =
        [ ActivityCreationOrUpdateFailure [ TagsCannotHaveNullOrEmptyValues ]
          ActivityCreationOrUpdateFailure [ TagsCannotHaveNullOrEmptyValues ]
          ActivityCreationOrUpdateFailure [ TagsCannotHaveNullOrEmptyValues ] ]

    //Act
    let results =
        List.map (fun tag -> createOrUpdateActivity operationTime Set.empty "Whatever" "Wherever" [ tag; "tag2"; "tag3" ])
        <| invalidTags

    //Assert
    test <@ results = expectedResults @>

[<Fact>]
let ``09. Should fail when duplicated tags are present`` () =
    //Arrange
    let operationTime = DateTime.Now
    let duplicatedTags = [ "tag1"; "tag1"; "tag2"; "tag3"; "tag2" ]
    let expectedResult = ActivityCreationOrUpdateFailure [ DuplicatedTagsDetected ]

    //Act
    let result =
        createOrUpdateActivity operationTime Set.empty "Whatever" "Description!" duplicatedTags

    //Assert
    test <@ result = expectedResult @>
