using ChemsonLab.API.Models.DTO.TestResult;

namespace ChemsonLab.API.Models.DTO.Evaluation
{
    public class EvaluationDTO
    {
        public int Id { get; set; }
        public TestResultDTO TestResult { get; set; }
        public int? Point { get; set; }
        public char? PointName { get; set; }
        public string? TimeEval { get; set; }
        public double? Torque { get; set; }
        public double? Bandwidth { get; set; }
        public double? StockTemp { get; set; }
        public double? Speed { get; set; }
        public double? Energy { get; set; }
        public string? TimeRange { get; set; }
        public double? TorqueRange { get; set; }
        public int? TimeEvalInt { get; set; }
        public int? TimeRangeInt { get; set; }
        public string? FileName { get; set; }
    }
}
