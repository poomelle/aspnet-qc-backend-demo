using ChemsonLab.API.Models.DTO.Batch;
using ChemsonLab.API.Models.DTO.TestResult;

namespace ChemsonLab.API.Models.DTO.BatchTestResult
{
    public class BatchTestResultDTO
    {
        public int Id { get; set; }
        public BatchDTO Batch { get; set; }
        public TestResultDTO TestResult { get; set; }
        //public int BatchId { get; set; }
        //public int TestResultId { get; set; }
    }
}
