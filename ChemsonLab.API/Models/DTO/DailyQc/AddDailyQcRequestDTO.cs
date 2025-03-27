namespace ChemsonLab.API.Models.DTO.DailyQc
{
    public class AddDailyQcRequestDTO
    {
        public DateTime IncomingDate { get; set; }
        public int ProductId { get; set; }
        public int? Priority { get; set; }
        public string? Comment { get; set; }
        public string? Batches { get; set; }
        public string? StdReqd { get; set; }
        public string? Extras { get; set; }
        public int? MixesReqd { get; set; }
        public int? Mixed { get; set; }
        public string? TestStatus { get; set; }
        public string? LastLabel { get; set; }
        public string? LastBatch { get; set; }
        public DateTime? TestedDate { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
    }
}
