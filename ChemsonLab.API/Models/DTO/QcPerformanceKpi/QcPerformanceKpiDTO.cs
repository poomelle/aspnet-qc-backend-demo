namespace ChemsonLab.API.Models.DTO.QcPerformanceKpi
{
    public class QcPerformanceKpiDTO
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public int? TotalTest { get; set; }
        public int? FirstPass { get; set; }
        public int? SecondPass { get; set; }
        public int? ThirdPass { get; set; }

        // Navigation properties
        public Domain.Product Product { get; set; }
        public Domain.Machine Machine { get; set; }
    }
}
