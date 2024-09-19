using Microsoft.AspNetCore.Mvc;
using AlimentosAPI.Data;
using AlimentosAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class AlimentosController : ControllerBase
{
    private readonly AlimentosContext _context;

    public AlimentosController(AlimentosContext context)
    {
        _context = context;
    }

    // Listar todos los alimentos ordenados por nombre
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Alimento>>> GetAlimentos(string sortBy = "nombre")
    {
        var alimentos = await _context.Alimentos
                                    .OrderBy(a => a.Nombre)
                                    .ToListAsync();

        return alimentos;
    }

    // Obtener un alimento por Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Alimento>> GetAlimento(int id)
    {
        var alimento = await _context.Alimentos.FindAsync(id);

        if (alimento == null)
        {
            return NotFound();
        }

        return alimento;
    }

    // Crear un nuevo alimento
    [HttpPost]
    public async Task<ActionResult<Alimento>> PostAlimento(Alimento alimento)
    {
        _context.Alimentos.Add(alimento);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAlimento", new { id = alimento.Id }, alimento);
    }

    // Editar un alimento
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAlimento(int id, Alimento alimento)
    {
        if (id != alimento.Id)
        {
            return BadRequest();
        }

        _context.Entry(alimento).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AlimentoExists(id))
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

    // Eliminar un alimento
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlimento(int id)
    {
        // Verificar si el alimento está en alguna receta
        bool alimentoEnReceta = await _context.RecetaAlimentos.AnyAsync(ra => ra.AlimentoId == id);
        if (alimentoEnReceta)
        {
            return BadRequest($"El alimento con ID {id} está asociado a una o más recetas y no puede ser eliminado.");
        }

        var alimento = await _context.Alimentos.FindAsync(id);
        if (alimento == null)
        {
            return NotFound();
        }

        _context.Alimentos.Remove(alimento);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AlimentoExists(int id)
    {
        return _context.Alimentos.Any(e => e.Id == id);
    }
}
