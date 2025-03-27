using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.Measurement
{
    public class AddMeasurementRequestDTO
    {
        [Required]
        public int TestResultId { get; set; }
        public string? TimeAct { get; set; }
        public double? Torque { get; set; }
        public double? Bandwidth { get; set; }
        public double? StockTemp { get; set; }
        public double? Speed { get; set; }
        public string? FileName { get; set; }
    }
}
