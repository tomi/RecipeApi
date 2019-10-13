namespace Ingredients

[<CLIMutable>]
type Ingredient = {
  id: int
  nameFi: string
  nameEn: string
  categoryId: int
}

module Validation =
  let validate v =
    let validators = [
      fun u -> if isNull u.nameFi then Some ("nameFi", "NameFi shouldn't be empty") else None
      fun u -> if isNull u.nameEn then Some ("nameEn", "NameEn shouldn't be empty") else None
    ]

    validators
    |> List.fold (fun acc e ->
      match e v with
      | Some (k,v) -> Map.add k v acc
      | None -> acc
    ) Map.empty
