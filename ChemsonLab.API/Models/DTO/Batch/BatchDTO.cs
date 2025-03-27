using ChemsonLab.API.Models.DTO.Product;

namespace ChemsonLab.API.Models.DTO.Batch
{
    public class BatchDTO
    {
        public int Id { get; set; }
        public string BatchName { get; set; }
        public string? SampleBy { get; set; }
        public string? Suffix { get; set; }

        // Navigation properties
        public ProductDTO Product { get; set; }
    }
}
