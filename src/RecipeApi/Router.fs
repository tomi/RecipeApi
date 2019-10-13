module Router

open Saturn

//Other scopes may use different pipelines and error handlers

let api = pipeline {
    // plug acceptJson
    set_header "x-pipeline-type" "Api"
}

let apiRouter = router {
    // not_found_handler (text "Api 404")
    pipe_through api

    forward "/ingredientCategories" IngredientCategories.Controller.resource
    forward "/ingredients" Ingredients.Controller.resource
    forward "/recipes" Recipes.Controller.resource
}

let appRouter = router {
    forward "/api" apiRouter
}
