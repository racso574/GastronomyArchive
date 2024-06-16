document.getElementById('addIngredientForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const name = document.getElementById('name').value;
    const calories = document.getElementById('calories').value;
    const carbohydrates = document.getElementById('carbohydrates').value;
    const proteins = document.getElementById('proteins').value;
    const fats = document.getElementById('fats').value;
    const weight_per_unit = document.getElementById('weight_per_unit').value;

    const response = await fetch('http://localhost:3000/ingredients', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name, calories, carbohydrates, proteins, fats, weight_per_unit })
    });

    const result = await response.json();
    alert(`Ingredient added: ${result.name}`);
    document.getElementById('addIngredientForm').reset();
});

document.getElementById('loadIngredients').addEventListener('click', async () => {
    const response = await fetch('http://localhost:3000/ingredients');
    const ingredients = await response.json();

    const ingredientList = document.getElementById('ingredientList');
    ingredientList.innerHTML = '';
    ingredients.forEach(ingredient => {
        const li = document.createElement('li');
        li.textContent = `${ingredient.name} - ${ingredient.calories} calories`;
        ingredientList.appendChild(li);
    });
});