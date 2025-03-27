using ChemsonLab.API.Models.DTO.TestResult;

namespace ChemsonLab.API.Models.DTO.Measurement
{
    public class MeasurementDTO
    {
        public long Id { get; set; }
        public TestResultDTO TestResult { get; set; }
        public string? TimeAct { get; set; }
        public double? Torque { get; set; }
        public double? Bandwidth { get; set; }
        public double? StockTemp { get; set; }
        public double? Speed { get; set; }
        public string? FileName { get; set; }
    }
}
