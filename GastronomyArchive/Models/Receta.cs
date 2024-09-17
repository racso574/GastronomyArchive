using AlimentosAPI.Models;
public class Receta
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public int CantidadPersonasBase { get; set; } // Nuevo campo para definir la cantidad de personas base
    public ICollection<RecetaAlimento> RecetaAlimentos { get; set; }
}


public class RecetaAlimento
{
    public int Id { get; set; }
    public int RecetaId { get; set; }
    public Receta Receta { get; set; }
    
    public int AlimentoId { get; set; }
    public Alimento Alimento { get; set; }
    
    // Cantidad del alimento, puede ser en gramos o en unidades.
    public decimal Cantidad { get; set; }

    // Este campo indicará si la cantidad es en gramos o unidades.
    public bool EsEnGramos { get; set; }
}
