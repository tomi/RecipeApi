namespace Recipes

[<CLIMutable>]
type RecipeIngredientRow =
    { recipeId: int
      ingredientId: int
      modifier: string
      quantity: double
      unit: string }

[<CLIMutable>]
type RecipeIngredient =
    { ingredientId: int
      nameFi: string
      nameEn: string
      modifier: string
      quantity: double
      unit: string }

[<CLIMutable>]
type Recipe =
    { id: int
      nameFi: string
      nameEn: string
      durationMin: int
      durationMax: int
      origin: string
      steps: string array
      ingredients: RecipeIngredient array }

[<CLIMutable>]
type RecipeIngredientDto =
    { ingredientId: int
      modifier: string
      quantity: double
      unit: string }

[<CLIMutable>]
type CreateRecipeDto =
    { nameFi: string
      nameEn: string
      durationMin: int
      durationMax: System.Nullable<int>
      origin: string
      steps: string array
      ingredients: RecipeIngredientDto array }

[<CLIMutable>]
type UpdateRecipeDto =
    { id: int
      nameFi: string
      nameEn: string
      durationMin: int
      durationMax: int
      origin: string
      steps: string array
      ingredients: RecipeIngredientDto array }

module Validation =
    let runValidation validators v =
        validators
        |> List.fold (fun acc e ->
            match e v with
            | Some(k, v) -> Map.add k v acc
            | None -> acc) Map.empty

    let validateUpdateDto (v: UpdateRecipeDto) =
        let validators =
            [ fun u ->
                if isNull u.nameFi then Some("nameFi", "NameFi shouldn't be empty")
                else None
              fun u ->
                  if isNull u.nameEn then Some("nameEn", "NameEn shouldn't be empty")
                  else None
              fun u ->
                  if isNull u.steps then Some("steps", "steps shouldn't be empty")
                  elif u.steps.Length = 0 then Some("steps", "need at least some step")
                  else None ]

        runValidation validators v

    let validateCreateDto (v: CreateRecipeDto) =
        let validators =
            [ fun (u: CreateRecipeDto) ->
                if isNull u.nameFi then Some("nameFi", "NameFi shouldn't be empty")
                else None
              fun (u: CreateRecipeDto) ->
                  if isNull u.nameEn then Some("nameEn", "NameEn shouldn't be empty")
                  else None
              fun (u: CreateRecipeDto) ->
                  if isNull u.steps then Some("steps", "steps shouldn't be empty")
                  elif u.steps.Length = 0 then Some("steps", "need at least some step")
                  else None ]

        runValidation validators v
