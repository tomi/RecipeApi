module Server

open Saturn
open Config

let endpointPipe =
    pipeline {
        plug head
        plug requestId
    }

let app =
    application {
        pipe_through endpointPipe

        error_handler (fun ex _ -> pipeline { json ex })
        use_router Router.appRouter
        url "http://0.0.0.0:8085/"
        memory_cache
        use_static "static"
        use_gzip
        use_config
            (fun _ ->
            { connectionString = "Server=127.0.0.1;Port=5445;Database=recipedb;User Id=recipeapp;Password=recipeapp;" }) //TODO: Set development time configuration
    }

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code
