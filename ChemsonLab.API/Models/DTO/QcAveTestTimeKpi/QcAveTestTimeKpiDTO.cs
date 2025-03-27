namespace ChemsonLab.API.Models.DTO.QcAveTestTimeKpi
{
    public class QcAveTestTimeKpiDTO
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public int? TotalTest { get; set; }
        public long? AveTestTime { get; set; }

        // Navigation properties
        public Domain.Product Product { get; set; }
        public Domain.Machine Machine { get; set; }
    }
}
