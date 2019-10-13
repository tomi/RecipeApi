namespace IngredientCategories

open Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

module Database =
    let getAll connectionString: Task<Result<IngredientCategory seq, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! query connection "SELECT id, nameFi, nameEn, parentId FROM IngredientCategories" None
        }

    let getById connectionString id: Task<Result<IngredientCategory option, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! querySingle connection "SELECT id, nameFi, nameEn, parentId FROM IngredientCategories WHERE id=@id"
                        (Some <| dict [ "id" => id ])
        }

    let update connectionString v: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection
                        "UPDATE IngredientCategories SET id = @id, nameFi = @nameFi, nameEn = @nameEn, parentId = @parentId WHERE id=@id"
                        v
        }

    let insert connectionString v: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection
                        "INSERT INTO IngredientCategories(nameFi, nameEn, parentId) VALUES (@nameFi, @nameEn, @parentId)"
                        v
        }

    let delete connectionString id: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection "DELETE FROM IngredientCategories WHERE id=@id" (dict [ "id" => id ])
        }
