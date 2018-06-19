# Why F# ?

---

> "Point of view is worth 80 IQ points" <br> Alan Kay

---

### What is F# ?

- Strong, static language from Microsoft with deep inference |
- 1.0 appeared on 2005 ( older than GO ...) |
- Multi-paradigm language : functional, imperative and objected oriented |
- Open Source & Cross Platform |
- Heavily influenced by ML, OCaml, Haskell, C# & others |
- Fully supported by .NET, Mono & Javascript ! |

---
### About functional paradigm

- Functional vs Object is a myth |
- Object Oriented paradigm focus on structure |
- Functional paradigm focus on data |
- Functional paradigm is around for a long time |
- … and it’s been spreading for a while C#, Java, Kotlin, Rust … |
- You do not need math, to go into production |
- … but there is a direct relation, it is interesting and useful ! (Monoid, Functor, Monad ...) |

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
@[6-8](These are type aliases)
@[11-14](We define a basic record, representing a person. This is one kind of product type)
@[17-27](These represents skills and business unit values. These are sum types)
@[30-36](This represent a collaborator. Many things here.)
@[41-43](This is a function, that we use as a constructor to create a Consultant value)
@[46-47](A function again use to create a Manager value this time)
@[50-54](Pretty print age, first name and last name to the console. Also meet Pattern Matching)
@[56-66](Pretty print what a collaborator does. Hello again Pattern matching ! Also welcome to map & reduce !)
@[67-73](Pretty print collaborator information with function composition)
@[74-87](Main function !)

+++
[REPL here](https://repl.it/@essic/SimpleFSharpDemo-01)

---

### Inference magic
@fa[arrow-down]

+++?code=src/implem02.fs&lang=fsharp
@[30-31](We remove types, F# is smart enough to figure out things)
@[33-34]
@[35-40]
@[41-51]
@[53-57]
@[59-68]

+++
[REPL here](https://repl.it/@essic/SimpleFSharpDemo-02)

---

### Naive error handling in F# !
@fa[arrow-down]

+++?code=src/implem03.fs&lang=fsharp
@[29-32](We use reduce again to check if in our list of string, we got on or more null ...)
@[34-39](We create a person constructor, with the 'T option' type !)
@[41-50](We use the new Person contructor with map, to create 'Collaborator option'.)
@[52-67](We change nothing here)
@[69-73](Same here)
@[83-85](Let's add some bad entries !)
@[88-92](The magic is here !)

+++
[REPL here](https://repl.it/@essic/SimpleFSharpDemo-03)

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
@[70-86]
@[72-77](Currying !)

+++
[REPL here](https://repl.it/@essic/SimpleFSharpDemo-04)

---
### So ... why F# ?
@fa[arrow-down]

+++
### Functional first 
- Focus on data 
- Function composition 
- Currying and partial application offered
- Map, Filter, Reduce at the heart of the language 

+++
### Immutable by default
- Simpler to reason about your program
- Allows for safer concurrency and parallelism

+++
### Error handling
- Consistant typings and constructions => make invalid state non representable with ease
- [Railway Oriented Programing](https://fsharpforfunandprofit.com/rop/)

+++
### And more !
- Interactivity with REPL & Scripting, checkout [FAKE](https://fake.build/) DSL for build task.
- IDE choice VScode or Atom with [Ionide](http://ionide.io/), Visual Studio
- Readability (from top to bottom, left to right) & consize code

+++
- No value by default, so no null by default as well !
- Strong inference !
- Pattern matching, Active patterns , Async computation, Computation expression, Type Providers ...
  
---

### Production ready ?

- Jet.com
- [Testimonials](https://fsharp.org/testimonials/) 

---

### Libraries 
- Paket |
- Fake |
- Suave |
- Unquote |
- Hedgehog |
- and more ! |

---

### More 

- https://fsharpforfunandprofit.com 
- https://fsharp.org/ 
- http://fable.io/ F# to JavaScript compiler 
- http://fsharp.github.io/FSharp.Data 
- Many more resources ... great community so don't hesitate to ask !

---

### Object oriented programming is not dead. <br> However having another point of view, is worth it !

---

### Thank you !
