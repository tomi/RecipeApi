namespace Recipes

open Database
open Dapper
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

open Ingredients

module Database =
    let getAll connectionString: Task<Result<Recipe seq, exn>> =
        task {
            use connection = getConnection (connectionString)
            let sql = @"
SELECT
    Recipes.*,
    RecipeIngredients.ingredientId as id,
    RecipeIngredients.*,
    Ingredients.*
FROM Recipes
JOIN RecipeIngredients ON RecipeIngredients.recipeId = Recipes.id
JOIN Ingredients ON Ingredients.id = RecipeIngredients.ingredientId
"
            let! res = connection.QueryAsync(sql, (fun (recipe: Recipe) (recipeIngredient: RecipeIngredientRow) (ingredient: Ingredient) ->
                (recipe, recipeIngredient, ingredient)
            ))

            let firstOfThree (a, _, _) = a

            let recipes = res
                        |> Seq.groupBy (fun (recipe, _, _) -> recipe.id)
                        |> Seq.map (fun (_, rows) ->
                            let firstRow = Seq.head rows
                            let recipe = firstOfThree firstRow
                            let ingredients = rows |> Seq.map (fun (_, b, c) -> {
                                ingredientId = b.ingredientId
                                nameFi = c.nameFi
                                nameEn = c.nameEn
                                modifier = b.modifier
                                quantity = b.quantity
                                unit = b.unit
                            })

                            { recipe with
                                ingredients = ingredients |> Seq.toArray })

            return Ok recipes
        }

    let getById connectionString id: Task<Result<Recipe option, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! querySingle connection
                        "SELECT id, nameFi, nameEn, durationMin, durationMax, origin, steps FROM Recipes WHERE id=@id"
                        (Some <| dict [ "id" => id ])
        }

    let update connectionString v: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection
                        "UPDATE Recipes SET id = @id, nameFi = @nameFi, nameEn = @nameEn, durationMin = @durationMin, durationMax = @durationMax, origin = @origin, steps = @steps WHERE id=@id"
                        v
        }

    let insertIngredients connection (recipeId: int) (v: RecipeIngredientDto array): Task<Result<int, exn>> =
        task {
            let rows =
                v |> Array.map (fun vv ->
                    {|
                        vv with
                            recipeId = recipeId
                    |})

            return! execute connection @"
INSERT INTO RecipeIngredients
(recipeId, ingredientId, modifier, quantity, unit) VALUES
(@recipeId, @ingredientId, @modifier, @quantity, @unit)" rows
        }

    let insert connectionString (v: CreateRecipeDto): Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            do! connection.OpenAsync()

            let trx = connection.BeginTransaction()

            // Create the recipe itself
            let! result = insert connection @"
INSERT INTO Recipes
(nameFi, nameEn, durationMin, durationMax, origin, steps) VALUES
(@nameFi, @nameEn, @durationMin, @durationMax, @origin, @steps) RETURNING id" v

            // Overcome the fact that I don't know how to embed this
            // into the match expression
            let handleIngredients = fun recipeId -> task {
                let! result2 = insertIngredients connection recipeId v.ingredients

                match result2 with
                | Error _ -> trx.Rollback()
                | Ok _ -> trx.Commit()

                return result2
            }

            match result with
            | Error _ ->
                trx.Rollback()
                return result
            | Ok recipeId ->
                return! handleIngredients recipeId
        // return match result with
            //         | Error _ ->
            //             trx.Rollback()
            //             result
            //         | Ok _ ->
            //             v.ingredients.
            //             trx.Commit()
            //             result


            // trx.Commit()

            // return finalResult
        }

    let delete connectionString id: Task<Result<int, exn>> =
        task {
            use connection = getConnection (connectionString)
            return! execute connection "DELETE FROM Recipes WHERE id=@id" (dict [ "id" => id ])
        }
