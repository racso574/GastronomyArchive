const { DataTypes, Model } = require('sequelize');
const sequelize = require('./database');

class Ingredient extends Model {
    static async createIngredient(data) {
        return await Ingredient.create(data);
    }

    static async getAllIngredients() {
        return await Ingredient.findAll();
    }

    static async getIngredientById(id) {
        return await Ingredient.findByPk(id);
    }

    static async updateIngredient(id, data) {
        const ingredient = await Ingredient.findByPk(id);
        if (ingredient) {
            return await ingredient.update(data);
        }
        return null;
    }

    static async deleteIngredient(id) {
        const ingredient = await Ingredient.findByPk(id);
        if (ingredient) {
            return await ingredient.destroy();
        }
        return null;
    }
}

Ingredient.init({
    ingredient_id: {
        type: DataTypes.INTEGER,
        autoIncrement: true,
        primaryKey: true
    },
    name: {
        type: DataTypes.STRING,
        allowNull: false
    },
    calories: DataTypes.FLOAT,
    carbohydrates: DataTypes.FLOAT,
    proteins: DataTypes.FLOAT,
    fats: DataTypes.FLOAT,
    weight_per_unit: DataTypes.FLOAT
}, {
    sequelize,
    modelName: 'Ingredient',
    tableName: 'ingredients',
    timestamps: false
});

module.exports = Ingredient;
