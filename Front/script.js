document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('ingredient-form');
    const ingredientsList = document.getElementById('ingredients-list');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        const name = document.getElementById('name').value;
        const protein = document.getElementById('protein').value;
        const fat = document.getElementById('fat').value;
        const carbs = document.getElementById('carbs').value;
        const calories = document.getElementById('calories').value;

        console.log('Submitting form');
        console.log({ name, protein, fat, carbs, calories });

        const response = await fetch('/api/ingredients', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name, protein, fat, carbs, calories }),
        });

        if (response.ok) {
            console.log('Ingredient added');
            loadIngredients();
        } else {
            console.error('Failed to add ingredient');
        }
    });

    const loadIngredients = async () => {
        console.log('Loading ingredients');
        const response = await fetch('/api/ingredients');
        const ingredients = await response.json();

        ingredientsList.innerHTML = '';
        ingredients.forEach(ingredient => {
            const li = document.createElement('li');
            li.textContent = `${ingredient.name} - Proteína: ${ingredient.protein}g, Grasa: ${ingredient.fat}g, Carbohidratos: ${ingredient.carbs}g, Calorías: ${ingredient.calories}`;
            ingredientsList.appendChild(li);
        });
    };

    loadIngredients();
});
