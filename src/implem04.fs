module Implem04

  open System

  type Age = int
  type FirstName = string
  type LastName = string

  type Person = 
    { FirstName : FirstName 
      LastName : LastName 
      Age : Age }
  
  type ConsultantSkills =
    | CSharp
    | FSharp
    | Java
    | Finance
    | ECommerce
  
  type ManagerBU =
    | SGCIB
    | Natixis
    | BNP
  
  type Collaborator =
    | Consultant of Person * (ConsultantSkills list) 
    | Manager of Person * ManagerBU 

  let isNull a =
    a |> List.fold (fun state str -> state || String.IsNullOrWhiteSpace(str) ) false 

  let createPerson firstName lastName age =
    if ( age < 0 ) || (isNull [firstName; lastName]) then
      None
    else
      { FirstName = firstName ; LastName = lastName ; Age = age } |> Some

  let createConsultant firstName lastName age skills =
    createPerson firstName lastName age 
    |> Option.map (fun person -> Consultant (person,skills) )
  
  let createManager firstName lastName age bu =
    createPerson firstName lastName age
    |> Option.map (fun person -> Manager (person,bu))

  let whoAreYou collaborator =
    match collaborator with
    | Consultant (p,_) -> printfn "I am a %i years old, consultant. My name is %s %s" p.Age p.FirstName p.LastName
    | Manager (p,_) -> printfn "I am a %i years old, manager. My name is %s %s" p.Age p.FirstName p.LastName
    collaborator

  let whatDoYouDo collaborator =
    match collaborator with
    | Manager (_,businessUnit) -> printfn "I handle business on %s" <| sprintf "%A" businessUnit 
    | Consultant (_,skills) ->
        match skills with
        | [] -> printfn "I have no skills yet !" // I handle the case where skills is an empty list !
        | _ -> // For any other case !
          let skillsString = List.reduce (fun acc elem -> acc + ", " + elem) ( skills |> List.map (sprintf "%A") )
          printfn "I work on %s" skillsString
    collaborator

  let customPrint collaborator =
    printfn "***********************"
    collaborator
    |> (whoAreYou >> whatDoYouDo) |> ignore
    printfn "***********************"

  [<EntryPoint>]
  let main argv =
    // createConsultant' : ConsultantSkills list -> FirstName * LastName * Age -> Collaborator option
    let createConsultant' skills (firstName,lastName,age) =
      createConsultant firstName lastName age skills
     
    // createCsharpConsultant : FirstName * LastName * Age -> Collaborator option
    let createCsharpConsultant = createConsultant' [CSharp]

    // We create some collaborators with C# skills
    [ 
      "Patrick","Sebastien",30
      "Julien","LePasBeau", 40
      "Foo","Bar",25
    ]
    |> List.map (fun info -> createCsharpConsultant info )
    |> List.iter (Option.map customPrint >> ignore)
    0
