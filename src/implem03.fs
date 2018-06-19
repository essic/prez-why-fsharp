module Implem03

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
    //List.fold : ('State -> 'T -> 'State) -> 'State -> 'T list -> 'State
    a |> List.fold (fun state str -> state || String.IsNullOrWhiteSpace(str) ) false 

  //string -> string -> int -> Person option
  let createPerson firstName lastName age =
    if ( age < 0 ) || (isNull [firstName; lastName]) then
      None
    else
      { FirstName = firstName ; LastName = lastName ; Age = age } |> Some

  //string -> string -> int -> Collaborator option
  let createConsultant firstName lastName age skills =
    createPerson firstName lastName age 
    // map : (T -> u) -> T option -> U option
    |> Option.map (fun person -> Consultant (person,skills) )
    
  //string -> string -> int -> Collaborator option
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
    // We create some collaborators
    let collaborators  = 
      [ 
        createConsultant "Aly-Bocar" "Cisse" 32 [CSharp;FSharp]
        createManager "Luffy" "Dragon" 20 SGCIB 
        createConsultant "Nobody" "Ulysse" 15 []
        createManager "Kevin" "" 25 Natixis //Should fail !
        createConsultant null "Something" 30 [Java] //Should fail !
        createConsultant "Gandalf" "The Gray" -200 [Finance]  // Should fail ! 
      ]
    //Let's iterate on every Collaborator and print !
    collaborators 
    //List.iter : ('T -> unit) -> 'T list -> unit
    //map : ('T -> 'U) -> 'T option -> 'U option
    //customPrint : Collaborator -> Unit
    |> List.iter (Option.map customPrint >> ignore)
    0
