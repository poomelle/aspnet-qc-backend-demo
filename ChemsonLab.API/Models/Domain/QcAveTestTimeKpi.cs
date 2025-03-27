namespace ChemsonLab.API.Models.Domain
{
    public class QcAveTestTimeKpi
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? MachineId { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public int? TotalTest  { get; set; }
        public long? AveTestTime { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public Machine Machine { get; set; }
    }
}
