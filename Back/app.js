const express = require('express');
const bodyParser = require('body-parser');
const mariadb = require('mariadb');
const path = require('path');

const app = express();
const port = 3000;

// Configurar body-parser
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

// Configuración de la conexión a MariaDB
const pool = mariadb.createPool({
  host: 'localhost', 
  user: 'test1', 
  password: '12345',
  database: 'gastronomyarchive'
});

// Servir archivos estáticos desde la carpeta 'Front'
app.use(express.static(path.join(__dirname, '../Front')));

// Ruta para agregar un ingrediente
app.post('/api/ingredients', async (req, res) => {
  console.log('POST /api/ingredients');
  const { name, protein, fat, carbs, calories } = req.body;
  console.log(req.body);
  try {
    const conn = await pool.getConnection();
    const result = await conn.query('INSERT INTO ingredients (name, protein, fat, carbs, calories) VALUES (?, ?, ?, ?, ?)', 
                                    [name, protein, fat, carbs, calories]);
    res.json({ message: 'Ingrediente añadido correctamente', result });
    conn.release();
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: err.message });
  }
});

// Ruta para obtener todos los ingredientes
app.get('/api/ingredients', async (req, res) => {
  console.log('GET /api/ingredients');
  try {
    const conn = await pool.getConnection();
    const rows = await conn.query('SELECT * FROM ingredients');
    res.json(rows);
    conn.release();
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: err.message });
  }
});

// Iniciar el servidor
app.listen(port, () => {
  console.log(`Servidor escuchando en http://localhost:${port}`);
});
