using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.BatchTestResult
{
    public class UpdateBatchTestResultRequestDTO
    {
        [Required]
        public int BatchId { get; set; }
        [Required]
        public int TestResultId { get; set; }
    }
}
