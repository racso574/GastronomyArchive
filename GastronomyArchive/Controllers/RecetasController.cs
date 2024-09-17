using Microsoft.AspNetCore.Mvc;
using AlimentosAPI.Data;
using AlimentosAPI.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RecetasController : ControllerBase
{
    private readonly AlimentosContext _context;

    public RecetasController(AlimentosContext context)
    {
        _context = context;
    }

    // Crear una nueva receta
    [HttpPost]
    public async Task<ActionResult<Receta>> CrearReceta(Receta receta)
    {
        _context.Recetas.Add(receta);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReceta), new { id = receta.Id }, receta);
    }

    // Obtener una receta por Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Receta>> GetReceta(int id)
    {
        var receta = await _context.Recetas
            .Include(r => r.RecetaAlimentos)
            .ThenInclude(ra => ra.Alimento)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (receta == null)
        {
            return NotFound();
        }

        return receta;
    }

    // Editar una receta
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReceta(int id, Receta receta)
    {
        if (id != receta.Id)
        {
            return BadRequest();
        }

        _context.Entry(receta).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RecetaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // Eliminar una receta
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReceta(int id)
    {
        var receta = await _context.Recetas.FindAsync(id);
        if (receta == null)
        {
            return NotFound();
        }

        _context.Recetas.Remove(receta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Calcular macros de una receta con cantidades ajustadas por personas
    [HttpGet("{id}/calcular-macros")]
    public async Task<ActionResult<dynamic>> CalcularMacros(int id, [FromQuery] int? personas = null)
    {
        var receta = await _context.Recetas
            .Include(r => r.RecetaAlimentos)
            .ThenInclude(ra => ra.Alimento)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (receta == null)
        {
            return NotFound();
        }

        // Si no se especifica una cantidad de personas, usar la cantidad base de la receta
        int personasCalculadas = personas ?? receta.CantidadPersonasBase;

        decimal totalProteinas = 0, totalCarbohidratos = 0, totalGrasas = 0, totalCalorias = 0;

        // Lista para almacenar las cantidades ajustadas de los ingredientes
        var ingredientesAjustados = new List<dynamic>();

        foreach (var recetaAlimento in receta.RecetaAlimentos)
        {
            var alimento = recetaAlimento.Alimento;

            // Escalar la cantidad de ingredientes según la cantidad de personas
            var cantidadEscalada = recetaAlimento.Cantidad * personasCalculadas / receta.CantidadPersonasBase;

            // Manejar el caso donde PesoUnidad puede ser null.
            var cantidadEnGramos = recetaAlimento.EsEnGramos 
                ? cantidadEscalada 
                : (cantidadEscalada * (alimento.PesoUnidad ?? 0));

            // Factor para ajustar las macros basado en la cantidad de alimento (en gramos)
            var factor = cantidadEnGramos / 100;

            // Calcular las macros por ración (1 persona)
            totalProteinas += alimento.Proteinas * factor;
            totalCarbohidratos += alimento.Carbohidratos * factor;
            totalGrasas += alimento.Grasas * factor;
            totalCalorias += alimento.Calorias * factor;

            // Agregar la cantidad ajustada de cada ingrediente a la lista
            ingredientesAjustados.Add(new 
            {
                NombreAlimento = alimento.Nombre,
                CantidadAjustada = cantidadEscalada,
                EsEnGramos = recetaAlimento.EsEnGramos
            });
        }

        // Devolver las macros por 1 ración y las cantidades ajustadas de los ingredientes
        return new
        {
            IngredientesAjustados = ingredientesAjustados,
            CaloriasPorRacion = totalCalorias,
            ProteinasPorRacion = totalProteinas,
            CarbohidratosPorRacion = totalCarbohidratos,
            GrasasPorRacion = totalGrasas
        };
    }

    private bool RecetaExists(int id)
    {
        return _context.Recetas.Any(e => e.Id == id);
    }
}
