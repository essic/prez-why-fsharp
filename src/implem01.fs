module Implem01

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

  // function returns a Collaborator which is a Manager
  let createManager (firstName:string) (lastName:string) (age:int) (bu:ManagerBU) : Collaborator =
    let person : Person = { FirstName = firstName ; LastName = lastName ; Age = age }
    let personAndBusinessUnit : Person * ManagerBU = (person, bu)
    Manager personAndBusinessUnit

  // function returns a Collaborator which is a Consultant
  let createConsultant (firstName:string) (lastName:string) (age:int) (skills: Skill list) : Collaborator =
    let person : Person = { FirstName = firstName ; LastName = lastName ; Age = age }
    let personAndSkills : Person * (Skill list) = (person,skills)
    Consultant personAndSkills

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
          let convertSkillsToStrings (param:Skill list) : string list =
            //List.map : (T -> U) -> T list -> U list
            let result = List.map (fun skill -> sprintf "%A" skill) skills
            result
          
          //List.reduce : (T -> T -> T) -> T list -> T
          let skillsString = List.reduce (fun acc elem -> acc + ", " + elem) (convertSkillsToStrings skills)
          printfn "I work on %s" skillsString
    collaborator

  let customPrint (collaborator:Collaborator) : Unit =
    printfn "***********************"
    //(>>) : ( a -> b ) -> (b -> c) -> a -> c
    (whoAreYou >> whatDoYouDo) collaborator
    |> ignore
    printfn "***********************"

  [<EntryPoint>]
  let main (argv:string[]) : int =
    // We create some collaborators
    let collaborators = 
      [ createConsultant "Aly-Bocar" "Cisse" 32 [CSharp;FSharp]
        createManager "Luffy" "Dragon" 20 SGCIB 
        createConsultant "Nobody" "Ulysse" 15 [] ] 
    
    //Let's iterate on every Collaborator and print !
    // List.iter : ('T -> Unit) -> 'T list -> unit
    List.iter customPrint collaborators 
    0
