// models/RecipeIngredient.js
const { DataTypes, Model } = require('sequelize');
const sequelize = require('./database');
const Recipe = require('./Recipe');
const Ingredient = require('./Ingredient');

class RecipeIngredient extends Model {}

RecipeIngredient.init({
    recipe_id: {
        type: DataTypes.INTEGER,
        references: {
            model: Recipe,
            key: 'recipe_id'
        },
        primaryKey: true
    },
    ingredient_id: {
        type: DataTypes.INTEGER,
        references: {
            model: Ingredient,
            key: 'ingredient_id'
        },
        primaryKey: true
    },
    quantity: {
        type: DataTypes.FLOAT,
        allowNull: false
    },
    unit: {
        type: DataTypes.STRING, // Asegúrate de que el tipo sea STRING (equivalente a CHAR en SQL)
        allowNull: false
    }
}, {
    sequelize,
    modelName: 'RecipeIngredient',
    tableName: 'recipe_ingredients_list',  // Cambia el nombre de la tabla aquí
    timestamps: false
});

module.exports = RecipeIngredient;
