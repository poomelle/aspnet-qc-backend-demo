using ChemsonLab.API.Models.DTO.BatchTestResult;
using ChemsonLab.API.Models.DTO.ProductSpecification;
using ChemsonLab.API.Models.DTO.Report;

namespace ChemsonLab.API.Models.DTO.TestResultReport
{
    public class TestResultReportDTO
    {
        public int Id { get; set; }
        public ReportDTO Report { get; set; }
        public BatchTestResultDTO BatchTestResult { get; set; }
        public string? StandardReference { get; set; }
        public double? TorqueDiff { get; set; }
        public double? FusionDiff { get; set; }
        public bool? Result { get; set; }
        public string? Comment { get; set; }
        public long? AveTestTime { get; set; }
        public string? FileLocation { get; set; }
    }
}
