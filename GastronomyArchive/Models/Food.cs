namespace GastronomyArchive.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal CaloriesPer100g { get; set; }
        public decimal ProteinsPer100g { get; set; }
        public decimal FatsPer100g { get; set; }
        public decimal CarbsPer100g { get; set; }
        public decimal? AverageWeightGrams { get; set; } // Nullable para alimentos sin peso promedio
    }
}
