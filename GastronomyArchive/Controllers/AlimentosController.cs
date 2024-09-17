using Microsoft.AspNetCore.Mvc;
using AlimentosAPI.Data;
using AlimentosAPI.Models;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AlimentosController : ControllerBase
{
    private readonly AlimentosContext _context;

    public AlimentosController(AlimentosContext context)
    {
        _context = context;
    }

    // GET: api/Alimentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Alimento>>> GetAlimentos()
    {
        return await _context.Alimentos.ToListAsync();
    }

    // GET: api/Alimentos/5
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

    // POST: api/Alimentos
    [HttpPost]
    public async Task<ActionResult<Alimento>> PostAlimento(Alimento alimento)
    {
        _context.Alimentos.Add(alimento);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAlimento", new { id = alimento.Id }, alimento);
    }

    // PUT: api/Alimentos/5
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

    // DELETE: api/Alimentos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlimento(int id)
    {
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
