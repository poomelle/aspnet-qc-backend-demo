namespace ChemsonLab.API.Models.Domain
{
    public class TestResult
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MachineId { get; set; }
        public DateTime TestDate { get; set; }
        public string? OperatorName { get; set; }
        public string? DriveUnit { get; set; }
        public string? Mixer { get; set; }
        public string? LoadingChute { get; set; }
        public string? Additive { get; set; }
        public double? Speed { get; set; }
        public double? MixerTemp { get; set; }
        public double? StartTemp { get; set; }
        public int? MeasRange { get; set; }
        public int? Damping { get; set; }
        public double? TestTime { get; set; }
        public double? SampleWeight { get; set; }
        public string? CodeNumber { get; set; }
        public string? Plasticizer { get; set; }
        public double? PlastWeight { get; set; }
        public double? LoadTime { get; set; }
        public double? LoadSpeed { get; set; }
        public string? Liquid { get; set; }
        public double? Titrate { get; set; }
        public int? TestNumber { get; set; }
        public string? TestType { get; set; }
        public string? BatchGroup { get; set; }
        public string? TestMethod { get; set; }
        public string? Colour { get; set; }
        public bool? Status { get; set; }
        public string? FileName { get; set; }

        // Navigate properties
        public Product Product { get; set; }
        public Machine Machine { get; set; }
    }
}
