#!/bin/bash
set -e

PGPASSWORD=recipeapp psql -h 127.0.0.1 -p 5445 -U recipeapp -c 'DROP DATABASE IF EXISTS recipedb' postgres
PGPASSWORD=recipeapp psql -h 127.0.0.1 -p 5445 -U recipeapp -c 'CREATE DATABASE recipedb' postgres

dotnet saturn migration
