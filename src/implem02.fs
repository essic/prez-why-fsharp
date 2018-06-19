module Implem02

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

  let createConsultant firstName lastName age skills =
    ({ FirstName = firstName ; LastName = lastName ; Age = age } , skills) |> Consultant
    
  let createManager firstName lastName age bu =
    Manager ({ FirstName = firstName ; LastName = lastName ; Age = age } , bu)

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
        | [] -> printfn "I have no skills yet !" 
        | _ ->
          let skillsString = List.reduce (fun acc elem -> acc + ", " + elem) ( skills |> List.map (sprintf "%A") )
          printfn "I work on %s" skillsString
    collaborator

  let customPrint collaborator =
    printfn "***********************"
    collaborator
    |> (whoAreYou >> whatDoYouDo) |> ignore
    printfn "***********************"

  [<EntryPoint>]
  let main _ =
    let collaborators = 
      [ createConsultant "Aly-Bocar" "Cisse" 32 [CSharp;FSharp]
        createManager "Luffy" "Dragon" 20 SGCIB 
        createConsultant "Nobody" "Ulysse" 15 [] ] 
    
    collaborators 
    |> List.iter customPrint 
    0
