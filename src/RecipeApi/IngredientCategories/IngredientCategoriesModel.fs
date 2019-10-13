namespace IngredientCategories

[<CLIMutable>]
type IngredientCategory = {
  id: int
  nameFi: string
  nameEn: string
  parentId: System.Nullable<int>
}

module Validation =
  let validate v =
    let validators = [
      fun u -> if isNull u.nameFi then Some ("nameFi", "nameFi shouldn't be empty") else None
      fun u -> if isNull u.nameEn then Some ("nameEn", "nameEn shouldn't be empty") else None
    ]

    validators
    |> List.fold (fun acc e ->
      match e v with
      | Some (k,v) -> Map.add k v acc
      | None -> acc
    ) Map.empty
