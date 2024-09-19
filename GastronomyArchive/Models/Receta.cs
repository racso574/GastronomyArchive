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
    public int RecetaId { get; set; }  // Clave foránea a la tabla Receta
    public int AlimentoId { get; set; }  // Clave foránea a la tabla Alimento
    public decimal Cantidad { get; set; }
    public bool EsEnGramos { get; set; }
}


