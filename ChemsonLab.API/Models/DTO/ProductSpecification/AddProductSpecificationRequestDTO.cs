using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.ProductSpecification
{
    public class AddProductSpecificationRequestDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int MachineId { get; set; }
        public bool? InUse { get; set; }
        public int? Temp { get; set; }
        public int? Load { get; set; }
        public int? RPM { get; set; }

    }
}
