namespace Ingredients

open Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

module Database =
    let getAll connectionString: Task<Result<Ingredient seq, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! query connection "SELECT id, nameFi, nameEn, categoryId FROM Ingredients" None
        }

    let getById connectionString id: Task<Result<Ingredient option, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! querySingle connection "SELECT id, nameFi, nameEn, categoryId FROM Ingredients WHERE id=@id"
                        (Some <| dict [ "id" => id ])
        }

    let update connectionString v: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection
                        "UPDATE Ingredients SET nameFi = @nameFi, nameEn = @nameEn, categoryId = @categoryId WHERE id=@id"
                        v
        }

    let insert connectionString v: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection
                        "INSERT INTO Ingredients(nameFi, nameEn, categoryId) VALUES (@nameFi, @nameEn, @categoryId)" v
        }

    let delete connectionString id: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection "DELETE FROM Ingredients WHERE id=@id" (dict [ "id" => id ])
        }
