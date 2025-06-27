using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.Batch
{
    public class UpdateBatchRequestDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string BatchName { get; set; }
        public string? SampleBy { get; set; }
        public string? Suffix { get; set; }
    }
}
