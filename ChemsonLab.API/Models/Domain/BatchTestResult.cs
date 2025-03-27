namespace ChemsonLab.API.Models.Domain
{
    public class BatchTestResult
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int TestResultId { get; set; }

        //Navigation properties
        public Batch Batch { get; set; }
        public TestResult TestResult { get; set; }

    }
}
