module Implem01

  open System
  
  //These are type aliases
  type Age = int
  type FirstName = string
  type LastName = string
  
  //This is a product type : Record
  type Person = 
    { FirstName : FirstName 
      LastName : LastName 
      Age : Age }
  
  //These are sum types
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
  
  //This is also a sum type
  type Collaborator =
    // 'Consultant' is a data contructor for 'Collaborator' type
    // This data constructor takes as parameter a 'Person' record and a list of 'ConsultantSkills'
    | Consultant of Person * (ConsultantSkills list) 
    // 'Manager' is another data constructor for 'Collaborator' type
    // This data constructor takes a 'Person' record as well and a 'ManagerBU' value
    | Manager of Person * ManagerBU 

  // ( |> ) : 'T1 -> ('T1 -> 'U) -> 'U
  
  // function returns a Collaborator which is a Consultant
  let createConsultant (firstName:string) (lastName:string) (age:int) (skills: ConsultantSkills list) : Collaborator =
    //Consultant ({ FirstName = firstName ; LastName = lastName ; Age = age } , skills )
    ({ FirstName = firstName ; LastName = lastName ; Age = age } , skills) |> Consultant
    
  // function returns a Collaborator which is a Manager
  let createManager (firstName:FirstName) (lastName:LastName) (age:Age) (bu:ManagerBU) : Collaborator =
    Manager ({ FirstName = firstName ; LastName = lastName ; Age = age } , bu)

  // function to pretty print 'age', 'firtname' and 'lastname' of a Collaborator 
  let whoAreYou (collaborator:Collaborator) : Collaborator =
    match collaborator with
    | Consultant (p,_) -> printfn "I am a %i years old, consultant. My name is %s %s" p.Age p.FirstName p.LastName
    | Manager (p,_) -> printfn "I am a %i years old, manager. My name is %s %s" p.Age p.FirstName p.LastName
    collaborator

  let whatDoYouDo (collaborator:Collaborator) : Collaborator =
    match collaborator with
    | Manager (_,businessUnit) -> printfn "I handle business on %s" <| sprintf "%A" businessUnit 
    | Consultant (_,skills) ->
        match skills with
        | [] -> printfn "I have no skills yet !" // I handle the case where skills is an empty list !
        | _ -> // For any other case !
          let skillsString = List.reduce (fun acc elem -> acc + ", " + elem) ( skills |> List.map (sprintf "%A") )
          printfn "I work on %s" skillsString
    collaborator

  let customPrint (collaborator:Collaborator) : Unit =
    printfn "***********************"
    collaborator
    |> (whoAreYou >> whatDoYouDo) |> ignore
    printfn "***********************"

  [<EntryPoint>]
  let main (argv:string[]) : int =
    // We create some collaborators
    let collaborators = 
      [ createConsultant "Aly-Bocar" "Cisse" 32 [CSharp;FSharp]
        createManager "Luffy" "Dragon" 20 SGCIB 
        createConsultant "Nobody" "Ulysse" 15 [] ] 
    
    //Let's iterate on every Collaborator and print !
    collaborators 
    // List.iter : ('T -> unit) -> 'T list -> unit
    |> List.iter customPrint 
    0
