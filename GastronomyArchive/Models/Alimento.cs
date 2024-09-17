namespace AlimentosAPI.Models
{
    public class Alimento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Nombre del alimento
        public int Calorias { get; set; } // Calorías
        public decimal Grasas { get; set; } // Grasas en gramos
        public decimal Carbohidratos { get; set; } // Carbohidratos en gramos
        public decimal Proteinas { get; set; } // Proteínas en gramos
        public decimal? PesoUnidad { get; set; } // Peso por unidad opcional
    }
}
