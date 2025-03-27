namespace ChemsonLab.API.Models.Domain
{
    public class TestResultReport
    {
        public int Id { get; set; }
        public int? ReportId { get; set; }
        public int? BatchTestResultId { get; set; }
        public string? StandardReference { get; set; }
        public double? TorqueDiff { get; set; }
        public double? FusionDiff { get; set; }
        public bool? Result { get; set; }
        public string? Comment { get; set; }
        public long? AveTestTime { get; set; }
        public string? FileLocation { get; set; }

        // Navigate properties
        public Report Report { get; set; }
        public BatchTestResult BatchTestResult { get; set; }

    }
}
