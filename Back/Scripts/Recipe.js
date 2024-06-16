const { DataTypes, Model } = require('sequelize');
const sequelize = require('./database');

class Recipe extends Model {
    static async createRecipe(data) {
        return await Recipe.create(data);
    }

    static async getAllRecipes() {
        return await Recipe.findAll();
    }

    static async getRecipeById(id) {
        return await Recipe.findByPk(id);
    }

    static async updateRecipe(id, data) {
        const recipe = await Recipe.findByPk(id);
        if (recipe) {
            return await recipe.update(data);
        }
        return null;
    }

    static async deleteRecipe(id) {
        const recipe = await Recipe.findByPk(id);
        if (recipe) {
            return await recipe.destroy();
        }
        return null;
    }
}

Recipe.init({
    recipe_id: {
        type: DataTypes.INTEGER,
        autoIncrement: true,
        primaryKey: true
    },
    name: {
        type: DataTypes.STRING,
        allowNull: false
    },
    serving_size: DataTypes.FLOAT,
    type: DataTypes.STRING,
    recipe_explanation_text: DataTypes.TEXT,
    cooking_time: DataTypes.FLOAT,
    calories_per_serving: DataTypes.FLOAT,
    carbohydrates_per_serving: DataTypes.FLOAT,
    proteins_per_serving: DataTypes.FLOAT,
    fats_per_serving: DataTypes.FLOAT
}, {
    sequelize,
    modelName: 'Recipe',
    tableName: 'recipes',
    timestamps: false
});

module.exports = Recipe;
