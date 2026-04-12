module Program

open Microsoft.Testing.Platform.Builder

[<EntryPoint>]
let main args =
    task {
        let! builder = TestApplication.CreateBuilderAsync(args)
        builder.AddXunit()
        use! app = builder.BuildAsync()
        return! app.RunAsync()
    }
    |> fun t -> t.GetAwaiter().GetResult()
