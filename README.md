JSON REST API for handling recipes. Using

* Saturn
* F#
* Postgresql

## Build

```
fake build
```

## Run

```
fake build -t Run
```

## Generate a model, database layer and controller for an entity

```
saturn dotnet gen.json NAME NAMES COLUMN:TYPE COLUMN:TYPE COLUMN:TYPE
```

## Migrate

```
cd src/RecipeApi
dotnet saturn migration
```
