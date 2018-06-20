# Why F# ?

[@essiccf37](https://twitter.com/essiccf37) <br> June 2018

---

> "Point of view is worth 80 IQ points" <br> Alan Kay

---

### What is F# ?

- Strongly, statically typed, open sourced and cross platform language from Microsoft with deep inference |
- 1.0 appeared on 2005 ( older than GO ...) |
- Multi-paradigm language : functional, imperative and object-oriented |

---
### What is F# ?

- It compiles to executable or you can script with it |
- Heavily influenced by ML, OCaml, Haskell, C# & others |
- Fully supported by .NET & Mono. Compiles to Javascript. |

---
### About functional paradigm

- Functional vs Object is a myth |
- Object Oriented paradigm focus on structure |
- Functional paradigm focus on data |
- Functional paradigm is old |
- … and it’s been spreading for a while now : C#, Java, Kotlin, Rust … |
- You do not need math, to go to production |
- … but there is a relation, it's interesting and useful ! (Monoid, Functor, Applicative, Monad ...) |

---
### Demo !

Showing information about collaborators <br> @fa[arrow-down]

+++
### 2 kinds
- Consultants |
- Managers |

+++
### Basic information
- First name |
- Last name |
- Age |

+++
### Also ...
- Managers handles business unit |
- Consultants have skills |

---

### First implementation
@fa[arrow-down]

+++?code=src/implem01.fs&lang=fsharp
@[1](We create our module)
@[3](Basic import, useful to access String type)
@[6-9](We define a basic record, representing a person. This is one kind of product type)
@[12-22](These represents possible skills and business unit values. These are sum types)
@[25-31](This represent a collaborator. Many things here.)
@[34-37](Function use to create a Manager value)
@[35](We create a Person)
@[36](We create a Tuple - Product Type - of Person and ManagerBU)
@[37](We create and return a Manager value, which is of type Collaborator)
@[40-43](Function use to create a Consultant value. Same thing !)
@[46-50](Pretty print age, first name and last name to the console. Also meet 'Pattern Matching' !)
@[47](This is it !)
@[48](Match this pattern, do this !)
@[49](Match that pattern, do that !)
@[52-67](Pretty print what a collaborator does. Hello again Pattern matching ! Also welcome to map & reduce !)
@[53-54](We handle Manager first)
@[53,55](Now we handle consultant)
@[55-57](Let's match on empty list of skills first for consultant)
@[55,56,58-66](For any other case ...)
@[59-62](Map this !)
@[65](Reduce that !)
@[69-74](Pretty print collaborator information with function composition)
@[71-72](Yes, right here)
@[75-87](Main function !)

+++
[REPL here](https://repl.it/@essic/whyFsharpDemo1-public)

---

### Inference magic
@fa[arrow-down]

+++?code=src/implem02.fs&lang=fsharp
@[34-37,40-43](We remove types, F# is smart enough to figure out things)
@[46-50]
@[52-67]
@[69-74]
@[77-87]

+++
[REPL here](https://repl.it/@essic/whyFsharpDemo2-public)

---

### Naive error handling in F# !
@fa[arrow-down]

+++
We wish to forbids the creation of a person with an invalid name and / or first name

+++
Let's say an invalid name / first name is null or empty here

+++
For an invalid age, since we're cool individuals, we'll just forbids negative number

+++?code=src/implem03.fs&lang=fsharp
@[34-37](We create a function which check if I got any nullable string is my list !)
@[37](We use a Folding to do that ! Reduce is in fact a kind of Fold :p)
@[39-49](We create a function to create a person or fail if any entry is invalid, let's use the 'T option' type ...)
@[39-41](This is our guy ! A sum type, yes.)
@[45-46](If we got an invalid entry we return 'None' which is a valid value of the 'Person option' type !)
@[45,48-49](If all is right, we return 'Some person' which is also a valid value of the type 'Person option' type !)
@[52-59](Let's use it to create a manager or fail if entries are invalid)
@[54]
@[56-57](Since we are not sure of the 'person', we use Option.map)
@[56-58](Same here)
@[59](So we return a value of the 'Collaborator option' type)
@[62-67](Same here)
@[70-74](We change nothing here)
@[76-98](Here as well ...)
@[100-118](Our main did change a little...)
@[108-110](Let's add some bad entries !)
@[112-117](The magic is here !)

+++
[REPL here](https://repl.it/@essic/whyFsharpDemo3-public) <br> @fa[arrow-down]

+++
### There are better ways ! 
@fa[arrow-down]

+++
Make illegal state non representable
```fsharp
type FirstName = private T of string

module FirstName =
  // String -> Option<FirstName>
  let create str =
    if String.IsNullOrWhiteSpace(str) then
      None
    else
      T str |> Some
```

+++
Using Result and its operators
```fsharp
type FirstName = private T of string

module FirstName =
  // type Result<TSuccess,TError> =
  // | Ok of TSuccess
  // | Error of TError
  
  // String -> Result<FirstName,String>
  let create str =
    if String.IsNullOrWhiteSpace(str) then
      Error "Can't initialize firstname with empty or null value !"
    else
      T str |> Ok
let doSomething () =
  //...
  FirstName.create someVariable 
  |> Result.map ( doSomethingWithFirstIfSuccess )
  |> Result.mapError ( handleErrorIfFailure )
```

+++
And a lot more to explore ...

---
### One small currying example 
@fa[arrow-down]

+++?code=src/implem04.fs&lang=fsharp
@[101-125]
@[103-104](An alternative definition to create a consultant)
@[107](Currying !)
@[109-117](We now use it)

+++
[REPL here](https://repl.it/@essic/whyFsharpDemo4-public)

---
### So ... why F# ?
@fa[arrow-down]

+++
### Functional first 
- Focus on data 
- Function composition 
- Currying and partial application for free
- Map, Filter, Reduce at the heart of the language 

+++
### Immutable by default
- Simpler to reason about your program
- Allows for safer concurrency and parallelism

+++
### Error handling
- Consistant typings and constructions => make invalid state non representable with ease, reduces opportunity for failure
- Result & Option are pure gold
- [Railway Oriented Programing](https://fsharpforfunandprofit.com/rop/)

+++

### Just a little bit ...
- Interactivity with REPL & Scripting, checkout [FAKE](https://fake.build/) DSL for build task.
- IDE choices expanded, VScode or Atom with [Ionide](http://ionide.io/) or still Visual Studio
- Readability (from top to bottom, left to right) 
- Consize code, less boiler plate

+++
### more !
- No value by default, so no null by default as well !
- Strong inference !
- Pattern matching, Active patterns , Async computation, Computation expression, Type Providers ...
  
---

### Production ready ?

- [Jet.com](http://fsharpshow.com/3-jet-revolutionizing-ecommerce-using-fsharp)
- [Testimonials](https://fsharp.org/testimonials/) 

---

### Libraries ?
@fa[arrow-down]

+++
[Paket](https://fsprojects.github.io/Paket/) is a dependency manager for .NET and mono projects 

+++
[Fake](https://fake.build/) is a DSL for build task 

+++
[Suave](https://suave.io/) is a Web development library 

+++
[Fsharp.Data](http://fsharp.github.io/FSharp.Data/) is a library for Data Access <br> (Type Providers !!) 

+++
[Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog) is a Property Based testing in F#  

+++
also any C# package that you can find, checkout NugGet, there's a lot

---

### More 

- https://fsharpforfunandprofit.com : great content about learning F#
- https://fsharp.org : Foundation website
- http://fable.io/ : F# to JavaScript compiler (write F#, transpiles Javascript)
- https://fsharp.org/guides/slack/ : Slack
- Many more resources ... great community so don't hesitate to ask !

---

### Object oriented programming is not dead. <br> However having another point of view, is worth it !

---

### Thank you !

Don't hesitate to drop by on [github](https://github.com/essic/prez-why-fsharp) or [tweet](https://twitter.com/essiccf37) me for any mistakes or observations, you might have.
