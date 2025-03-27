namespace ChemsonLab.API.Models.Domain
{
    public class Measurement
    {
        public long Id { get; set; }
        public int TestResultId { get; set; }
        public TimeSpan TimeAct { get; set; }
        public double? Torque { get; set; }
        public double? Bandwidth { get; set; }
        public double? StockTemp { get; set; }
        public double? Speed { get; set; }
        public string? FileName { get; set; }

        //Navigation properties
        public TestResult TestResult { get; set; }
    }
}
