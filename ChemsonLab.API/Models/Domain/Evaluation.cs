namespace ChemsonLab.API.Models.Domain
{
    public class Evaluation
    {
        public int Id { get; set; }
        public int TestResultId { get; set; }
        public int? Point { get; set; }
        public char? PointName { get; set; }
        public TimeSpan TimeEval { get; set; }
        public double? Torque { get; set; }
        public double? Bandwidth { get; set; }
        public double? StockTemp { get; set; }
        public double? Speed { get; set; }
        public double? Energy { get; set; }
        public TimeSpan TimeRange { get; set; }
        public double? TorqueRange { get; set; }
        public int? TimeEvalInt { get; set; }
        public int? TimeRangeInt { get; set; }
        public string? FileName { get; set; }

        //Navigation properties
        public TestResult TestResult { get; set; }
    }
}
