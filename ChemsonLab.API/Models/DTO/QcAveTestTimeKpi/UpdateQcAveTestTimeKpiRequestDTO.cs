namespace ChemsonLab.API.Models.DTO.QcAveTestTimeKpi
{
    public class UpdateQcAveTestTimeKpiRequestDTO
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? MachineId { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public int? TotalTest { get; set; }
        public long? AveTestTime { get; set; }
    }
}
