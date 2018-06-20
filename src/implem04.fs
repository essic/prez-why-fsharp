module Implem04

  open System
   
  //This is a product type : Record
  type Person = 
    { FirstName : string 
      LastName : string 
      Age : int }
  
  //These are sum types
  type Skill =
    | CSharp
    | FSharp
    | Java
    | Finance
    | ECommerce
  
  type ManagerBU =
    | SGCIB
    | Natixis
    | BNP
  
  //This is also a sum type with product types on the inside :p
  type Collaborator =
    // 'Consultant' is a data contructor for 'Collaborator' type
    // This data constructor takes as parameter a 'Person' record and a list of 'Skills'
    | Consultant of Person * (Skill list) 
    // 'Manager' is another data constructor for 'Collaborator' type
    // This data constructor takes a 'Person' record as well and a 'ManagerBU' value
    | Manager of Person * ManagerBU 


  // anyStringIsNull : string list -> bool
  let anyStringIsNull listOfString =
    //List.fold : ('State -> 'T -> 'State) -> 'State -> 'T list -> 'State
    List.fold (fun state str -> state || String.IsNullOrWhiteSpace(str) ) false listOfString

  //type Option<T> =
  //| None
  //| Some T

  // createPersonOrFail : string -> string -> int -> Option<Person>
  let createPersonOrFail firstName lastName age =
    if ( age < 0 ) || (anyStringIsNull [firstName; lastName]) then
      None
    else
      let person = { FirstName = firstName ; LastName = lastName ; Age = age }
      Some person

  // function returns a Collaborator which is a Manager
  // createManager : string -> string -> int -> ManagerBU -> Option<Collaborator>
  let createManager firstName lastName age bu =
    let maybeAPerson =  createPersonOrFail firstName lastName age

    //Option.map : (T -> U) -> T option -> U option
    let maybeAPersonAndBusinessUnit = Option.map (fun surelyAPerson -> (surelyAPerson,bu)) maybeAPerson
    let maybeAManager = Option.map (fun surelyAPersonAndABusinessUnit -> Manager surelyAPersonAndABusinessUnit ) maybeAPersonAndBusinessUnit
    maybeAManager

  // function returns a Collaborator which is a Consultant
  // createConsultant : string -> string -> int -> Skill list -> Option<Collaborator>
  let createConsultant firstName lastName age skills =
    let maybeAPerson =  createPersonOrFail firstName lastName age
    let maybeBeAPersonAndSkills = Option.map (fun surelyAPerson -> (surelyAPerson,skills)) maybeAPerson
    let maybeBeAConsultant = Option.map (fun surelyAPersonAndSkills -> Consultant surelyAPersonAndSkills) maybeBeAPersonAndSkills
    maybeBeAConsultant

  // function to pretty print 'age', 'firtname' and 'lastname' of a Collaborator 
  let whoAreYou collaborator:Collaborator  =
    match collaborator with
    | Consultant (p,_) -> printfn "I am a %i years old, consultant. My name is %s %s" p.Age p.FirstName p.LastName
    | Manager (p,_) -> printfn "I am a %i years old, manager. My name is %s %s" p.Age p.FirstName p.LastName
    collaborator

  let whatDoYouDo collaborator:Collaborator =
    match collaborator with
    | Manager (_,businessUnit) -> printfn "I handle business on %s" (sprintf "%A" businessUnit)
    | Consultant (_,skills) ->
        match skills with
        | [] -> printfn "I have no skills yet !" // I handle the case where skills is an empty list !
        | _ -> // For any other case !
          let convertSkillsToStrings skills' : string list =
            //List.map : (T -> U) -> T list -> U list
            let result = List.map (fun skill -> sprintf "%A" skill) skills'
            result
          
          //List.reduce : (T -> T -> T) -> T list -> T
          let skillsString = List.reduce (fun acc elem -> acc + ", " + elem) (convertSkillsToStrings skills)
          printfn "I work on %s" skillsString
    collaborator

  let customPrint collaborator =
    printfn "***********************"
    //(>>) : ( a -> b ) -> (b -> c) -> a -> c
    (whoAreYou >> whatDoYouDo) collaborator
    |> ignore
    printfn "***********************"

  [<EntryPoint>]
  let main _ =
    // createConsultant' : ConsultantSkills list -> FirstName * LastName * Age -> Collaborator option
    let createConsultant' skills (firstName,lastName,age) =
      createConsultant firstName lastName age skills
 
    // createCsharpConsultant : FirstName * LastName * Age -> Collaborator option
    let createCsharpConsultant = createConsultant' [CSharp]
 
    // We create some collaborators with C# skills
    let newCollaborators = 
      [ 
        "Patrick","Sebastien",30
        "Julien","LePasBeau", 40
        "Foo","Bar",25
      ]
    
    let newCollaborators = List.map (fun info -> createCsharpConsultant info ) newCollaborators

    //Let's iterate on every Collaborator and print ! ... except for invalid collaborators !
    // List.iter : ('T -> Unit) -> 'T list -> unit
    // Option.map : (T -> U) -> T option -> U option
    // (>>) : (a -> b) -> (b -> c) -> a -> c
    // ignore : 'T -> unit
    List.iter ( (fun maybeACollaborator -> Option.map (fun collaborator -> customPrint collaborator) maybeACollaborator) >> ignore) newCollaborators 
    0
