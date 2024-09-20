using Microsoft.AspNetCore.Mvc;
using AlimentosAPI.Data;
using AlimentosAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    // Obtener todas las recetas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Receta>>> GetRecetas()
    {
        var recetas = await _context.Recetas
            .Include(r => r.RecetaAlimentos)
            .ToListAsync();

        return Ok(recetas);
    }

    // Obtener una receta por Id con ajuste opcional de personas
[HttpGet("{id}")]
public async Task<ActionResult<dynamic>> GetReceta(int id, [FromQuery] int? personas = null)
{
    var receta = await _context.Recetas
        .Include(r => r.RecetaAlimentos)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (receta == null)
    {
        return NotFound();
    }

    // Si no se proporciona el parámetro personas, usar la cantidad de personas base
    int personasCalculadas = personas ?? receta.CantidadPersonasBase;

    // Ajustar las cantidades de los ingredientes según el número de personas
    var ingredientesAjustados = receta.RecetaAlimentos.Select(ra => new 
    {
        NombreAlimento = _context.Alimentos.FirstOrDefault(a => a.Id == ra.AlimentoId)?.Nombre ?? "Desconocido",
        CantidadAjustada = ra.Cantidad * personasCalculadas / receta.CantidadPersonasBase,
        EsEnGramos = ra.EsEnGramos
    }).ToList();

    return new
    {
        Nombre = receta.Nombre,
        Descripcion = receta.Descripcion,
        CantidadPersonas = personasCalculadas,
        Ingredientes = ingredientesAjustados
    };
}


    [HttpPut("{id}")]
public async Task<IActionResult> PutReceta(int id, Receta receta)
{
    if (id != receta.Id)
    {
        return BadRequest();
    }

    // Obtener la receta actual con sus RecetaAlimentos
    var recetaExistente = await _context.Recetas
        .Include(r => r.RecetaAlimentos)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (recetaExistente == null)
    {
        return NotFound();
    }

    // Comparar la lista de alimentos actual con la nueva lista de alimentos
    var alimentosExistentesIds = recetaExistente.RecetaAlimentos.Select(ra => ra.AlimentoId).ToList();
    var nuevosAlimentosIds = receta.RecetaAlimentos.Select(ra => ra.AlimentoId).ToList();

    // Eliminar los alimentos que ya no están en la receta actualizada
    var alimentosAEliminar = recetaExistente.RecetaAlimentos
        .Where(ra => !nuevosAlimentosIds.Contains(ra.AlimentoId))
        .ToList();

    if (alimentosAEliminar.Any())
    {
        _context.RecetaAlimentos.RemoveRange(alimentosAEliminar);
    }

    // Agregar nuevos alimentos a la receta
    var alimentosAAgregar = receta.RecetaAlimentos
        .Where(ra => !alimentosExistentesIds.Contains(ra.AlimentoId))
        .ToList();

    if (alimentosAAgregar.Any())
    {
        foreach (var nuevoAlimento in alimentosAAgregar)
        {
            recetaExistente.RecetaAlimentos.Add(new RecetaAlimento
            {
                RecetaId = receta.Id,
                AlimentoId = nuevoAlimento.AlimentoId,
                Cantidad = nuevoAlimento.Cantidad,
                EsEnGramos = nuevoAlimento.EsEnGramos
            });
        }
    }

    // Actualizar las cantidades de los alimentos existentes
    foreach (var alimentoExistente in recetaExistente.RecetaAlimentos)
    {
        var alimentoActualizado = receta.RecetaAlimentos
            .FirstOrDefault(ra => ra.AlimentoId == alimentoExistente.AlimentoId);

        if (alimentoActualizado != null)
        {
            alimentoExistente.Cantidad = alimentoActualizado.Cantidad;
            alimentoExistente.EsEnGramos = alimentoActualizado.EsEnGramos;
        }
    }

    // Actualizar el resto de la receta (nombre, descripción, etc.)
    recetaExistente.Nombre = receta.Nombre;
    recetaExistente.Descripcion = receta.Descripcion;
    recetaExistente.CantidadPersonasBase = receta.CantidadPersonasBase;

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
        var receta = await _context.Recetas
            .Include(r => r.RecetaAlimentos)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (receta == null)
        {
            return NotFound();
        }

        // Eliminar las relaciones en RecetaAlimentos antes de eliminar la receta
        _context.RecetaAlimentos.RemoveRange(receta.RecetaAlimentos);

        _context.Recetas.Remove(receta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

   
   
    // Calcular macros de una receta para 1 persona
[HttpGet("{id}/calcular-macros")]
public async Task<ActionResult<dynamic>> CalcularMacros(int id)
{
    var receta = await _context.Recetas
        .Include(r => r.RecetaAlimentos)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (receta == null)
    {
        return NotFound();
    }

    var alimentoIds = receta.RecetaAlimentos.Select(ra => ra.AlimentoId).ToList();

    var alimentos = await _context.Alimentos
        .Where(a => alimentoIds.Contains(a.Id))
        .ToDictionaryAsync(a => a.Id);

    decimal totalProteinas = 0, totalCarbohidratos = 0, totalGrasas = 0, totalCalorias = 0;

    var ingredientesAjustados = new List<dynamic>();

    // Ajustar cantidades para 1 persona
    foreach (var recetaAlimento in receta.RecetaAlimentos)
    {
        if (!alimentos.TryGetValue(recetaAlimento.AlimentoId, out var alimento))
        {
            return NotFound($"Alimento con ID {recetaAlimento.AlimentoId} no encontrado.");
        }

        // Ajustar las cantidades para 1 persona
        var cantidadEscalada = recetaAlimento.Cantidad / receta.CantidadPersonasBase;
        var cantidadEnGramos = recetaAlimento.EsEnGramos 
            ? cantidadEscalada 
            : (cantidadEscalada * (alimento.PesoUnidad ?? 0));

        var factor = cantidadEnGramos / 100;

        totalProteinas += alimento.Proteinas * factor;
        totalCarbohidratos += alimento.Carbohidratos * factor;
        totalGrasas += alimento.Grasas * factor;
        totalCalorias += alimento.Calorias * factor;

        ingredientesAjustados.Add(new 
        {
            NombreAlimento = alimento.Nombre,
            CantidadAjustada = cantidadEscalada,
            EsEnGramos = recetaAlimento.EsEnGramos
        });
    }

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
