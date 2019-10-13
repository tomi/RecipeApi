namespace Migrations

open SimpleMigrations

[<Migration(201910122315L, "Create Ingredients")>]
type CreateIngredients() =
    inherit Migration()

    override __.Up() = base.Execute(@"CREATE TABLE Ingredients(
      id INTEGER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
      nameFi VARCHAR NOT NULL,
      nameEn VARCHAR NOT NULL,
      categoryId INTEGER NOT NULL REFERENCES IngredientCategories
    )")

    override __.Down() = base.Execute(@"DROP TABLE Ingredients")
