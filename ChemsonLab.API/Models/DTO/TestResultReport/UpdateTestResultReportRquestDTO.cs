using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.TestResultReport
{
    public class UpdateTestResultReportRquestDTO
    {
        public int? ReportId { get; set; }
        public int? BatchTestResultId { get; set; }
        public string? StandardReference { get; set; }
        public double? TorqueDiff { get; set; }
        public double? FusionDiff { get; set; }
        public bool? Result { get; set; }
        public string? Comment { get; set; }
        public long? AveTestTime { get; set; }
        public string? FileLocation { get; set; }
    }
}
