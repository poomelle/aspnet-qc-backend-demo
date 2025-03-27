namespace ChemsonLab.API.Models.Domain
{
    public class Batch
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BatchName { get; set; }
        public string? SampleBy { get; set; }
        public string? Suffix { get; set; }

        //Navigation properties
        public Product Product { get; set; }
    }
}
