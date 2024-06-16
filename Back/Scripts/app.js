const express = require('express');
const bodyParser = require('body-parser');
const path = require('path');
const sequelize = require('./database');
const Ingredient = require('./Ingredient');
const Recipe = require('./Recipe');
const RecipeIngredient = require('./RecipeIngredient');

const app = express();
app.use(bodyParser.json());

app.use(express.static(path.join(__dirname, '../../Front')));

// Ruta para servir index.html
app.get('/', (req, res) => {
    res.sendFile(path.join(__dirname, '../../Front', 'index.html'));
});

app.post('/ingredients', async (req, res) => {
    try {
        const ingredient = await Ingredient.createIngredient(req.body);
        res.json(ingredient);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.get('/ingredients', async (req, res) => {
    const ingredients = await Ingredient.getAllIngredients();
    res.json(ingredients);
});

app.get('/ingredients/:id', async (req, res) => {
    const ingredient = await Ingredient.getIngredientById(req.params.id);
    res.json(ingredient);
});

app.put('/ingredients/:id', async (req, res) => {
    const ingredient = await Ingredient.updateIngredient(req.params.id, req.body);
    res.json(ingredient);
});

app.delete('/ingredients/:id', async (req, res) => {
    const ingredient = await Ingredient.deleteIngredient(req.params.id);
    res.json(ingredient);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, async () => {
    await sequelize.sync();
    console.log(`Server is running on port ${PORT}`);
});
