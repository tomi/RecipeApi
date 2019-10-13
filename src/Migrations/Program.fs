module Program

open System.Reflection
open SimpleMigrations
open SimpleMigrations.DatabaseProvider
open SimpleMigrations.Console
open Npgsql

[<EntryPoint>]
let main argv =
    let assembly = Assembly.GetExecutingAssembly()
    use db = new NpgsqlConnection "Server=127.0.0.1;Port=5445;Database=recipedb;User Id=recipeapp;Password=recipeapp;"
    let provider = PostgresqlDatabaseProvider(db)
    let migrator = SimpleMigrator(assembly, provider)
    let consoleRunner = ConsoleRunner(migrator)
    consoleRunner.Run(argv)
